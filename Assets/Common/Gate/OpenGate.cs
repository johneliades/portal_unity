using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGate : MonoBehaviour
{
	public GameObject gate;
	private GameObject sketchup, current;

	private void OnTriggerEnter(Collider collider) {
		gate.GetComponent<Collider>().enabled = false;
		sketchup = gate.transform.GetChild(0).gameObject;

		for(int i=1; i<sketchup.transform.childCount; i++) {
			current = sketchup.transform.GetChild(i).gameObject;
			Renderer renderer= current.GetComponent<Renderer>();
			if(renderer!=null)
				renderer.enabled = false;
		}
	}

	private void OnTriggerExit(Collider collider) {
		gate.GetComponent<Collider>().enabled = true;
		sketchup = gate.transform.GetChild(0).gameObject;

		for(int i=1; i<sketchup.transform.childCount; i++) {
			current = sketchup.transform.GetChild(i).gameObject;
			Renderer renderer= current.GetComponent<Renderer>();
			if(renderer!=null)
				renderer.enabled = true;
		}
	}
}
