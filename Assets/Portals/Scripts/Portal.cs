using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEditor.Rendering.Universal;
using RenderPipeline = UnityEngine.Rendering.RenderPipelineManager;

[ExecuteInEditMode]
public class Portal : MonoBehaviour
{
    public Camera portalCamera;
    public Transform pairPortal;
    
    private void OnEnable()
    {
        RenderPipeline.beginCameraRendering += UpdateCamera;
    }

    private void OnDisable()
    {
        RenderPipeline.beginCameraRendering -= UpdateCamera;
    }

    void UpdateCamera(ScriptableRenderContext empty, Camera playerCamera)
    {
        if ((playerCamera.cameraType == CameraType.Game || playerCamera.cameraType == CameraType.SceneView) &&
            playerCamera.tag != "Portal Camera")
        {
            portalCamera.projectionMatrix = playerCamera.projectionMatrix; // Match matrices

            var relativePosition = transform.InverseTransformPoint(playerCamera.transform.position);
            relativePosition = Vector3.Scale(relativePosition, new Vector3(-1, 1, -1));
            portalCamera.transform.position = pairPortal.TransformPoint(relativePosition);

            var relativeRotation = transform.InverseTransformDirection(playerCamera.transform.forward);
            relativeRotation = Vector3.Scale(relativeRotation, new Vector3(-1, 1, -1));
            portalCamera.transform.forward = pairPortal.TransformDirection(relativeRotation);
        }
    }
}
