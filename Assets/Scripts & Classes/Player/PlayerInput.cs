using UnityEngine;
using System.Collections;

namespace Manipulators
{
	public class PlayerInput : MonoBehaviour
	{


		// Debug mode
		public bool debugModeOn = false;

		//vars

		bool mouseOnGround;


		// * * * * get/set & Input access * * * *


		public float MouseX {
			get{
				return removeExtremeMouseValues(Input.mousePosition.x,false);
			}
		}
		public float MouseY {
			get{
				return removeExtremeMouseValues(Input.mousePosition.y,true);
			}
		}
		public float CameraDistanceToGround {
			get{
				return findYDistance_FromCam_ToCollidersBelow(CurrentCameraPosition);
			}
		}
		public float CameraScreenWidth {
			get{ 
				return Camera.main.pixelWidth;
			}
		}
		public float CameraScreenHeight {
			get{ 
				return Camera.main.pixelHeight;
			}
		}
		public float CameraHorizontalKeys {
			get{
				return Input.GetAxis ("Horizontal");
			}
		}
		public float CameraVerticalKeys {
			get{ 
				return Input.GetAxis ("Vertical");
			}
		}
		public float MouseWheelZoom {
			get{ 
				return Input.GetAxis ("Mouse ScrollWheel");
			}
		}

		public bool AltKey {
			get{ 
				return Input.GetButton ("TurnCamera");
			}
		}
		public bool AltKeyDown {
			get{ 
				return Input.GetButtonDown ("TurnCamera");
			}
		}
		public bool MouseOnGround {get { return mouseOnGround;}set { mouseOnGround = value;}}

		public bool LeftMouseClick { //clicked, not held
			get{
				return Input.GetButtonDown ("LMB");
			}
		} 
		
		public Vector3 CurrentMousePosition {
			get{ 
				return Input.mousePosition;
			}
		}
		public Vector3 CurrentCameraPosition {
			get{ 
				return Camera.main.transform.position;
			}
		}
		public Vector3 CurrentCameraRotation {
			get{ 
				return Camera.main.transform.eulerAngles;
			}
		}
		public float CamXrotation {
			get{
				return Camera.main.transform.eulerAngles.x;
			}
		}
		public float CamDistanceFromGround {
			get{
				return findYDistance_FromCam_ToCollidersBelow(CurrentCameraPosition);
			}
		}



		// * * * * Functionality * * * *

		float removeExtremeMouseValues (float val, bool vertical)
		{
			if (vertical) {
				if (val < 0) {
					return 0;
				} else if (val > CameraScreenHeight) {
					return CameraScreenHeight;
				} else {
					return val;
				}
			} else {
				if (val < 0) {
					return 0;
				} else if (val > CameraScreenWidth) {
					return CameraScreenWidth;
				} else {
					return val;
				}
			}
		}



		// * * * * * Layer masks Generators * * * * *


		public static int genLayerMask (int whichLayer, bool invert)
		{
			int layerMask;
			layerMask = 1 << whichLayer;
			if (invert) {
				layerMask = ~layerMask;
			}
			return layerMask;
		}

		public static int genLayerMask (int whichLayer, int whichLayer2, bool invert)
		{
			int layerMask;
			layerMask = (1 << whichLayer) | (1 << whichLayer2);
			if (invert) {
				layerMask = ~layerMask;
			}
			return layerMask;
		}

		public static int genLayerMask (int whichLayer, int whichLayer2, int whichLayer3, bool invert)
		{
			int layerMask;
			layerMask = (1 << whichLayer) | (1 << whichLayer2) | (1 << whichLayer3);
			if (invert) {
				layerMask = ~layerMask;
			}
			return layerMask;
		}

		public static int genLayerMask (int whichLayer, int whichLayer2, int whichLayer3, int whichLayer4, bool invert)
		{
			int layerMask;
			layerMask = (1 << whichLayer) | (1 << whichLayer2) | (1 << whichLayer3) | (1 << whichLayer4);
			if (invert) {
				layerMask = ~layerMask;
			}
			return layerMask;
		}

