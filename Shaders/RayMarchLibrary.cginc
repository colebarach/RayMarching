/*----------------------------------------------------------------------------------------------------------------------------------------------
  Ray Marching Library
	
	Author -  Cole Barach
	Created - 2022.02.11
	Updated - 2022.04.10
	
	Function
		-Shader include for RayMarchRenderer
        -Generic Distance Functions for renderers
    Notes
        -All renderers are preprogrammed, meshes not supported yet
----------------------------------------------------------------------------------------------------------------------------------------------*/

float3 GetTransformPosition(float4x4 transform) {
    return mul(transform, float4(0,0,0,1)).xyz;
}

struct Renderer {
    float4x4 transform;
    int      renderIdentity;
    int      materialIdentity;
    float4   albedo;
};

float UnitSphereDistance(float3 position) {
    return length(position)-0.5f;
}
float UnitCubeDistance(float3 position) {
    float3 normalDistance = abs(position) - float3(0.5,0.5,0.5);
    float maxMagnitude = length(max(0,normalDistance));
    float negative = max(max(min(0,normalDistance.x), min(0,normalDistance.y)), min(0,normalDistance.z));
    return maxMagnitude+negative;
}

float GetRendererDistance(float3 position, Renderer renderer) {
    float3 localPosition = mul(renderer.transform,float4(position.xyz,1)).xyz;
    float distance = 0;
    switch(renderer.renderIdentity) {
        case 1:
            distance = UnitSphereDistance(localPosition);
            break;
        case 2:
            distance = UnitCubeDistance(localPosition);
            break;
    }
    return distance;
}
float4 GetRendererMaterial(Renderer renderer) {
    float4 color = renderer.albedo;
    switch(renderer.materialIdentity) {
        case 1:
            color = renderer.albedo;
            break;
    }
    return color;
}