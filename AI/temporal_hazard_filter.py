from __future__ import annotations

from collections import deque
from dataclasses import dataclass, field
from typing import Iterable


Box = tuple[int, int, int, int, str, float]


def box_iou(a: Box, b: Box) -> float:
    ax1, ay1, ax2, ay2 = a[:4]
    bx1, by1, bx2, by2 = b[:4]
    intersection = max(0, min(ax2, bx2) - max(ax1, bx1)) * max(
        0, min(ay2, by2) - max(ay1, by1)
    )
    if intersection == 0:
        return 0.0
    union = (ax2 - ax1) * (ay2 - ay1) + (bx2 - bx1) * (by2 - by1) - intersection
    return intersection / max(union, 1)


def normalized_center_distance(a: Box, b: Box, width: int, height: int) -> float:
    acx, acy = (a[0] + a[2]) * 0.5, (a[1] + a[3]) * 0.5
    bcx, bcy = (b[0] + b[2]) * 0.5, (b[1] + b[3]) * 0.5
    dx = (acx - bcx) / max(width, 1)
    dy = (acy - bcy) / max(height, 1)
    return (dx * dx + dy * dy) ** 0.5


@dataclass
class Track:
    track_id: int
    box: Box
    history: deque[float]
    misses: int = 0
    confirmed: bool = False


@dataclass
class TemporalHazardFilter:
    width: int
    height: int
    window_frames: int = 10
    minimum_hits: int = 5
    minimum_average_confidence: float = 0.35
    minimum_iou: float = 0.15
    maximum_center_distance: float = 0.08
    maximum_misses: int = 4
    class_thresholds: dict[str, float] = field(
        default_factory=lambda: {"fire": 0.25, "smoke": 0.25}
    )

    def __post_init__(self) -> None:
        if not 1 <= self.minimum_hits <= self.window_frames:
            raise ValueError("minimum_hits must be in [1, window_frames]")
        self._tracks: list[Track] = []
        self._next_track_id = 1

    def update(self, detections: Iterable[Box]) -> list[Box]:
        candidates = [
            box
            for box in detections
            if box[4].lower() in self.class_thresholds
            and box[5] >= self.class_thresholds[box[4].lower()]
        ]
        unmatched_tracks = set(range(len(self._tracks)))
        unmatched_boxes = set(range(len(candidates)))
        matches: list[tuple[int, int]] = []

        scored_pairs: list[tuple[float, int, int]] = []
        for track_index, track in enumerate(self._tracks):
            for box_index, box in enumerate(candidates):
                if track.box[4].lower() != box[4].lower():
                    continue
                overlap = box_iou(track.box, box)
                distance = normalized_center_distance(
                    track.box, box, self.width, self.height
                )
                if overlap >= self.minimum_iou or distance <= self.maximum_center_distance:
                    scored_pairs.append((overlap - distance, track_index, box_index))

        for _, track_index, box_index in sorted(scored_pairs, reverse=True):
            if track_index not in unmatched_tracks or box_index not in unmatched_boxes:
                continue
            unmatched_tracks.remove(track_index)
            unmatched_boxes.remove(box_index)
            matches.append((track_index, box_index))

        for track_index, box_index in matches:
            track = self._tracks[track_index]
            track.box = candidates[box_index]
            track.history.append(candidates[box_index][5])
            track.misses = 0
            self._update_confirmation(track)

        for track_index in unmatched_tracks:
            track = self._tracks[track_index]
            track.history.append(0.0)
            track.misses += 1
            self._update_confirmation(track)

        for box_index in unmatched_boxes:
            box = candidates[box_index]
            history: deque[float] = deque(maxlen=self.window_frames)
            history.append(box[5])
            self._tracks.append(Track(self._next_track_id, box, history))
            self._next_track_id += 1

        self._tracks = [track for track in self._tracks if track.misses <= self.maximum_misses]
        return [
            track.box
            for track in self._tracks
            if track.confirmed and track.misses == 0
        ]

    def _update_confirmation(self, track: Track) -> None:
        confidences = [value for value in track.history if value > 0]
        hits = len(confidences)
        average = sum(confidences) / hits if hits else 0.0
        if not track.confirmed:
            track.confirmed = (
                hits >= self.minimum_hits
                and average >= self.minimum_average_confidence
            )
        elif track.misses > self.maximum_misses:
            track.confirmed = False
