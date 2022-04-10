using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*----------------------------------------------------------------------------------------------------------------------------------------------
  Ray Marching Camera
	
	Author -  Cole Barach
	Created - 2022.02.11
	Updated - 2022.04.10
	
	Function
		-Rendering of Ray Marching Objects
		-Dispatching of renderShader
    Dependencies
        -RayMarchRenderer Shader
    Notes
        -Z-Component of transform.localScale should be -1, due to render methods
----------------------------------------------------------------------------------------------------------------------------------------------*/

public class RayMarchingCamera : MonoBehaviour {
    [Header("Shader")]
    public ComputeShader renderShader;
    public Vector3Int    renderThreads = new Vector3Int(16,16,1);
    [Header("Parameters")]
    public int   rayDepth              = 64;
    public float iso                   = 0.01f;
    [ColorUsage(true,true)]
    public Color skybox                = Color.black;
    public int   maxDistance           = 1024;

    RenderTexture         render;
    int                   renderKernel;
    RayMarchingRenderer[] renderStack;
    Renderer[]            renderStructs;
    ComputeBuffer         renderBuffer;

    Camera                cameraComponent;
    Matrix4x4             cameraTransform;
    Matrix4x4             cameraInverseProjection;

    struct Renderer {
        public Matrix4x4 transform;
        public int       renderIdentity;
        public int       materialIdentity;
        public Color     albedo;

        static public int GetStride() {
            return sizeof(float)*4*4  + 2*sizeof(int) + sizeof(float)*4;
        }
    };

    void Start() {
        cameraComponent = GetComponent<Camera>();
        renderKernel = renderShader.FindKernel("Render");
        render = new RenderTexture(Screen.width,Screen.height,24);
        render.enableRandomWrite = true;
    }
    void OnRenderImage(RenderTexture source, RenderTexture destination) {
        InitializeRenderTexture(render,source);
        InitializeStack();
        InitializeCamera();

        renderShader.SetTexture(renderKernel, "render",            render);
        renderShader.SetBuffer(renderKernel,  "stack",             renderBuffer);
        renderShader.SetInt(                  "stackCount",        renderStack.Length);
        renderShader.SetMatrix(               "transform",         cameraTransform);
        renderShader.SetMatrix(               "inverseProjection", cameraInverseProjection);
        renderShader.SetInt(                  "depth",             rayDepth);
        renderShader.SetFloat(                "iso",               iso);
        renderShader.SetVector(               "skybox",            skybox);
        renderShader.SetInt(                  "maxDistance",       maxDistance);

        renderShader.Dispatch(renderKernel,render.width/renderThreads.x,render.height/renderThreads.y,renderThreads.z);
        
        Graphics.Blit(render,destination);
    }
    void OnApplicationQuit() {
        if(render != null) render.Release();
        if(renderBuffer != null) renderBuffer.Release();
    }

    void InitializeRenderTexture(RenderTexture destination, RenderTexture source) {
        if(destination.width != source.width || destination.height != source.height) {
            destination.Release();
            destination = new RenderTexture(source.width,source.height,source.depth);
            destination.enableRandomWrite = true;
        }
    }
    void InitializeStack() {
        renderStack = GameObject.FindObjectsOfType<RayMarchingRenderer>();
        if(renderBuffer == null || renderStack.Length != renderBuffer.count) {
            if(renderBuffer != null) renderBuffer.Release();
            renderBuffer = new ComputeBuffer(renderStack.Length, Renderer.GetStride());
            renderStructs = new Renderer[renderStack.Length];
        }
        for(int x = 0; x < renderStructs.Length; x++) {
            renderStructs[x].transform        = renderStack[x].transform.worldToLocalMatrix;
            renderStructs[x].renderIdentity   = renderStack[x].renderIdentity;
            renderStructs[x].materialIdentity = renderStack[x].materialIdentity;
            renderStructs[x].albedo           = renderStack[x].albedo;
        }
        renderBuffer.SetData(renderStructs);
    }
    void InitializeCamera() {
        cameraTransform = transform.localToWorldMatrix;
        cameraInverseProjection = Matrix4x4.Inverse(cameraComponent.projectionMatrix);
    }
}