using UnityEngine;
using System.Collections;

public class KeyboardInput : MonoBehaviour {

	public bool showCursor = true; // global variable to disable cursor
	public bool keyInterruptAllowed = true; // global variable to disable keys

	void Start (){

		Screen.showCursor = showCursor;
	}

	void Update () {

		if(keyInterruptAllowed)
		{
		if(Input.GetKeyDown(KeyCode.X)){
		   if(Screen.showCursor){
				Debug.Log("Cursor Off");
		   		Screen.showCursor = false;
			}
		   else if(showCursor){
				Debug.Log("Cursor On");
				Screen.showCursor = true;
			}
		}
		}
	}
}
