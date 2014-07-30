using UnityEngine;
using System.Collections;

public class SelectableUnit : MonoBehaviour {

	public bool isSelected {get; private set;}

	void Awake()
	{
		Deselected();
	}

	public virtual void Selected()
	{
		isSelected = true;
		foreach(Transform child in transform)
		{
			child.renderer.material.color = Color.red;
		}

	}

	public virtual void Deselected()
	{
		isSelected = false;
		foreach(Transform child in transform)
		{
			child.renderer.material.color = Color.white;
		}
	}
}
