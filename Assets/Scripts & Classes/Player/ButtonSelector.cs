using UnityEngine;
using System.Collections;

public class ButtonSelector : MonoBehaviour {

	private Camera cam;

	void Start () {
		cam = Camera.main;
	}
	
	void Update () {
		if(Input.GetMouseButtonDown(0)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			 if(Physics.Raycast(ray, out hit)) {
			 	Button button = hit.collider.gameObject.GetComponent<Button>();
			 	if(hit.collider != null && button != null) {
			 		button.buttonAction();
			 	}
			 }
		}
	}
}
