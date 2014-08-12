using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitManager : MonoBehaviour
{
    public LayerMask CanTravelOn;
    public static UnitManager Instance { private set; get; }

    public SelectableUnit selectedUnit;

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
        if (Input.GetMouseButtonDown(0))
        {
            // Single unit selection
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
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
        }

        if (selectedUnit != null && Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, CanTravelOn))
            {
                selectedUnit.MoveCommand(hit.point);
            }
        }
    }
}
            
	