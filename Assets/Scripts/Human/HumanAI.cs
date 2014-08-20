using System;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Collections;
using PathNode = Pathfinding.PathNode;
using Random = UnityEngine.Random;

[RequireComponent(typeof (StateDisplay))]
[RequireComponent(typeof(AIPath))]
[RequireComponent(typeof(SimpleSmoothModifier))]
[RequireComponent(typeof(CharacterController))]
public class HumanAI : MonoBehaviour
{
    public Transform[] PathFromExit {  get; set; }
    public MovementState StateOfMovement { get; set; }

    private enum HumanState
    {
        AutoPathing,
        EventInteraction,
        Resting,
        Exiting
    }

    public enum MovementState
    {
        Walking,
        Jogging,
        Running
    }

    // Pre-disposed from 1-3 
    private int strengthLevel;
    private int charismaLevel;

    // Pre-disposed 0-3 
    private int suspicionLevel;

    // Visibility
    private bool strengthIsVisible;
    private bool charismaIsVisible;
    private bool suspicionIsVisible;

    private StateDisplay stateDisplay;
    private List<Transform> traversedNodes;

    private HumanState humanState;
    private int currentPathNode;

    private GameObject targetPosition;
    private AIPath path;
    private String statsText;

    void Awake()
    {
        stateDisplay = GetComponent<StateDisplay>();
        path = GetComponent<AIPath>();
    }

    void Start()
    {
        currentPathNode = 0;
        humanState = HumanState.AutoPathing;
        StatGeneration();
        UpdateStateText();

        targetPosition = new GameObject("HumanTargetPosition");
        targetPosition.transform.parent = GameObject.Find("PathingWaypoints").transform;
        targetPosition.transform.position = PathFromExit[currentPathNode].position;
        path.target = targetPosition.transform;
    }

    void Update()
    {
        switch (humanState)
        {
            case HumanState.AutoPathing:
                
                break;
            case HumanState.EventInteraction:

                break;
        }
    }

    void StatGeneration()
    {
        // 40% chance for 1, 40% chance for 2, 20% for 3 
        int strengthRN = Random.Range(0, 10);
        int charismaRN = Random.Range(0, 10);

        if (strengthRN > 7)
            strengthLevel = 3;
        else if (strengthRN > 3)
            strengthLevel = 2;
        else
            strengthLevel = 1;

        if (charismaRN > 7)
            charismaLevel = 3;
        else if (charismaRN > 3)
            charismaLevel = 2;
        else
            charismaLevel = 1;

        // 40% chance for 0, 40% chance for 1, 20% chance for 2
        int suspicionRN = Random.Range(0, 10);

        if (suspicionRN > 7)
            suspicionLevel = 2;
        else if (suspicionRN > 3)
            suspicionLevel = 1;
        else if (suspicionRN > 1)
            suspicionLevel = 0;
    }

    void UpdateStateText()
    {
        statsText = "Str: " + (strengthIsVisible ? strengthLevel.ToString() : "?") + "/ Sus: " +
                    (suspicionIsVisible ? suspicionLevel.ToString() : "?") + "/ Cha: " +
                    (charismaIsVisible ? charismaLevel.ToString() : "?");

        stateDisplay.UpdateState(humanState.ToString() + " | " + statsText);
    }

    public void NodeReached(NodeOnPath.PathNodeType pathNodeType)
    {
        if (PathFromExit.Length - 1 == currentPathNode)
        {
            // Exit node reached
            SpawnManager.Instance.humanCount--;
            Destroy(gameObject);
        }
        else if (PathFromExit.Length - 2 == currentPathNode)
        {
            // 25% chance to go around again
            if (Random.Range(0, 2) == 0)
            {
                currentPathNode = 0;
                targetPosition.transform.position = PathFromExit[currentPathNode].position;
            }
            else
            {
                // Send to exit 
                currentPathNode++;
                targetPosition.transform.position = PathFromExit[currentPathNode].position;
            }
        }
        else if(!(currentPathNode == 0 && pathNodeType == NodeOnPath.PathNodeType.EnterExit))
        {
            if (PathFromExit[currentPathNode + 1].GetComponent<NodeOnPath>().NodeType == NodeOnPath.PathNodeType.Rest)
            {
                // 50% chance to move towards rest node and rest there, 50% chance to pass
                int restRN = Random.Range(0, 2);
                currentPathNode = restRN == 1 ? currentPathNode + 1 : currentPathNode + 2;
                if (restRN == 1)
                {
                    humanState = HumanState.Resting;                
                    UpdateStateText();
                }

                targetPosition.transform.position = PathFromExit[currentPathNode].position;
            }
            else if (PathFromExit[currentPathNode].GetComponent<NodeOnPath>().NodeType == NodeOnPath.PathNodeType.Rest && humanState == HumanState.Resting)
            {
                // Run resting co-routine to take a rest and to have a 50% chance after each rest to take an additional resting period
                StartCoroutine(Resting());
            }
            else
            {
                // Go to next node on path if this isn't the first node and they're colliding with the initial Enter/Exit node 
                // The next node is a path node, by default
                currentPathNode++;
                targetPosition.transform.position = PathFromExit[currentPathNode].position;
            }
        }

        // Down here on initial spawn
    }

    private IEnumerator Resting()
    {
        float currentTimeSpentResting = 0;
        float timeToSpendDuringThisRest =
            Random.Range(
                SpawnManager.Instance.AverageHumanRestTime - SpawnManager.Instance.RangeFromAverageHumanRestTime,
                SpawnManager.Instance.AverageHumanRestTime + SpawnManager.Instance.RangeFromAverageHumanRestTime);

        while (currentTimeSpentResting < timeToSpendDuringThisRest)
        {
            currentTimeSpentResting += Time.deltaTime;
            yield return null;
        }
        // Chance to rest more
        int restAgainRN = Random.Range(0, 2);
        if (restAgainRN == 0)
        {
            StartCoroutine(Resting());
            yield break;
        }
        else
        {
            currentPathNode++;
            targetPosition.transform.position = PathFromExit[currentPathNode].position;

            humanState = HumanState.AutoPathing;
            UpdateStateText();
            yield break;
        }
    }
}
