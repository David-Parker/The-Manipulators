using UnityEngine;
using System.Collections;
using Manipulators;


public class UseCamera : MonoBehaviour {

	PlayerInput playerInput;

	//Level designer values
	public float cameraPanSpeed = 300;
	public float cameraZoomSensitivity = 1500;
	public float cameraClosestZoom = 15;
	public float cameraFarthestZoom = 90;
	public float cameraBorder = 420;
	public float triggerScreenPanPixels = 20;
	public float edgePanEasing = 15;
	public float cameraPivotIncrementAngle = 1;
	public float edgePushBackTrigger=20;


	//inputs from PlayerInput
	float playerSetHeight = 15;
	float mouseGroundPivotDistance;
	Vector3 mouseIntersectP;


	//store values for altMove()
	Vector3 storeMousePosition;
	Vector3 storeCameraPosition;


	//	* * * * pan functions; call cameraMove()  * * * *
	// *  * **  * * * * * * * * * * * * * *  * **  * * * * *

	/**Movement along local X axis. Uses a float input for magnitute of movement, and its sign as direction.
	 */
	void cameraXPan (float rightIsPos){
		Camera.main.transform.Translate (Vector3.right * Time.deltaTime * rightIsPos * cameraPanSpeed);
	}

	void cameraYPan (float upIsPos, float cameraYrotation){
		Vector3 movementVector = new Vector3(0,Mathf.Cos(degreesToRadians(cameraYrotation)),Mathf.Sin(degreesToRadians(cameraYrotation) ) );
		Camera.main.transform.Translate (movementVector * Time.deltaTime * upIsPos * cameraPanSpeed);
	}


	/**Movement along local Z axis. Uses a float input for magnitute of movement, and its sign as direction. 
	 * Uses a second float as input for its current position to avoid changing it's X.rotation
	 */
	void cameraZPan (float forwardIsPos, float cameraXrotation){
		Vector3 movementvector = new Vector3(0,Mathf.Sin(degreesToRadians(cameraXrotation)),Mathf.Cos(degreesToRadians(cameraXrotation) ) );
		Camera.main.transform.Translate (movementvector * Time.deltaTime * forwardIsPos * cameraPanSpeed);

	}

	/**Modifies the camera's Y.position & sets playerSetHeight
	 */
	void cameraZoom (float inIsPos){
		float cameraYposStore = playerInput.CurrentCameraPosition.y;
		Camera.main.transform.Translate (Vector3.forward * inIsPos *  Time.deltaTime  *cameraZoomSensitivity);
		playerSetHeight += playerInput.CurrentCameraPosition.y - cameraYposStore;		
	}

	/**Rotates the camera (X,Z axes) around a pivot groundIntersectP, distance to pivot is the 2D distance to the pivot (not accounting for Y).
	 */
	void cameraPivotAround (float distanceToPivot, float PosIsRight, Vector3 groundIntersectP){
		//cameraRotate(groundIntersectP, PosIsRight / 20, cameraPivotIncrementAngle );
		Camera.main.transform.RotateAround( groundIntersectP,Vector3.up,cameraPivotIncrementAngle * PosIsRight / 20 * Time.deltaTime);
	}


	// * * * * utility * * * * * 
	public static float degreesToRadians (float degrees) {
		return degrees * Mathf.PI / 180;
	}

	void Start () {
		playerInput = gameObject.GetComponent< PlayerInput >();
	}

