from __future__ import annotations
import json
import argparse
import pathlib
from collections import defaultdict

DATASETS_PATH = pathlib.Path(__file__).parent.resolve()


class ImageWithBBoxes:
    """
    Stores all bounding boxes for a single image.
    """

    def __init__(self, bboxes: list[dict], image_id: int):
        """
        Initializes the ImageWithBBoxes object.

        Args:
            bboxes: List of bounding boxes that have the same image_id.
            image_id: The image_id of the image.
        """
        self.bboxes: dict[int, dict] = defaultdict(dict)  # Maps bbox id to bbox
        self.max_area: dict[int, int] = {}  # Maximum area bbox for each category
        self.counts: dict[int, int] = {}  # Number of bboxes for each category
        self.image_id = image_id  # image_id of the image

        for bbox in bboxes:
            category_id = bbox["category_id"]
            self.max_area[category_id] = max(self.max_area.get(category_id, 0), bbox["area"])
            self.counts[category_id] = self.counts.get(category_id, 0) + 1

            self.bboxes[bbox["id"]] = bbox

    def filter(self):
        """
        Filter out bad bounding boxes.

        The following bounding boxes are removed:
        - Bounding boxes with area MAX_AREA or smaller
        - All bounding boxes with area smaller than the maximum area for its category
        """
        MAX_AREA = 50  # May need to be adjusted for different simulations

        # Identify bad bounding boxes
        remove = []
        for id, bbox in self.bboxes.items():
            if bbox["area"] <= MAX_AREA or bbox["area"] < self.max_area[bbox["category_id"]]:
                remove.append(id)

        # Remove bad bounding boxes
        for id in remove:
            del self.bboxes[id]

    def export(self) -> list[dict]:
        """
        Returns:
            List of bounding boxes for the image.
        """
        return list(self.bboxes.values())


def filter(bbox_path: pathlib.Path):
    """
    Filter the bounding boxes of a COCO dataset.

    Args:
        bbox_path: Path to the JSON file containing the bounding boxes.
    """
    with open(bbox_path, "r") as file:
        # Parse the JSON file and convert it into a dictionary
        super_dict = json.load(file)
        annotations = super_dict["annotations"]

        # Collect all bounding boxes for the same image under one list
        raw_images = defaultdict(list)
        for bbox in annotations:
            raw_images[bbox["image_id"]].append(bbox)

        # Convert each image into an ImageWithBBoxes object
        images = []
        for image_id, image in raw_images.items():
            images.append(ImageWithBBoxes(image, image_id))

        # Export each image
        export_jsons = []
        for image_with_bboxes in images:
            image_with_bboxes.filter()
            bbox_list = image_with_bboxes.export()
            export_jsons.extend(bbox_list)
        super_dict["annotations"] = export_jsons

        # Write back the dictionary as JSON
        with open(bbox_path, "w") as file:
            json.dump(super_dict, file, indent=4)


if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="Filter COCO bounding boxes.")
    parser.add_argument("dataset_path", help="Path to the COCO dataset to filter.", type=pathlib.Path)

    args = parser.parse_args()

    filter(DATASETS_PATH / args.dataset_path / "bbox.json")
