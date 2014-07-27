using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	private int speedSmooth = 50;

	public Transform origin;
	public Camera cam;

	/* Pan Variables */
	public int minScrollArea = 10;
	public int speed = 5;

	/* Orbit Variables */
	public int scrollSpeed = 2;

	/* Zoom Variables */
	public float distance = 50;
	public float sensitivityDistance = 50;
	public float minFOV = 5;
	public float maxFOV = 100;

	void Start () {
		/* Initialize the camera to a known state */
		transform.eulerAngles = new Vector3(45,270,0);
		distance = camera.fieldOfView;
	}
	
	void Update () {
		/* Check for pan */
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
		if(Input.GetKey(KeyCode.LeftShift)) {
			int direction = (Input.mousePosition.x > Screen.width/2) ? -scrollSpeed : scrollSpeed;
			transform.RotateAround(origin.position, Vector3.up,direction);
		}
	
	}

}
