using Pathfinding;
using UnityEngine;
using System.Collections;

[RequireComponent (typeof (StateDisplay))]
[RequireComponent (typeof (AIPath))]
[RequireComponent (typeof (SimpleSmoothModifier))]
[RequireComponent (typeof(CapsuleCollider))]
public class SelectableUnit : MonoBehaviour 
{

	public bool isSelected {get; private set;}

    private AIPath path;
    private GameObject targetPosition;
    private StateDisplay stateDisplay;

	void Awake()
	{
	    path = GetComponent<AIPath>();
        stateDisplay = GetComponent<StateDisplay>();
		Deselected();
	}

    void Start()
    {
        // Create the Transform regarding the target position of this transform
        targetPosition = new GameObject("AlienTargetPosition");
        targetPosition.transform.parent = GameObject.Find("PathingWaypoints").transform;
        Debug.Log(transform.position);
        targetPosition.transform.position = transform.position;
        path.target = targetPosition.transform;
    }

	public virtual void Selected()
	{
        stateDisplay.UpdateState("Selected");
		isSelected = true;

		foreach(Transform child in transform)
		{
            if (child.gameObject.name != "StateBillBoard")
                child.renderer.material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");
		}
	}

	public virtual void Deselected()
	{
        stateDisplay.UpdateState("Deselected");
		isSelected = false;

		foreach(Transform child in transform)
		{
            if (child.gameObject.name != "StateBillBoard")
		        child.renderer.material.shader = Shader.Find("Diffuse");
		}
	}

    public virtual void MoveCommand(Vector3 movePosition)
    {
        stateDisplay.UpdateState("Move Command");
        // Clear ability if selected
        targetPosition.transform.position = movePosition;       
    }
}
