using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tp : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject pairPortal;
    public Transform colliderplane;
	
	private GameObject player;
	private CharacterController cc;
	private bool active = true;
	private bool playerIsOverlapping = false;
    void Start()
	{
		cc = GameObject.FindObjectOfType<CharacterController>();
		player = cc.gameObject;
	}

    void OnTriggerEnter(Collider other)
	{
        Debug.Log("enter:"+this.name);
		if(other.gameObject == player) {
			playerIsOverlapping = true;
		}
	}

	void OnTriggerExit(Collider other)
	{
		Debug.Log("exit:"+this.name);
		if(other.gameObject == player) {
			playerIsOverlapping = false;
		}
	}

    // Update is called once per frame
void Update()
	{
		//Debug.Log(transform.up+" "+player.transform.position+" "+Vector3.Dot(transform.up, portalToPlayer)+this.name);
		
		if(playerIsOverlapping) {
			Debug.Log("enter:"+this.name);
			Vector3 portalToPlayer = player.transform.position - transform.position;
			float dotProduct = Vector3.Dot(transform.forward, portalToPlayer);
			//pairPortal.GetComponent<Portal>().active = false;
			//active = false;

			// If this is true: The player has moved across the portal
			if (dotProduct < 0f)
			{
				// Teleport him!
				float rotationDiff = -Quaternion.Angle(transform.rotation,
					colliderplane.rotation);
				cc.enabled = false;
				rotationDiff += 180;
                //rotationDiff = 0;
				player.transform.Rotate(Vector3.up, rotationDiff);

				Vector3 positionOffset = Quaternion.Euler(0f,
					rotationDiff, 0f) * portalToPlayer;
				
				player.transform.position = colliderplane.position + positionOffset;
				playerIsOverlapping = false;
				cc.enabled = true;
			}
		}

	}
}
