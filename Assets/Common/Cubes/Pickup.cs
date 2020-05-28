using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
	private GameObject mainCamera;
	private bool carrying;
	private GameObject carriedObject;
	private float distance;
	private float smooth;

	// Use this for initialization
	void Start () {
		mainCamera = GameObject.FindWithTag("MainCamera");
		distance = 2;
		smooth = 20;
	}
	
	public GameObject getCarriedObject() {
		return carriedObject;
	}

	// Update is called once per frame
	void Update () {
		if(carrying) {
			carriedObject.transform.position = Vector3.Lerp(
				carriedObject.transform.position, mainCamera.transform.position
				+ mainCamera.transform.forward * distance, Time.deltaTime * smooth);

			if(Input.GetKeyUp("e")) {
				carrying = false;
				carriedObject.gameObject.GetComponent<Rigidbody>().useGravity = true;
	//			carriedObject.gameObject.GetComponent<Rigidbody>().isKinematic = false;
				carriedObject = null;
			}
		} 
		else {
			if(Input.GetKeyUp("e")) {
				int x = Screen.width / 2;
				int y = Screen.height / 2;
				
				Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(
					new Vector3(x,y));
				RaycastHit hit;
				if(Physics.Raycast(ray, out hit)) {
					Pickable p = hit.collider.GetComponent<Pickable>();
					if(p != null) {
						carrying = true;
						carriedObject = p.gameObject;
     					carriedObject.gameObject.GetComponent<Rigidbody>()
     						.useGravity = false;
	//					p.gameObject.GetComponent<Rigidbody>().isKinematic = true;
					}
				}
			}
		}
	}
}
