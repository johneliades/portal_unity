using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Teleport : MonoBehaviour
{
	// Start is called before the first frame update
	public Transform pairCollider;
	private GameObject player;
	private CharacterController cc;
	private bool playerIsOverlapping = false;
	
	void Start()
	{
		cc = GameObject.FindObjectOfType<CharacterController>();
		player = cc.gameObject;
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject == player) {
			playerIsOverlapping = true;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.gameObject == player) {
			playerIsOverlapping = false;
		}
	}

	// Update is called once per frame
	void LateUpdate()
	{
		//Debug.Log(transform.up+" "+player.transform.position+" "+Vector3.Dot(transform.up, portalToPlayer)+this.name);
		
		if(playerIsOverlapping) {
			Vector3 portalToPlayer = player.transform.position - transform.position;
			float dotProduct = Vector3.Dot(transform.forward, portalToPlayer);

			// If this is true: The player has moved across the portal
			if (dotProduct < 0f)
			{
				cc.enabled = false;

				float yRot = transform.eulerAngles.y;
				float yPairRot = pairCollider.eulerAngles.y;

				// Teleport him!
				float rotationDiff = 180 - (yRot - yPairRot);
				player.transform.Rotate(Vector3.up, rotationDiff);

				Vector3 positionOffset = Quaternion.Euler(0f,
					rotationDiff, 0f) * portalToPlayer;
				
				player.transform.position = pairCollider.position + positionOffset;
				playerIsOverlapping = false;
				player.GetComponent<FirstPersonController>().MouseReset();
				cc.enabled = true;
			}
		}
	}
}