		public static int genLayerMask (int whichLayer, int whichLayer2, int whichLayer3, int whichLayer4, int whichLayer5, bool invert)
		{
			int layerMask;
			layerMask = (1 << whichLayer) | (1 << whichLayer2) | (1 << whichLayer3) | (1 << whichLayer4);
			if (invert) {
				layerMask = ~layerMask;
			}
			return layerMask;
		}


	
		/** Finds the difference between camPositionY and the closest collider's position.y
		 * Uses multiple colliders and returns the lowest vales.
		 * If no collider is found, it returns 15 (default max zoom boundary)
		 */
		float findYDistance_FromCam_ToCollidersBelow ( Vector3 camPosition){
			int layerMask = genLayerMask(8,false);
			Ray[] shootDown = new Ray[9];

			shootDown[0] = new Ray(camPosition,Vector3.down);
			int secondaryRaysDistance = 10; //adjust the distances of secondary rays through this variable.
			shootDown[1] = new Ray(camPosition+Vector3.forward*secondaryRaysDistance,Vector3.down);
			shootDown[2] = new Ray(camPosition+Vector3.right*secondaryRaysDistance,Vector3.down);
			shootDown[3] = new Ray(camPosition+Vector3.back*secondaryRaysDistance,Vector3.down);
			shootDown[4] = new Ray(camPosition+Vector3.left*secondaryRaysDistance,Vector3.down);
			shootDown[5] = new Ray(camPosition+Vector3.forward*secondaryRaysDistance+Vector3.right*secondaryRaysDistance,Vector3.down);
			shootDown[6] = new Ray(camPosition+Vector3.right*secondaryRaysDistance+Vector3.back*secondaryRaysDistance,Vector3.down);
			shootDown[7] = new Ray(camPosition+Vector3.back*secondaryRaysDistance+Vector3.left*secondaryRaysDistance,Vector3.down);
			shootDown[8] = new Ray(camPosition+Vector3.left*secondaryRaysDistance+Vector3.forward*secondaryRaysDistance,Vector3.down);

			RaycastHit foundcollider = new RaycastHit();
			float[] distances = new float[9];
			int i = 0;
			foreach(Ray shootRay in shootDown){
				
				if(Physics.Raycast(shootRay, out foundcollider, Mathf.Infinity, layerMask)){
					distances[i] = camPosition.y - foundcollider.point.y;
				}else{
					if(debugModeOn){
						Debug.Log("No collider found on Raycasting down");
					}
					distances[i] = 15;
				}
				i++;
			}
			float intestDistance = distances[0];
			for(i=1;i < 9;i++){
				if(distances[i] < intestDistance){
					intestDistance = distances[i];
				}
			}
			return intestDistance;
			
		}

		// * * * * Checking colliders from mouse input * * * * 

		/**Finds and returns the collider the mouse is pointing to.
		 */
		public Collider findColliderFromMousePointer (int layerMask){
			Ray screenPixelRay = Camera.main.ScreenPointToRay (CurrentMousePosition);
			RaycastHit foundCollider;
			if (Physics.Raycast (screenPixelRay, out foundCollider, Mathf.Infinity, layerMask)) {
				return foundCollider.collider;
			} else {
				if(debugModeOn){
					Debug.Log("Couldn't find a collider from Mouse position");
				}
				return null;
			}
		}
		/**Finds the point in the game world where the mouse is pointing to. Returns it as Vector3.
		 */
		public Vector3 findMouseCollisionPoint (int layerMask)
		{
			Ray screenPixelRay = Camera.main.ScreenPointToRay (CurrentMousePosition);
			RaycastHit collisionP;
			if (Physics.Raycast (screenPixelRay, out collisionP, Mathf.Infinity, layerMask)) {
				MouseOnGround = true;
				return collisionP.point;
			} else {
				MouseOnGround = false;
				if(debugModeOn){
					Debug.Log("Mouse collision point not found, might be out of bounds");
				}
				return Vector3.zero;
			}
		}

		public static float findGroundistance (Vector3 point1, Vector3 point2)
		{
			point1.y = 0;
			point2.y = 0;
			return Vector3.Distance (point1, point2);
		}
	
		/**calls findCollisionPoint() ; use MouseonGround in origin f() to check if true
		 */
		public Vector3 findGroundCollisionP ()
		{
			int groundLayerMask = genLayerMask (8, false);
			return findMouseCollisionPoint (groundLayerMask);
		}

	}
}
