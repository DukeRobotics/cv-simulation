# CV Simulation

A Unity 3D underwater simulation to generate synthetic, labeled data for computer vision.

## Simulation Overview
The simulation was built using Unity 2022.2.9f1. It uses the HDRP render pipeline and [Unity Perception](https://docs.unity3d.com/Packages/com.unity.perception@1.0/manual/index.html). See [this tutorial](https://github.com/Unity-Technologies/com.unity.perception/blob/main/com.unity.perception/Documentation~/Tutorial/TUTORIAL.md) to get started with Unity Perception.

The simulation is setup to generate synthetic images of the gate and buoy with automatically generated bounding boxes for the RoboSub 2023 glyphs. The simulation occurs underwater, in a simulated swimming pool with murky water.

When the simulation is run, the camera is moved in a circle around the gate/buoy that is parallel to the pool floor. Every time the camera completes a circle, its radius is decreased. When the circle reaches the minimum specified radius, its height is increased and the process repeats. This ensures that the camera captures the gate and buoy from a variety of distances and angles.

In addition to the camera movements, the sun's position is randomized, and the gate's hue and the buoy's glyph orientations are randomized. This ensures that the camera captures the gate and buoy under a variety of lighting conditions, with a variety of colors for the gate, and with a variety of orientations for the buoy glyphs.

## Code Overview
The code is written in C# in the following files:
- [CameraController.cs](/Assets/CameraController.cs): Controls the camera movements.
- [RandomizedGateHue.cs](/Assets/RandomizedGateHue.cs): Randomizes the hue of the gate.
- [RotateGlyphs.cs](/Assets/RotateGlyphs.cs): Randomizes the orientation of the buoy glyphs.
- [Sun.cs](/Assets/Sun.cs): Randomizes the sun's position.

## Running the Simulation
Use the play button at the top of the Unity editor to run the simulation. The following option controls whether the simulation saves generated and bounding boxes to disk:
- Open the inspector for "Main Camera"
- Scroll down in the inspector to "Perception Camera (Script)"
- Check/uncheck "Save Camera RGB Output to Disk"

_Note: Generated datasets can occupy a lot of disk space._

Each time the simulation is run with the above option checked, a new folder is created inside the `Datasets` directory. The new folder contains the generated images and bounding boxes in [SOLO](https://docs.unity3d.com/Packages/com.unity.perception@1.0/manual/Schema/SoloSchema.html) format, created by Unity. To convert SOLO to COCO format, see [this article](https://docs.unity3d.com/Packages/com.unity.perception@1.0/manual/Tutorial/convert_to_coco.html). Many converters exist to convert COCO to other formats.

## Future Development
This project was last updated in April 2023. For future development, it is recommended to update the project to use the latest versions of Unity and Unity Perception.

