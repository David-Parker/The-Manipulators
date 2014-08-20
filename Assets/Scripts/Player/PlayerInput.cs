using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour
{
    void Update()
    {
        // Left click 
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                // Handle unit selection
                UnitManager.Instance.OnLeftClick(hit);
                // Handle spore selection
                if (hit.collider.gameObject.GetComponent<Spore>() != null)
                {
                    hit.collider.gameObject.GetComponent<Spore>().OnLeftClick();
                }
            }
        }

        // Right click
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                // Handle unit selection
                UnitManager.Instance.OnRightClick(hit);
            }
        }
    }
}
