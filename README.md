# Orthographic to Perspective
## Description 
This project allows you transform a 3d model to look like it's in orthographic view from a perspective camera from a single point of view. 

## Installation 
prerequisites:
Unity version 6000.3.11f1


```bash
# clone the repo
git clone https://github.com/Rennik2/Ortho-to-Perspective.git
```
Go into the folder cloned and open Unity-Ortho_to_Perspective folder in Unity.

## How To Use
### ToOrthoV1 script
**Use to easily make a lot of appear in orthographic view** 

- place the script ToOrthoV1 on an empty object
- add objects to scene (most have a mesh render and mesh filter component and read right set to true in import settings)
- add all object that will be turned to orthographic view into toGameObjects array in unity inspector 
- set the main camera as the From Position 
- enter play mode. Use updateContinuously bool set to true to update the mesh as you change settings. On meshes over a few hundred vertices set updateContinuously to false and press Update Mesh to see changes made and not having so much lag.
##### From Position
From Position is the point that when viewed from it appears the meshes are in orthographic view. If this is different than the main camera will look distorted.
##### Is Orthographic 
Is Orthographic allows easy switching between seeing the orthographic view or the perspective view. When true shows orthographic view, false to see perspective view.
##### **Through Camera Plane** 
Through Camera Plane controls how big the orthographic camera is. Larger values will show more of the scene / make the object appear smaller when view from the From Position.  

### ToOrthoV2 script
**Use to make objects appear in orthographic view while giving per object control.**

- add objects to scene (most have a mesh render and mesh filter component and read right set to true in import settings)
- add the ToOrthoV2 script on object that will be made orthographic
- set the main camera as the From Position 
- enter play mode. 
##### From Position
From Position is the point that when viewed from it appears the meshes are in orthographic view. If this is different than the main camera will look distorted.
##### **Through Camera Plane** 
Through Camera controls how big the orthographic camera is. Larger values will show more of the scene / make the object appear smaller when view from the from Position.
##### **Obj Scale** 
Obj Scale controls the scale of the object in the world while maintaining how large it appears from the view point.

### SlideVert script 
**Use to slide individual verts closer or farther from the camera to create illusions / weird shapes.**

- add SlideVert script to game object 
- set the main camera as the From Position 
- enter play mode
- play with the Unique Vert Multiplier to move the vertices closer or further from the From Position while not the location when viewed from From Position
##### From Position
From Position is the point that when viewed from it appears the meshes are in orthographic view. If this is different than the main camera will look distorted.
##### Full Object Scale 
Full Object Scale controls the scale of the object in the world while maintaining how large it appears from the view point.

##### **Unique Vert Multiplier** 
Unique Vert Multiplier controls the distance between the From Position while maintaining its location when viewed from the From Position.
#### updating
Use updateContinuously bool set to true to update the mesh as you change settings. On meshes over a few hundred vertices set updateContinuously to false and press Update Mesh to see changes made and not having so much lag.

### SaveObject script
**Use to save the distorted meshes in play mode to your computer as obj files.** 

- add to the object you wish to save or add all the object you wish to save to the GameObject array
- enter play mode
- distort the mesh
- press on of the save buttons
##### Path
The path where the meshes will be saved on your computer. (only tested on windows)
##### File Name
The name of the saved files. Will export to different files if more then one object is being saved.
##### Game Objects
GameObjects is an array of all object that will be saved. 
##### **Save Objects** 
Save Objects saves all the objects in GameObjects
#### **Save This Object**
Save This Object only saves the game object that this script is attached to. 
