using UnityEngine;
using System.Collections;

public class Selectable : MonoBehaviour {

	public bool isSelected;

	private UnitManager unitManager;

	
	void Awake()
	{
		isSelected = false;
		unitManager = Camera.main.GetComponentInChildren<UnitManager>();
	}

	void Update()
	{
		if(isSelected)
		{

		}
	}

	void Selected()
	{
		// UnitManager select this object
		unitManager.SelectSingleUnit(gameObject);
		Debug.Log("Selected: " + gameObject);

		isSelected = true;
	}
}
