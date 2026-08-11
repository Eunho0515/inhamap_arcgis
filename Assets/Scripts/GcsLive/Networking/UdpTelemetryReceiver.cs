using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Peh.Gcs.Live.Data;
using UnityEngine;

namespace Peh.Gcs.Live.Networking
{
    /// <summary>
    /// Receives UTF-8 JSON datagrams on a worker thread and publishes validated packets
    /// on Unity's main thread. It intentionally does not parse raw MAVLink frames.
    /// </summary>
    public sealed class UdpTelemetryReceiver : MonoBehaviour
    {
        [SerializeField, Min(1)] private int listenPort = 5005;
        [SerializeField, Min(1)] private int maxDatagramBytes = 16 * 1024;
        [SerializeField, Min(1)] private int maxPacketsPerFrame = 32;
        [SerializeField, Min(0.1f)] private float staleAfterSeconds = 2f;

        private readonly ConcurrentQueue<ReceivedDatagram> pending = new();
        private UdpClient client;
        private Thread receiveThread;
        private volatile bool running;
        private long lastAcceptedSequence = -1;
        private float lastPacketRealtime = float.NegativeInfinity;

        public int ListenPort => listenPort;
        public bool IsListening => running;
        public bool HasTelemetry { get; private set; }
        public bool IsStale => !HasTelemetry || Time.realtimeSinceStartup - lastPacketRealtime > staleAfterSeconds;
        public LiveTelemetryPacket Latest { get; private set; }
        public IPEndPoint LastSender { get; private set; }
        public string LastError { get; private set; }
        public long AcceptedPacketCount { get; private set; }
        public long DroppedPacketCount { get; private set; }

        public event Action<LiveTelemetryPacket> TelemetryUpdated;

        private void OnEnable() => StartListening();

        private void Update()
        {
            var handled = 0;
            while (handled++ < maxPacketsPerFrame && pending.TryDequeue(out var datagram))
                Process(datagram);
        }

        private void OnDisable() => StopListening();
        private void OnDestroy() => StopListening();

        public void StartListening()
        {
            if (running)
                return;

            try
            {
                client = new UdpClient(listenPort);
                client.Client.ReceiveBufferSize = Math.Max(client.Client.ReceiveBufferSize, maxDatagramBytes * 4);
                running = true;
                LastError = null;
                receiveThread = new Thread(ReceiveLoop)
                {
                    IsBackground = true,
                    Name = $"GCS UDP telemetry :{listenPort}"
                };
                receiveThread.Start();
            }
            catch (Exception exception)
            {
                running = false;
                LastError = $"Could not listen on UDP {listenPort}: {exception.Message}";
                Debug.LogError(LastError, this);
            }
        }

        public void StopListening()
        {
            running = false;
            client?.Close();
            client = null;

            if (receiveThread != null && receiveThread.IsAlive)
                receiveThread.Join(250);
            receiveThread = null;
        }

        private void ReceiveLoop()
        {
            while (running)
            {
                try
                {
                    var sender = new IPEndPoint(IPAddress.Any, 0);
                    var bytes = client.Receive(ref sender);
                    if (bytes.Length > maxDatagramBytes)
                    {
                        pending.Enqueue(new ReceivedDatagram(null, sender, "Datagram exceeds the configured size limit."));
                        continue;
                    }

                    pending.Enqueue(new ReceivedDatagram(Encoding.UTF8.GetString(bytes), sender, null));
                }
                catch (ObjectDisposedException)
                {
                    return;
                }
                catch (SocketException exception)
                {
                    if (running)
                        pending.Enqueue(new ReceivedDatagram(null, null, exception.Message));
                }
                catch (Exception exception)
                {
                    pending.Enqueue(new ReceivedDatagram(null, null, exception.Message));
                }
            }
        }

        private void Process(ReceivedDatagram datagram)
        {
            if (datagram.Error != null)
            {
                Reject(datagram.Error);
                return;
            }

            LiveTelemetryPacket packet;
            try
            {
                packet = JsonUtility.FromJson<LiveTelemetryPacket>(datagram.Json);
            }
            catch (Exception exception)
            {
                Reject($"Invalid telemetry JSON: {exception.Message}");
                return;
            }

            string validationError = null;
            if (packet == null || !packet.TryValidate(out validationError))
            {
                Reject(validationError ?? "Telemetry JSON was empty.");
                return;
            }

            if (lastAcceptedSequence >= 0 && packet.sequence_number <= lastAcceptedSequence)
            {
                Reject($"Out-of-order sequence {packet.sequence_number}; latest is {lastAcceptedSequence}.");
                return;
            }

            lastAcceptedSequence = packet.sequence_number;
            Latest = packet;
            LastSender = datagram.Sender;
            LastError = null;
            HasTelemetry = true;
            lastPacketRealtime = Time.realtimeSinceStartup;
            AcceptedPacketCount++;
            TelemetryUpdated?.Invoke(packet);
        }

        private void Reject(string message)
        {
            DroppedPacketCount++;
            LastError = message;
        }

        private readonly struct ReceivedDatagram
        {
            public ReceivedDatagram(string json, IPEndPoint sender, string error)
            {
                Json = json;
                Sender = sender;
                Error = error;
            }

            public string Json { get; }
            public IPEndPoint Sender { get; }
            public string Error { get; }
        }
    }
}