	void Update () {
		//resets camera's Z rotation
		if(playerInput.CurrentCameraRotation.z != 0){
			Camera.main.transform.Rotate(0,0, -playerInput.CurrentCameraRotation.z);
		}

		//Following 4 methods are making sure camera is not out of bounds. Doesn't use deltaTime for a quicker response
		if(playerInput.CurrentCameraPosition.x < -cameraBorder){
			Camera.main.transform.Translate(Vector3.right*2,Space.World);
		}

		if(playerInput.CurrentCameraPosition.x > cameraBorder){
			Camera.main.transform.Translate(Vector3.left*2, Space.World);
		}

		if(playerInput.CurrentCameraPosition.z < -cameraBorder){
			Camera.main.transform.Translate(Vector3.forward*2, Space.World);
		}

		if(playerInput.CurrentCameraPosition.z > cameraBorder){
			Camera.main.transform.Translate(Vector3.back*2, Space.World);
		}

		//Keeps the camera at the distancce the player has set it to by zooming in/out
		if(Mathf.Abs(playerInput.CameraDistanceToGround - playerSetHeight) > 5){
			float distanceToGround = playerInput.CameraDistanceToGround;
			if(distanceToGround  < playerSetHeight){ //move cam up 
				Camera.main.transform.Translate(Vector3.up*2);
			}

			else{//move cam down
				Camera.main.transform.Translate(Vector3.down*2);
			}
		}

		//stores a point of reference for camera pivot
		if(playerInput.AltKeyDown){
			storeCameraPosition = playerInput.CurrentCameraPosition;
			storeMousePosition = playerInput.CurrentMousePosition;
			mouseIntersectP = playerInput.findGroundCollisionP();
			mouseGroundPivotDistance = PlayerInput.findGroundistance(storeCameraPosition , mouseIntersectP );
		}

		//pivot camera
		if(playerInput.AltKey){
			if(playerInput.MouseOnGround){
				if(playerInput.MouseX < storeMousePosition.x){
					cameraPivotAround(mouseGroundPivotDistance,(storeMousePosition.x-playerInput.MouseX),mouseIntersectP);
				}
				if(playerInput.MouseX > storeMousePosition.x){
					cameraPivotAround(mouseGroundPivotDistance,(storeMousePosition.x-playerInput.MouseX),mouseIntersectP);
				}
			}

		}else{
			// Panning w/ Keys
			if(playerInput.CameraHorizontalKeys != 0 ){
				if(playerInput.CameraHorizontalKeys  > 0 ){
					cameraXPan(playerInput.CameraHorizontalKeys);
				}
				if(playerInput.CameraHorizontalKeys  < 0 ){
					cameraXPan(playerInput.CameraHorizontalKeys);
				}
			}
			if(playerInput.CameraVerticalKeys != 0){
				if(playerInput.CameraVerticalKeys  > 0 ){
					cameraZPan(playerInput.CameraVerticalKeys, playerInput.CamXrotation);
				}
				if(playerInput.CameraVerticalKeys  < 0 ){
					cameraZPan(playerInput.CameraVerticalKeys, playerInput.CamXrotation);
				}
			}

			//Panning w/ mouse at Screen's edge
			if (playerInput.MouseX < triggerScreenPanPixels ){ 
				cameraXPan(  -(triggerScreenPanPixels -playerInput.MouseX)/edgePanEasing  );
			}
			if (playerInput.MouseX > playerInput.CameraScreenWidth -triggerScreenPanPixels ){
				cameraXPan(  (triggerScreenPanPixels -playerInput.CameraScreenWidth + playerInput.MouseX)/edgePanEasing  );
			}
			if (playerInput.MouseY < triggerScreenPanPixels ){
				cameraZPan(  -(triggerScreenPanPixels -playerInput.MouseY)/edgePanEasing , playerInput.CamXrotation  );
			}
			if (playerInput.MouseY > playerInput.CameraScreenHeight -triggerScreenPanPixels ){
				cameraZPan(  (triggerScreenPanPixels -playerInput.CameraScreenHeight + playerInput.MouseY)/edgePanEasing , playerInput.CamXrotation );
			}

			//Zooming w/ mouse
			if(playerInput.MouseWheelZoom!=0){
				if(playerInput.MouseWheelZoom<0 && playerInput.CurrentCameraPosition.y < cameraFarthestZoom ){ //zoom out
					cameraZoom(playerInput.MouseWheelZoom);
				}else if(playerInput.MouseWheelZoom > 0 && playerInput.CurrentCameraPosition.y > cameraClosestZoom  && playerInput.CameraDistanceToGround > cameraClosestZoom ){ //zoom in
					cameraZoom(playerInput.MouseWheelZoom);
				}
			}
		}



	}
}

