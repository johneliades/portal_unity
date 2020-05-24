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
	float disableTimer = 0;

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

	void OnTriggerEnter(Collider collider)
	{
		Debug.Log("WTF");
		if (disableTimer <= 0 && collider.gameObject == player) {
			disableTimer = 1;

			/* change the direction of the player's velocity to be the same as
			the portalNormal direction */
			Vector3 exitVelocity = portalNormal * cc.velocity.magnitude;
			cc.Move(exitVelocity);
		//	cc.velocity = exitVelocity;

			//set the player position to just in front of the portal.
			Vector3 exitPosition = pairPortal.transform.position;
			player.transform.position = exitPosition;

			/* disable player movement while in the air. This is turned back on when
				the player hits the ground. */
//			cm.isInputEnabled = false;
			//play the teleport sound

	//		audioSource.clip = teleportSound;
	//		audioSource.Play();
		}
	}

	void Update()
	{
		if (disableTimer > 0)
			disableTimer -= Time.deltaTime;
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
