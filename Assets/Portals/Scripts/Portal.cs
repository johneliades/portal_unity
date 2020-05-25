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
	private GameObject player;
	private CharacterController cc;
	private Vector3 portalNormal;
	private bool overlapping = false;

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

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject == player) {
			overlapping = true;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.gameObject == player) {
			overlapping = false;
		}
	}

	void Update()
	{
		if (overlapping) {
			Vector3 portalToPlayer = cc.transform.position - transform.position;
			float dotProduct = Vector3.Dot(transform.up, portalToPlayer);

			// If this is true: The player has moved across the portal
			if (dotProduct < 0f)
			{
				// Teleport him!
				float rotationDiff = -Quaternion.Angle(transform.rotation,
					pairPortal.rotation);
				
				rotationDiff += 180;
				player.transform.Rotate(Vector3.up, rotationDiff);

				Vector3 positionOffset = Quaternion.Euler(0f,
					rotationDiff, 0f) * portalToPlayer;
				
				player.transform.position = pairPortal.position + positionOffset;

				overlapping = false;
			}
		}
	}

	void UpdateCamera(ScriptableRenderContext empty, Camera camera)
	{
		if ((camera.cameraType == CameraType.Game || camera.cameraType == CameraType.SceneView) &&
			camera.tag != "Portal Camera")
		{
			portalCamera.projectionMatrix = camera.projectionMatrix; // Match matrices

			var relativePosition = transform.InverseTransformPoint(camera.transform.position);
			relativePosition = Vector3.Scale(relativePosition, new Vector3(-1, 1, -1));
			portalCamera.transform.position = pairPortal.TransformPoint(relativePosition);

			var relativeRotation = transform.InverseTransformDirection(camera.transform.forward);
			relativeRotation = Vector3.Scale(relativeRotation, new Vector3(-1, 1, -1));
			portalCamera.transform.forward = pairPortal.TransformDirection(relativeRotation);
		}
	}
}
