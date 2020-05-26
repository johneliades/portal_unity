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
	public GameObject pairPortal;
	
	private GameObject player;
	private CharacterController cc;
	private bool active = true;
	private bool playerIsOverlapping = false;

	void Start()
	{
		cc = GameObject.FindObjectOfType<CharacterController>();
		player = cc.gameObject;
	}

	private void OnEnable()
	{
		RenderPipeline.beginCameraRendering += UpdateCamera;
	}

	private void OnDisable()
	{
		RenderPipeline.beginCameraRendering -= UpdateCamera;
	}


	void UpdateCamera(ScriptableRenderContext empty, Camera camera)
	{
		if ((camera.cameraType == CameraType.Game || camera.cameraType == CameraType.SceneView) &&
			camera.tag != "Portal Camera")
		{
			portalCamera.projectionMatrix = camera.projectionMatrix; // Match matrices

			var relativePosition = transform.InverseTransformPoint(camera.transform.position);
			relativePosition = Vector3.Scale(relativePosition, new Vector3(-1, 1, -1));
			portalCamera.transform.position = pairPortal.transform.TransformPoint(relativePosition);

			var relativeRotation = transform.InverseTransformDirection(camera.transform.forward);
			relativeRotation = Vector3.Scale(relativeRotation, new Vector3(-1, 1, -1));
			portalCamera.transform.forward = pairPortal.transform.TransformDirection(relativeRotation);
		}
	}
}
