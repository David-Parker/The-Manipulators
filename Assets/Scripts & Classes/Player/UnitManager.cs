using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitManager : MonoBehaviour 
{
	public Texture SelectionBoxGraphic; 
	public Color SelectionBoxColor;

	// This variable helps determine whether the input is interpreted as a click or a click-to-drag 
	// Note: Relevant to ScreenPoint coordinates
	public Vector2 MinimumSelectionBoxSize;

	private Rect SelectionBoxRect;
	private List<GameObject> selectedUnits;
	private List<GameObject> selectableUnits;
	private Vector2 selectionBoxOrigin;
	private Vector2 selectionBoxSize;
	private bool ctrlHasBeenDown;
	private bool shiftHasBeenDown;
	private float debugMessageTimer;

	void Start()
	{
		selectedUnits = new List<GameObject>();
		selectedUnits.Clear();

		debugMessageTimer = 0;

		List<GameObject> selectableUnitsCheck = new List<GameObject>(GameObject.FindGameObjectsWithTag("SelectableUnit"));
		foreach(GameObject selectable in selectableUnitsCheck){
			if(selectable.GetComponent(typeof(SelectableUnit)) == null){
				Debug.LogError("Not all objects in the SelectableUnit Layer have a SelectableUnit() script attached");
				break;
			}
		}
		selectableUnitsCheck.Clear();
	}

	private void OnGUI()
	{
		// Only Draw once it is larger then the defined minimum size
		if(Mathf.Abs(selectionBoxSize.x) >= MinimumSelectionBoxSize.x && Mathf.Abs(selectionBoxSize.y) > MinimumSelectionBoxSize.y)
		{
			SelectionBoxRect = new Rect(selectionBoxOrigin.x, selectionBoxOrigin.y, selectionBoxSize.x, selectionBoxSize.y);
			GUI.color = SelectionBoxColor;
			GUI.DrawTexture(SelectionBoxRect, SelectionBoxGraphic);
		}
	}

	void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			// Determine the selection box origin
			// Note: Have to use inverted Y value since Input.mousePosition origin is bottom-left and OnGUI origin is upper-left
			float _invertedY = Screen.height - Input.mousePosition.y;
			selectionBoxOrigin = new Vector2(Input.mousePosition.x, _invertedY);


			ctrlHasBeenDown = false;
			shiftHasBeenDown = false;
			// Handle modifier keys
			if(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) || Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
				ctrlHasBeenDown = true;
			if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
				shiftHasBeenDown = true;
			if(ctrlHasBeenDown && shiftHasBeenDown)
			{
				ctrlHasBeenDown = false;
				shiftHasBeenDown = false;
			}
		}
		
		if(Input.GetMouseButton(0))
		{
			// Determine size of selection box based on origin and current mouse position
			// Note: Have to use inverted Y value since Input.mousePosition origin is bottom-left and OnGUI origin is upper-left
			float _invertedY = Screen.height - Input.mousePosition.y;
			selectionBoxSize = new Vector2(Input.mousePosition.x - selectionBoxOrigin.x, (selectionBoxOrigin.y - _invertedY) * -1);
			
			// Handle modifier status 
			// Note: Must remain held down from mouse-down to mouse-up to enable the control feature
			if(ctrlHasBeenDown)
			{
				if(!(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
					ctrlHasBeenDown = false;				
			}
			if(shiftHasBeenDown)
			{
				if(!(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
					shiftHasBeenDown = false;
			}
		}

		if(Input.GetMouseButtonUp(0))
		{	
			// Note: This code block might not be necessary; it's just one frame off from GetMouseButton so not much would change anyways
			// Handle modifier status 
			if(ctrlHasBeenDown)
			{
				if(!(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
					ctrlHasBeenDown = false;				
			}
			if(shiftHasBeenDown)
			{
				if(!(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
					shiftHasBeenDown = false;
			}
			// Determine size of selection box based on origin and current mouse position
			float _invertedY = Screen.height - Input.mousePosition.y;
			selectionBoxSize = new Vector2(Input.mousePosition.x - selectionBoxOrigin.x, (selectionBoxOrigin.y - _invertedY) * -1);

			// Single unit handling
			if(Mathf.Abs(selectionBoxSize.x) < MinimumSelectionBoxSize.x || Mathf.Abs(selectionBoxSize.y) < MinimumSelectionBoxSize.y)
			{
				Debug.Log("Handling single unit..");
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if(Physics.Raycast(ray, out hit))
				{
					if(hit.collider.gameObject.tag == "SelectableUnit")
					{
						Debug.Log("Hit a selectable unit..");
						// Additional single unit 
						if(shiftHasBeenDown)
						{	
							// Add additional single unit if it has not already been added
							if(!hit.collider.gameObject.GetComponent<SelectableUnit>().isSelected)
								SelectAdditionalUnit(hit.collider.gameObject);
						}
						// Select Additional/Deselect single unit
						else if(ctrlHasBeenDown)
						{
							// Add additional single unit if it has not already been added
							if(hit.collider.gameObject.GetComponent<SelectableUnit>().isSelected)
								DeselectSingleUnit(hit.collider.gameObject);
							else
								SelectAdditionalUnit(hit.collider.gameObject);
						}
						// Select single unit
						else 
						{
							SelectSingleUnit(hit.collider.gameObject);							
						}
					}
					else
					{
						// Deselect all selected units if mouse position hit an object but is not selected (tag) 
						DeselectAllUnits();
					}
				}
				else
				{
					// Deselect all selected units if the mouse position didn't hit a game object
					DeselectAllUnits();
				}
			}
			// Multiple unit handling
			else
			{
				Debug.Log("Handling multi units..");
				// Fill the selectableUnits array with all the selectable units that exist within the scene 
				// Note: May want to increase efficiency by only filling the array with units that are selectable and are visible
				selectableUnits = new List<GameObject>(GameObject.FindGameObjectsWithTag("SelectableUnit"));

				// Check if the previous state of selected units needs to be saved or not
				if(!ctrlHasBeenDown && !shiftHasBeenDown)
					DeselectAllUnits();

				bool aUnitWasModified = false;
				foreach(GameObject unit in selectableUnits)
				{
					// Convert the world position of the unit to a screen position and then to a GUI point
					Vector3 _screenPos = Camera.main.WorldToScreenPoint(unit.transform.position);
					Vector2 _screenPoint = new Vector2(_screenPos.x, Screen.height - _screenPos.y);

					if(SelectionBoxRect.Contains(_screenPoint, true))
					{
						if(ctrlHasBeenDown)
						{
							aUnitWasModified = true;
							if(unit.GetComponent<SelectableUnit>().isSelected)

								DeselectSingleUnit(unit);
							else
								SelectAdditionalUnit(unit);
						}
						// Adding unit regardless if shift was held down or not
						else 
						{
							if(!unit.GetComponent<SelectableUnit>().isSelected)
							{
								aUnitWasModified = true;
								SelectAdditionalUnit(unit);
							}								
						}
					}
				}

				// If no units were selected empty selected units list
				if(!aUnitWasModified)
					DeselectAllUnits();
			}

			// Select/Deselect units based on their position in relationship to the selection box


			// Reset selection box 
			SelectionBoxRect.width = 0;
			SelectionBoxRect.height = 0;
			selectionBoxSize = Vector2.zero;
		}

		// Testing
		debugMessageTimer += Time.deltaTime;
		if(debugMessageTimer > 1)
		{
			Debug.Log("Currently " + selectedUnits.Count + " units are selected");
			debugMessageTimer = 0;
		}			
	}

	private void SelectSingleUnit(GameObject unit)
	{ 
		DeselectAllUnits();
		SelectAdditionalUnit(unit);
	}

	private void DeselectSingleUnit(GameObject unit)
	{
		unit.SendMessage("Deselected", SendMessageOptions.DontRequireReceiver);
		selectedUnits.Remove(unit);
	}

	private void SelectAdditionalUnit(GameObject unit)
	{
		unit.SendMessage("Selected", SendMessageOptions.DontRequireReceiver);
		selectedUnits.Add(unit);
	}

	private void DeselectAllUnits()
	{
		foreach(GameObject obj in selectedUnits)
		{
			obj.SendMessage("Deselected", SendMessageOptions.DontRequireReceiver);
		}
		if(selectedUnits.Count > 0)
			selectedUnits.Clear();
	}
}
