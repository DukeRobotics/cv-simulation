
import json
import argparse
import pathlib

from collections import defaultdict

DATASETS_PATH = pathlib.Path(__file__).parent.resolve()

class ImageWithBBoxes:

    # Default threshold is to eliminate bounding boxes less than 10% of the maximum size bounding box
    def __init__(self, bboxes, threshold=0.1):

        self.threshold = threshold
        self.bboxes = defaultdict(dict)
        self.max_area = float("-inf")

        for bbox in bboxes:
            if bbox["area"] >= self.max_area:
                self.max_area = bbox["area"]
            self.bboxes[bbox["id"]] = bbox

        # Filter/remove all bounding boxes less than threshold% of the max_area bounding box        
        self.filter()

    def filter(self):
        # remove = [self.bboxes[bbox]["id"] for bbox in self.bboxes if self.bboxes[bbox]["area"] <= (0.1 * self.max_area) or self.bboxes[bbox]["area"] == 1]
        remove = [self.bboxes[bbox]["id"] for bbox in self.bboxes if self.bboxes[bbox]["area"] == 1]
        for id in remove:
            print(f"Removed id {id}")
            del self.bboxes[id]

    def export(self):
        return [self.bboxes[id] for id in self.bboxes]


def filter(filename: pathlib.Path):

    # Open the JSON file for reading
    with open(filename, "r") as file:

        # Parse the JSON file and convert it into a Python dictionary
        superdirectory = json.load(file)
        annotations = superdirectory["annotations"]

        # Collect all bounding boxes for the same image under one list
        images_to_bbox = defaultdict(list)
        for bbox in annotations:
            images_to_bbox[bbox["image_id"]].append(bbox)

        # Convert into image objects
        images = []
        for id in images_to_bbox:
            images.append(ImageWithBBoxes(images_to_bbox[id]))

        export_jsons = []
        for image in images:
            bbox_list = image.export()
            export_jsons.extend(bbox_list)

        superdirectory["annotations"] = export_jsons
        # Write the dictionary to a JSON file
        with open(filename, "w") as file:
            json.dump(superdirectory, file, indent=4)
        print("Success")
        

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="Upload a dataset to Roboflow.")
    parser.add_argument("dataset_path", help="Path to the COCO dataset to filter.", type=pathlib.Path)

    args = parser.parse_args()

    filter(DATASETS_PATH / args.dataset_path / "bbox.json")