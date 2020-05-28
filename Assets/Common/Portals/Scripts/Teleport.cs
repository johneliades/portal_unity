using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Teleport : MonoBehaviour
{
	// Start is called before the first frame update
	public Transform pairCollider;
	private GameObject player;
	private GameObject toMove;
	private CharacterController cc;
	private bool overlapping = false;
	

	void Start()
	{
		cc = GameObject.FindObjectOfType<CharacterController>();
		player = cc.gameObject;
	}

	void OnTriggerStay(Collider other) {
		if(other.gameObject != player) {
			Pickup pickup = cc.gameObject.GetComponent<Pickup>();
			if(pickup!=null && pickup.getCarriedObject() == other.gameObject)
				return;
			overlapping = true;
			toMove = other.gameObject;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject == player) {
			overlapping = true;
			toMove = player;
		}

		Teleportable otherObject = other.gameObject.GetComponent<Teleportable>();
		if(otherObject!=null) {
			Pickup pickup = cc.gameObject.GetComponent<Pickup>();
			if(pickup!=null && pickup.getCarriedObject() == other.gameObject)
				return;

			overlapping = true;
			toMove = other.gameObject;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.gameObject == player) {
			overlapping = false;
		}
	}

	// Update is called once per frame
	void LateUpdate()
	{
		//Debug.Log(transform.up+" "+player.transform.position+" "+Vector3.Dot(transform.up, portalToPlayer)+this.name);
		
		if(overlapping) {
			Vector3 portalToPlayer = toMove.transform.position - transform.position;
			float dotProduct = Vector3.Dot(transform.forward, portalToPlayer);

			// If this is true: The player has moved across the portal
			if (dotProduct < 0f)
			{
				if(toMove == player)
					cc.enabled = false;

				float yRot = transform.eulerAngles.y;
				float yPairRot = pairCollider.eulerAngles.y;

				// Teleport him!
				float rotationDiff = 180 - (yRot - yPairRot);
				toMove.transform.Rotate(Vector3.up, rotationDiff);

				Vector3 positionOffset = Quaternion.Euler(0f,
					rotationDiff, 0f) * portalToPlayer;
				
				toMove.transform.position = pairCollider.position + positionOffset;

				if(toMove == player) {
					toMove.GetComponent<FirstPersonController>().MouseReset();
					cc.enabled = true;
				}
				
				overlapping = false;
			}
		}
	}
}
