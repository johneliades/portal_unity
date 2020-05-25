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
		if(active && other.gameObject == player) {
			pairPortal.GetComponent<Portal>().active = false;
			active = false;

			Vector3 portalToPlayer = cc.transform.position - transform.position;
			float dotProduct = Vector3.Dot(transform.up, portalToPlayer);

			// If this is true: The player has moved across the portal
			if (dotProduct < 0f)
			{
				// Teleport him!
				float rotationDiff = -Quaternion.Angle(transform.rotation,
					pairPortal.transform.rotation);
				
				rotationDiff += 180;
				player.transform.Rotate(Vector3.up, rotationDiff);

				Vector3 positionOffset = Quaternion.Euler(0f,
					rotationDiff, 0f) * portalToPlayer;
				
				player.transform.position = pairPortal.transform.
					Find("Spawn").position + positionOffset;
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.gameObject == player) {
			active = true;
		}
	}

	void Update()
	{

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
