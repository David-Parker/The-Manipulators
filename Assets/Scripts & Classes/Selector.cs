using UnityEngine;
using System.Collections;

public class Selector : MonoBehaviour {



	void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray, out hit))
			{			
				hit.collider.gameObject.SendMessage("Selected", SendMessageOptions.DontRequireReceiver);
			}
		}
	}	
}
