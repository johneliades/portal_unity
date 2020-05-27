﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class PortalGun : MonoBehaviour {
 
	public AudioClip portalSound;
	public AudioClip errorSound;
 	private AudioSource audioSrc;

	public GameObject orangePortal;
	public GameObject bluePortal;
 
	// Use this for initialization
	void Start () {
		audioSrc = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		// fire the right portal (left or right) based on input
		if (Input.GetButtonDown("Fire1")) {
			FirePortal("blue");
		} else if (Input.GetButtonDown("Fire2")) {
			FirePortal("orange");
		}
	}
 
	void FirePortal(string type) {
		// struct object that will hold our raycast information
		RaycastHit hit;
 
		// if we collide with an object with our raycast, spawn a portal there
		if (Physics.Raycast(Camera.main.transform.position,
			Camera.main.transform.forward, out hit, Mathf.Infinity)) {
			
			audioSrc.PlayOneShot(portalSound);
//			portalSound.Play();
			
			// choose between the correct portals based on string input
			GameObject portal = type == "orange" ? orangePortal : bluePortal;
 
			// set the portal to the same position as the raycast point, and set
			// its rotation to orient to the wall relative to what its "up" direction is,
			// which is Vector.up in world space 
			portal.transform.SetPositionAndRotation(
				hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
//			orangePortal.GetComponentInChildren<Camera>().Render();
//			bluePortal.GetComponentInChildren<Camera>().Render();
		} 
		else {
			audioSrc.PlayOneShot(errorSound);
//			errorSound.Play();
		}
	}
}