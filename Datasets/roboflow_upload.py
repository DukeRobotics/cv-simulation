from typing_extensions import TypedDict
from roboflow import Roboflow
import pathlib
import yaml
import argparse
import textwrap
from pydantic import TypeAdapter, ValidationError
import concurrent.futures
import functools

DATASETS_PATH = pathlib.Path(__file__).parent.resolve()
ROBOFLOW_CONFIG_FILE = DATASETS_PATH / "roboflow_config.yaml"


class RoboflowConfig(TypedDict):
    api_key: str

    workspace: str
    project: str


def create_roboflow_project_config_file():
    """
    Generate a yaml config file for uploading to Roboflow.
    """
    # YAML content with comments
    yaml_content = textwrap.dedent(
        """\
        # Configuration file for Roboflow upload

        # Go to https://app.roboflow.com/settings/account.
        # Navigate to the desired workspace > Roboflow API and copy/paste your Private API Key.
        api_key: ~

        # Navigate to the desired project version and copy/paste the following information from the URL:
        # https://app.roboflow.com/<workspace>/<project>/*
        workspace: ~
        project: ~
    """
    )

    # Write YAML content to a file
    with open(ROBOFLOW_CONFIG_FILE, "w") as yaml_file:
        yaml_file.write(yaml_content)


def load_roboflow_config():
    """
    Load the Roboflow configuration file as a dictionary.

    Raises:
        SystemExit: If the schema of the configuration file is invalid.
    """
    # Load file
    with open(ROBOFLOW_CONFIG_FILE, "r") as file:
        config = yaml.load(file, Loader=yaml.FullLoader)

    # Runtime type check
    SomeDictValidator = TypeAdapter(RoboflowConfig)
    try:
        return SomeDictValidator.validate_python(config)
    except ValidationError as e:
        raise SystemExit(f"Invalid schema for {ROBOFLOW_CONFIG_FILE.name}. {e}")


def upload_image(image_path: pathlib.Path, project: Roboflow, annotation_path: pathlib.Path,
                 success_path: pathlib.Path):
    """
    Upload an image to Roboflow.

    Args:
        image_path: Path to the image file.
        project: Roboflow project object.
        annotation_path: Path to the annotation file.
        success_path: Path to save the image file after successful upload.
    """
    NUM_RETRY_UPLOADS = 5
    results = project.single_upload(
        image_path=str(image_path),
        annotation_path=annotation_path.resolve(),
        NUM_RETRY_UPLOADS=NUM_RETRY_UPLOADS,
    )
    print(results)

    if (
        results.get("image").get("success") is True
        and results.get("annotation").get("success") is True
    ):
        image_path.rename(success_path / image_path.name)  # Move the image to the success folder


def upload_dataset(config: RoboflowConfig, dataset_path: pathlib.Path):
    """
    Upload a dataset to Roboflow.

    Args:
        config: Roboflow configuration dictionary.
        dataset_path: Path to the COCO dataset to upload.
    """
    # Initialize Roboflow client
    rf = Roboflow(api_key=config["api_key"])
    project = rf.workspace(config["workspace"]).project(config["project"])

    # Annotation file path
    annotation_filename = dataset_path / "bbox.json"

    # Images to upload
    image_glob = (dataset_path / "images").glob("*.png")

    # Create a success folder
    success_path = dataset_path / "success"
    success_path.mkdir(parents=True, exist_ok=True)

    # Upload images
    with concurrent.futures.ThreadPoolExecutor() as executor:
        upload_image_closed = functools.partial(
            upload_image,
            project=project,
            annotation_filename=annotation_filename,
            success_path=success_path,
        )
        executor.map(upload_image_closed, list(image_glob))


if __name__ == "__main__":
    if not (ROBOFLOW_CONFIG_FILE).exists():
        create_roboflow_project_config_file()
        raise SystemExit(
            f"Missing {ROBOFLOW_CONFIG_FILE.name} file. Please fill in the required fields."
        )

    parser = argparse.ArgumentParser(description="Upload a dataset to Roboflow.")
    parser.add_argument("dataset_path", help="Path to the COCO dataset to upload.")
    config: RoboflowConfig = load_roboflow_config()

    args = parser.parse_args()

    upload_dataset(config, pathlib.Path(args.dataset_path))
