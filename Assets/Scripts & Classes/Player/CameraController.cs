﻿using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	const int UINITIALIZED = -99999999;

	private int speedSmooth = 50;

	public Transform origin;
	public Camera cam;
	public bool rotateWithoutPivot = true;

	/* Pan Variables */
	public int minScrollArea = 10;
	public int panSpeed = 10;
	public int panSpeedFast = 30;

	/* Orbit Variables */
	public int zoomSpeed = 2;
	private Vector3 pivotPoint;
	private bool newPivot;
	private float currPos;
	private float lastPos;

	/* Zoom Variables */
	private float distance = 50;
	public float sensitivityDistance = 50;
	public float minFOV = 5;
	public float maxFOV = 60;

	void Start () {
		/* Initialize the camera to a known state */
		transform.eulerAngles = new Vector3(45,180,0);
		distance = camera.fieldOfView;
		lastPos = UINITIALIZED;
		newPivot = true;
	}
	
	void Update () {
		/* Check for pan */
		int speed = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) ? panSpeedFast : panSpeed;
		if(Input.mousePosition.x < (Screen.width/minScrollArea)) {
			transform.position -= transform.TransformDirection(1,0,0)*speed/speedSmooth;
		}

		else if(Input.mousePosition.x > (Screen.width - (Screen.width/minScrollArea))) {
			transform.position += transform.TransformDirection(1,0,0)*speed/speedSmooth;
		}

		if(Input.mousePosition.y < (Screen.height/minScrollArea)) {
			transform.position -= transform.TransformDirection(0,1,1)*speed/speedSmooth;
		}

		else if(Input.mousePosition.y > (Screen.height - (Screen.height/minScrollArea))) {
			transform.position += transform.TransformDirection(0,1,1)*speed/speedSmooth;
		}

		/* Check for zoom */
		distance -= Input.GetAxis("Mouse ScrollWheel") * sensitivityDistance;
        distance = Mathf.Clamp(distance, minFOV, maxFOV);
        camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, distance,  Time.deltaTime * 10);

		/* Check for Orbit */
		if(Input.GetMouseButton(2)) {
			/* Only pivot around the point where the user initially held down the rotate button. */
			RaycastHit hit;
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));

            if(Physics.Raycast(ray, out hit)) {
          		currPos = Input.mousePosition.x;
          		if(lastPos == UINITIALIZED) lastPos = currPos;
            	if(hit.collider != null) {
            		if(newPivot) {
            			pivotPoint = hit.point;
            			newPivot = false;
            		}
            	}
            }
            else if(rotateWithoutPivot) {
            	currPos = Input.mousePosition.x;
            	if(lastPos == UINITIALIZED) lastPos = currPos;
            	pivotPoint = origin.position;
            }
            transform.RotateAround(pivotPoint, Vector3.up,(currPos - lastPos)*zoomSpeed/8);
            lastPos = currPos;
		}
		else {
			newPivot = true;
			lastPos = UINITIALIZED;
		} 
	}
}
