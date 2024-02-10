import glob
from roboflow import Roboflow

# Initialize Roboflow client
rf = Roboflow(api_key="iEPfFiljtTJ0TV7wuwSq")

# Directory path and file extension for images
dir_name = "coco/images"
file_extension_type = ".png"

# Annotation file path and format (e.g., .coco.json)
annotation_filename = "coco/bbox.json"

# Get the upload project from Roboflow workspace
project = rf.workspace("duke-robotics-club-2024").project("buoy-glyphs")

# Upload images
image_glob = glob.glob(dir_name + '/*' + file_extension_type)

success_images = []
failed_images = []
for image_path in image_glob:
    results = project.single_upload(
        image_path=image_path,
        annotation_path=annotation_filename,
        num_retry_uploads=3,
        batch_name='feb8',
    )
