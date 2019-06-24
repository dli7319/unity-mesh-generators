# Unity Mesh Generators
A collection of scripts to generate meshes in Unity  
The meshes created by these have the same uv orientation as the default Unity meshes.  
They've been tested to work on Unity 2019.1.0f2.  
Note that meshes generated with more than 65k vertices may cause issues when built for certain platforms.

# How to use these
* Clone or download this to somewhere in `Assets` folder.
* Restart your Unity Editor.
* Create an asset from your Unity Editor by selecting a wizard under `Assets/Create` in the toolbar.

# Generators Included
* Octahedron Sphere
  * Allows you to select the level to divide.
* UVSphere
  * Allows you to select the number of width and height segments.
* Layered Sphere
  * A multi-layered version of the Octahedron Sphere for simulating volumes.
  * Allows you to select the number of layers and the radius.

# Credits
* Octohedron Sphere is modified from a version written by Jasper Flick sourced from BinPress
  * https://www.binpress.com/creating-octahedron-sphere-unity/
* UVSphere is modified from a version listed on the Unity3D wiki
  * https://wiki.unity3d.com/index.php/ProceduralPrimitives
