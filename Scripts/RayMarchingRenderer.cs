using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*----------------------------------------------------------------------------------------------------------------------------------------------
  Ray Marching Renderer
	
	Author -  Cole Barach
	Created - 2022.02.11
	Updated - 2022.04.10
	
	Function
		-Component for renderer recognized by RayMarchCamera
    Notes
        -Render Identities/Materials preprogrammed, work on implementing more generic solution
        -Limited Functionality, materials not implemented
----------------------------------------------------------------------------------------------------------------------------------------------*/

public class RayMarchingRenderer : MonoBehaviour {
    [Header("Properties")]
    public int   renderIdentity;
    public int   materialIdentity;
    [ColorUsage(true,true)]
    public Color albedo;
    [Header("Debug")]
    public bool drawGizmo;
    public bool drawAsWireframe;

    void OnDrawGizmos() {
        if(drawGizmo) {
            switch(renderIdentity) {
                case 1:
                    Gizmos.color = albedo;
                    Gizmos.matrix = transform.localToWorldMatrix;
                    if(drawAsWireframe) {
                        Gizmos.DrawWireSphere(Vector3.zero,0.5f);
                    } else {
                        Gizmos.DrawSphere(Vector3.zero,0.5f);
                    }
                    break;
                case 2:
                    Gizmos.color = albedo;
                    Gizmos.matrix = transform.localToWorldMatrix;
                    if(drawAsWireframe) {
                        Gizmos.DrawWireCube(Vector3.zero,Vector3.one);
                    } else {
                        Gizmos.DrawCube(Vector3.zero,Vector3.one);
                    }
                    break;
            }
        }
    }
}
