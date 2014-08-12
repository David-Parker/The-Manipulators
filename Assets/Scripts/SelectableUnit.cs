using Pathfinding;
using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Seeker))]
[RequireComponent (typeof (AIPath))]
[RequireComponent (typeof (SimpleSmoothModifier))]
[RequireComponent (typeof(CharacterController))]
[RequireComponent (typeof(CapsuleCollider))]
public class SelectableUnit : MonoBehaviour 
{

	public bool isSelected {get; private set;}

    private AIPath path;
    private GameObject targetPosition;

	void Awake()
	{
	    path = GetComponent<AIPath>();
		Deselected();
	}

    void Start()
    {
        // Create the Transform regarding the target position of this transform
        targetPosition = new GameObject("TargetPosition");
        targetPosition.transform.parent = GameObject.Find("PathingWaypoints").transform;
        targetPosition.transform.position = transform.position;
        path.target = targetPosition.transform;
    }

	public virtual void Selected()
	{
		isSelected = true;

		foreach(Transform child in transform)
		{
		    child.renderer.material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");
		}
	}

	public virtual void Deselected()
	{
		isSelected = false;

		foreach(Transform child in transform)
		{
		    child.renderer.material.shader = Shader.Find("Diffuse");
		}
	}

    public virtual void MoveCommand(Vector3 movePosition)
    {
        // Clear ability if selected

        targetPosition.transform.position = movePosition;
        
    }
}
