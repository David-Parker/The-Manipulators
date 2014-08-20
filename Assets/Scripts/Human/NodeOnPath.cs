using Pathfinding;
using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider))]
public class NodeOnPath : MonoBehaviour
{
    public PathNodeType NodeType;


    public Transform[] PathFromNode
    {
        get
        {
            if (NodeType == PathNodeType.EnterExit)
            {
                return PathForEnterExit;
            }
            else
            {
                return null;
            }
        }
        private set { PathFromNode = value; }
    }
    public Transform[] PathForEnterExit;
    public enum PathNodeType
    {
        EnterExit,
        Path,
        Rest,
    }


    void OnTriggerEnter(Collider col)
    {
        Debug.Log(col.gameObject);
        if (col.gameObject.tag == "Human")
        {
            col.gameObject.GetComponent<HumanAI>().NodeReached(NodeType);
        }
    }
}
