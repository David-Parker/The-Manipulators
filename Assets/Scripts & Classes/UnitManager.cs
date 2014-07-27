using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitManager : MonoBehaviour 
{
	public List<GameObject> selectedUnits;

	void Start()
	{
		selectedUnits.Clear();
	}

	public void SelectSingleUnit(GameObject unit)
	{ 
		foreach(GameObject obj in selectedUnits)
		{
			obj.GetComponent<Selectable>().isSelected = false;
		}
		selectedUnits.Clear();

		selectedUnits.Add(unit);
	}
}
