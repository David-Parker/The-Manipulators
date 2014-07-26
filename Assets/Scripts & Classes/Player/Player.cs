using UnityEngine;
using Manipulators;


/*	TODO
 * 	1.select() & layer masks & select modes (info modes etc)
 * 	2.UI & Right click menu
 * 	3.Randomisation constructors for Item.cs, Person.cs, etc.
 * 	4.Human.cs to Puppet.cs transition
 * 	5.Replace specific references to Camera.main and assign a camera object to Player & PlayerInput instead
 * 
 * 
 * 	Test functionailty:
 * 	*use a trigger box to start events
 * 	*unity material.color & onMouseOver to test object selection through changing colour and opacity
 * 	*reassinging a class
 * 
 * 	FIXME 
 * 		 
 * 
 * 
 */



public class Player : MonoBehaviour {

	PlayerInput playerInput;
	Town theTown;

	bool displayObjectInfo = false;
	bool enableGameClock = true;
	bool enableInfoMode = true;
	bool mouseOnMenus = false;
	bool enableOverlayButtons = true;

	/* player settings *
	int informationLevel;
	int difficultyLevel;
	*/

	//Player specific in-game objects
	Person queen; //should be of type 

	//storing Object variables for display
	string currentItemName;
	string currentItemDesc;

	// storing selections
	bool aGameObjectSelected;
	GameObject selectedGO; //GO = GameObject
	GameObject HoveredGO;

	// * * * * * * GUI * * * * * * *

	// * * * *  Vars * * * * 
	Rect clockGUI = new Rect(30,30,50,25);

	/**outputs currentItemName, currentItemDesc and enables/disables the display of these by changing displayObjectInfo (bool)
	 */
	void displayMouseOverInfo(Info anInfoObject){
		if (anInfoObject != null){ 
			//check the type of object and display the appropriate information.
			if (anInfoObject.GetType() == typeof(Item)){ //FIXME might want to use is instead of typeof
				currentItemDesc = anInfoObject.ItemDescription;
				currentItemName = anInfoObject.ItemName;
				displayObjectInfo = true;
			} 
		}else{
			displayObjectInfo = false;
		}
	}


	// * * * * * * * Selection * * * * * * * //FIXME

	/*
	GameObject selectGameObject(){

	}
	*/


 


	// * * * * * * lookup & get components in objects section  * * * * * * * *

	/**Raycasts from the mouse position, and returns the Info component attached to a found collider.
	*/
	Info getInfoFromMouseRayCast(int generatedLayerMask, int numParentsToSearch){



		Collider aCollider = playerInput.findColliderFromMousePointer(generatedLayerMask); 
		if (aCollider != null){ // raycast returned a collider
			Component foundComponent = ManipulatorsUtils.findAComponent(aCollider.gameObject,numParentsToSearch,"Info");
			Info returnInfo = foundComponent.GetComponent<Info>();
			return returnInfo;
		}else{ //raycast hasn't returned a collider
			return null;
		}

	}




	void Awake () {
		playerInput = gameObject.GetComponent< PlayerInput >();
	}

	void Start() {

	}

	void Update () {

		if(enableInfoMode){
			int genLayerMask = PlayerInput.genLayerMask(12,false);
			Info foundInfo = getInfoFromMouseRayCast(genLayerMask,2);
			displayMouseOverInfo(foundInfo);
		}


		if(playerInput.LeftMouseClick==true){
			Debug.Log(Time.realtimeSinceStartup);

		}
	}

	void displayClockWindow(int windowID){
		//GUI.Button(new Rect (10,20,100,20), "Can't drag me");
		GUI.Box(new Rect(0,0,50,25),Town.ClockTime24h);
		GUI.DragWindow();
	}

	void displayOverlayControlsWindow(){

	}



	void OnGUI () { 
		if(enableOverlayButtons){
			if(enableInfoMode){ //FIXME: Switch Overlay buttons from area to window
				//Disable info mode button
				GUILayout.BeginArea(new Rect(playerInput.CameraScreenWidth-130,playerInput.CameraScreenHeight-50,125,25));
				if(GUILayout.Button("Disable info mode")){
					enableInfoMode = false;
				}
				GUILayout.EndArea();
			}else{
				GUILayout.BeginArea(new Rect(playerInput.CameraScreenWidth-130,playerInput.CameraScreenHeight-50,125,25));
				if(GUILayout.Button("Enable info mode")){
					enableInfoMode = true;
				}
				GUILayout.EndArea();
			}
		}
		if(displayObjectInfo && enableInfoMode){
			// Info displayed near mouse
			string displayString = currentItemName + "\n"+currentItemDesc;
			GUILayout.BeginArea(new Rect(playerInput.MouseX,playerInput.CameraScreenHeight-playerInput.MouseY,200,200));
			GUILayout.Box(displayString);
			GUILayout.EndArea();
		}
		if(enableGameClock){
			clockGUI = GUI.Window(0,clockGUI,displayClockWindow," ");
		}
	}
}
