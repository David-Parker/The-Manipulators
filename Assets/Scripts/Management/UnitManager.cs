using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitManager : MonoBehaviour
{
    public LayerMask CanTravelOn;
    public static UnitManager Instance { private set; get; }

    public SelectableUnit selectedUnit { private set; get; }

    void Awake()
    {
        if (Instance != null  && Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);
        Instance = this;
    }

    void Update()
    {        
    }

    public void OnLeftClick(RaycastHit hit)
    {
        if (hit.collider.gameObject.tag == "SelectableUnit")
        {
            if (selectedUnit != null)
            {
                // Deselect previous unit
                selectedUnit.Deselected();
            }

            // Select new unit
            selectedUnit = hit.collider.gameObject.GetComponent<SelectableUnit>();
            selectedUnit.Selected();
        }
        else
        {
            if (selectedUnit != null)
            {
                selectedUnit.Deselected();
                selectedUnit = null;
            }
        }
    }

    public void OnRightClick(RaycastHit hit)
    {
        // Check if unit is selected and that the point is traversable
        if (selectedUnit != null && ((1 << hit.collider.gameObject.layer) & CanTravelOn) > 0)
        {
            selectedUnit.MoveCommand(hit.point);
        }
    }
}
            
	