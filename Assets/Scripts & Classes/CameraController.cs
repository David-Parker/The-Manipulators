using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	const int UINITIALIZED = -99999999;

	private int speedSmooth = 50;

	public Transform origin;
	public Camera cam;
	public bool rotateWithoutPivot;

	/* Pan Variables */
	public int minScrollArea = 10;
	public int speed = 5;

	/* Orbit Variables */
	public int scrollSpeed = 2;
	private Vector3 pivotPoint;
	private bool newPivot;
	private float currPos;
	private float lastPos;

	/* Zoom Variables */
	public float distance = 50;
	public float sensitivityDistance = 50;
	public float minFOV = 5;
	public float maxFOV = 60;

	void Start () {
		/* Initialize the camera to a known state */
		transform.eulerAngles = new Vector3(45,270,0);
		distance = camera.fieldOfView;
		lastPos = UINITIALIZED;
		newPivot = true;
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
		if(Input.GetMouseButton(2)) {
			/* Only pivot around the point where the user initially pressed shift */
			int direction = (Input.mousePosition.x > Screen.width/2) ? scrollSpeed : -scrollSpeed;
			RaycastHit hit;
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));

            if(Physics.Raycast(ray, out hit)) {
          		currPos = Input.mousePosition.x;
          		if(lastPos == UINITIALIZED) lastPos = currPos;
            	if(hit.collider != null) {
            		if(newPivot) {
            			pivotPoint = hit.point;
            			newPivot = false;
            			Debug.Log(pivotPoint);
            		}
            		transform.RotateAround(pivotPoint, Vector3.up,(currPos - lastPos)*scrollSpeed/8);
            	}
            	lastPos = currPos;
            }
            else if(rotateWithoutPivot) {
            	transform.RotateAround(origin.position, Vector3.up,direction);
            }
		}
		else {
			newPivot = true;
			lastPos = UINITIALIZED;
		} 
	}
}
