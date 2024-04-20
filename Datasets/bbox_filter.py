import json
import argparse
import pathlib
from collections import defaultdict

DATASETS_PATH = pathlib.Path(__file__).parent.resolve()

class ImageWithBBoxes:
    def __init__(self, bboxes):
        self.bboxes = defaultdict(dict)
        self.max_area_by_category = {}
        self.counts = {}

        for bbox in bboxes:
            if (image_id := bbox["image_id"]) not in self.max_area_by_category:
                self.max_area_by_category[image_id] = {}
                self.counts[image_id] = {}

            if (category_id := bbox["category_id"]) not in self.max_area_by_category[image_id]:
                self.max_area_by_category[image_id][category_id] = bbox["area"]
                self.counts[image_id][category_id] = 0
            else:
                self.max_area_by_category[image_id][category_id] = max(self.max_area_by_category[image_id][category_id], bbox["area"])
                self.counts[image_id][category_id] += 1

            self.bboxes[bbox["id"]] = bbox
        
        for image_id, category_ids in self.counts.items():
            for category_id, count in category_ids.items():
                if count > 1:
                    print(image_id, category_id)
      
        self.filter()

    def filter(self):
        remove = []
        for id, bbox in self.bboxes.items():
            # Remove bboxes with area 1 or smaller duplicates
            if bbox["area"] <= 5 or bbox["area"] < self.max_area_by_category[bbox["image_id"]][bbox["category_id"]]:
                remove.append(id)

        for id in remove:
            del self.bboxes[id]

    def export(self):
        return [self.bboxes[id] for id in self.bboxes]


def filter(filename: pathlib.Path):
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

        # Export each image
        export_jsons = []
        for image in images:
            bbox_list = image.export()
            export_jsons.extend(bbox_list)
        superdirectory["annotations"] = export_jsons

        # Write the dictionary to a JSON file
        with open(filename, "w") as file:
            json.dump(superdirectory, file, indent=4)
        

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="Filter COCO bounding boxes.")
    parser.add_argument("dataset_path", help="Path to the COCO dataset to filter.", type=pathlib.Path)

    args = parser.parse_args()

    filter(DATASETS_PATH / args.dataset_path / "bbox.json")