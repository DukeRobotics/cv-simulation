# CV Simulation

A Unity 3D underwater simulation to generate synthetic, labeled data for computer vision.

## Simulation Overview
The simulation was built using Unity 2022 LTS. It uses the HDRP render pipeline and [Unity Perception](https://docs.unity3d.com/Packages/com.unity.perception@1.0/manual/index.html). See [this tutorial](https://github.com/Unity-Technologies/com.unity.perception/blob/main/com.unity.perception/Documentation~/Tutorial/TUTORIAL.md) to get started with Unity Perception.

The simulation is setup to generate synthetic images of the gate and buoy with automatically generated bounding boxes for the RoboSub 2023 glyphs. The simulation occurs underwater, in a simulated swimming pool with murky water.

The camera captures the gate and buoy from a variety of distances and angles. In addition to the camera movements, the sun's position, buoy and gate glyphs, and gate boxes are also randomized. This ensures that the camera captures the gate and buoy under a variety of different conditions leading to a more robust CV model.

## Code Overview
The code is written in C# in the following files:
- [CameraController.cs](/Assets/CameraController.cs): Controls the camera movements.
- [RandomizedGateHue.cs](/Assets/RandomizeGateHue.cs): Randomizes the hue of the gate.
- [RotateGlyphs.cs](/Assets/RotateGlyphs.cs): Randomizes the orientation of the buoy glyphs.
- [RandomizeGateGlyphs.cs](/Assets/RandomizeGateGlyphs.cs): Randomizes the order and orientation of the gate glyphs.
- [RandomizeBoxes.cs](/Assets/RandomizeBoxes.cs): Randomizes the order of the gate boxes.
- [Sun.cs](/Assets/Sun.cs): Randomizes the sun's position.

## Dependencies
- [Unity Hub](https://unity.com/download)
- Unity 2022 LTS: Install through Unity Hub.
- [Miniconda](https://docs.conda.io/projects/miniconda/en/latest/)

Once Miniconda is installed, create the `cv-sim` environment with:
```python
conda env create -f environment.yml
```

Then, activate the `cv-sim` environment with:
```python
conda activate cv-sim
```

Lastly, due to a [backwards compatiability issue](https://github.com/opencv/opencv-python/issues/884) with `opencv`, downgrade `opencv` with:
```python
pip install opencv-python==4.8.0.74
```
An error about dependency resolution may appear. This can be safely ignored.

In the future, only the `conda activate cv-sim` command needs to be executed to set up the Python environment for this repository.

## Running the Simulation
> [!IMPORTANT]
> Before running the simulation for the first time, the dataset generation path needs to be specified. Open the Project Settings window by selecting the menu `Edit > Project Settings...` and switch to the `Perception` pane. Change the `Base Path` to the `Datasets` directory of this repository.

Use the play button at the top of the Unity editor to run the simulation. The following option controls whether the simulation saves generated images and their corresponding bounding boxes to disk:
- Open the inspector for "Main Camera"
- Scroll down in the inspector to "Perception Camera (Script)"
- Check/uncheck "Save Camera RGB Output to Disk"

_Note: Generated datasets can occupy a lot of disk space._

Each time the simulation is run with the above option checked, a new folder is created inside the `Datasets` directory.

The new folder contains the generated images and bounding boxes in [SOLO](https://docs.unity3d.com/Packages/com.unity.perception@1.0/manual/Schema/SoloSchema.html) format, created by Unity.

## Visualizing SOLO Datasets
To visualize a SOLO dataset, use [Voxel51](https://voxel51.com):
```python
pysolotools-fiftyone "Datasets/<SOLO Dataset>"
```

> [!NOTE]
> The viewer will initially open with a subset of frames visible. The data is still being imported in the background. Once all frames are imported, click the "FiftyOne" button in the top left to update the viewer.

## Converting from SOLO
To convert from SOLO to COCO format, run:
```python
solo2coco "Datasets/<SOLO Dataset>" <Output Path>
```

Many converters exist to convert COCO to other formats.

To convert from SOLO to YOLO format, run:
```python
solo2yolo "Datasets/<SOLO Dataset>" <Output Path>
```

## Uploading to Roboflow
Upload the generated dataset to [Roboflow](https://roboflow.com) to easily train a model or share the dataset with others.

```python
python Datasets/roboflow_upload.py <dataset_path>
```

## Full Workflow for DAI Cameras
1. Ensure that the game resolution is set to 416x416
2. Generate images
3. Convert to COCO
```bash
solo2coco solo coco
```
> [!NOTE]
> `solo2coco` creates the real `coco` dataset inside an extraneous `coco` directory. In the following steps, either specify `coco/coco` or remove the unnecessary parent folder.

4. Manually delete any bad images in `coco/images`
5. Run bbox filter to remove bad annotations 
```bash
python bbox_filter.py coco
```
6. Upload to roboflow
```bash
python roboflow_upload.py coco
```
7. Export dataset from Roboflow in the `YOLO v7 PyTorch` format
8. Train using [cv-training](https://github.com/DukeRobotics/cv-training)
9. Convert weights to a blob file using [tools.luxonis.com](https://tools.luxonis.com)
10. Upload weights to [robosub-ros](https://github.com/DukeRobotics/robosub-ros)