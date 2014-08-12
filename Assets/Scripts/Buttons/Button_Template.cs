using UnityEngine;
using System.Collections;
[RequireComponent (typeof(Button))]

public class Button_Template : MonoBehaviour {
	/* Scene Modifiable Variables Here */
	public int dummy;

	void buttonAction() {
		/* Add Button Functionality here. For example   */
		/* loading a new level or scene, opening a menu */
		/* dialog or performing an in-game action.      */
		Debug.Log("Button Pressed!");
	}
}
