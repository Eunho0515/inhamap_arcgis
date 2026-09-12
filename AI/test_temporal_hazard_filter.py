import unittest

from temporal_hazard_filter import TemporalHazardFilter


class TemporalHazardFilterTests(unittest.TestCase):
    def make_filter(self) -> TemporalHazardFilter:
        return TemporalHazardFilter(
            width=720,
            height=1280,
            window_frames=5,
            minimum_hits=3,
            minimum_average_confidence=0.3,
            maximum_misses=2,
        )

    def test_single_frame_false_alarm_is_rejected(self) -> None:
        filter_ = self.make_filter()
        box = (100, 200, 200, 300, "fire", 0.8)
        self.assertEqual(filter_.update([box]), [])
        self.assertEqual(filter_.update([]), [])
        self.assertEqual(filter_.update([]), [])

    def test_persistent_detection_is_confirmed(self) -> None:
        filter_ = self.make_filter()
        outputs = []
        for offset in range(3):
            outputs = filter_.update(
                [(100 + offset, 200, 200 + offset, 300, "fire", 0.7)]
            )
        self.assertEqual(len(outputs), 1)
        self.assertEqual(outputs[0][4], "fire")

    def test_classes_are_not_joined_into_one_track(self) -> None:
        filter_ = self.make_filter()
        for _ in range(3):
            outputs = filter_.update(
                [
                    (100, 200, 200, 300, "fire", 0.7),
                    (100, 200, 200, 300, "smoke", 0.7),
                ]
            )
        self.assertEqual({box[4] for box in outputs}, {"fire", "smoke"})


if __name__ == "__main__":
    unittest.main()
