# RayMarching
Unity implementation of the ray marching render technique. Sample scene given in [Scenes/RayMarching.unity](/Scenes/RayMarching.unity)
## Usage
- At least one instance of RayMarchingCamera.cs is required to initialized rendering
- Default shader for rendering is RayMarchRenderer.compute
- Any object to be rendered exists as an instance of RayMarchingRenderer.cs
- Render and Material Identites exist as preprogrammed cases, currently only cube and sphere are supported.

## Expansion
- Any object with a known distance function can be rendered
- To add a new object, a new case and distance function can be added to the GetRendererDistance function of the RayMarchLibrary
- Transformations are already implemented through matrices, avoid implementing scale, position, and rotation for redundancy.

## To Do
- Implement rendering of generic meshes
- Implement usage of generic materials
- Correct scale requirement of camera (Z-Component)
