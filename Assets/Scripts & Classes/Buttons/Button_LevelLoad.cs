using UnityEngine;
using System.Collections;
[RequireComponent (typeof(Button))]

public class Button_LevelLoad : MonoBehaviour {

	/* Set to -1 if you want to load a level by name */
	public int loadByNumber = -1;
	public string loadByName = "";

	void buttonAction() {
		if(loadByNumber >= 0)
			Application.LoadLevel(loadByNumber);
		else 
			Application.LoadLevel(loadByName);
	}
}
