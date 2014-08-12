using UnityEngine;
using System.Collections;

public class ButtonSelector : MonoBehaviour {

	public Camera cam;

	void Start () {
		cam = Camera.main;
	}
	
	void Update () {
		if(Input.GetMouseButtonDown(0)) {
			RaycastHit hit;
			Ray ray = cam.ScreenPointToRay(Input.mousePosition);

			 if(Physics.Raycast(ray, out hit)) {
			 	GameObject go = hit.collider.gameObject;
			 	Button button = go.GetComponent<Button>();
			 	if(hit.collider != null && button != null) {
			 		go.SendMessage("buttonAction");
			 	}
			 }
		}
	}
}
