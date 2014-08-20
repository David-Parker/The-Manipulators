using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] Humans;
    public Transform[] EnterExitNodes;
    public int MaximumHumans = 10;
    public float AverageSpawnRate = 10;
    public float RangeFromAverageSpawnRate = 5;
    public float AverageHumanRestTime = 20;
    public float RangeFromAverageHumanRestTime = 10;
    public float HumanRunSpeed;
    public float HumanJogSpeed;
    public float HumanWalkSpeed;
    
    public static SpawnManager Instance;

    public int humanCount { get; set; }
    private float timeSinceLastSpawn;
    private bool readyToSpawn;  

    void Awake()
    {
        if (Instance != null && this != Instance)
            Destroy(gameObject);

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        humanCount = 0;
        timeSinceLastSpawn = float.MaxValue;
        readyToSpawn = true;
    }

    void Update()
    {
        if (humanCount < MaximumHumans && readyToSpawn)
        {
            readyToSpawn = false;
            Invoke("SpawnHuman", Random.Range(AverageSpawnRate - RangeFromAverageHumanRestTime, AverageSpawnRate + RangeFromAverageSpawnRate));
        }
    }

    void SpawnHuman()
    {
        humanCount++;
        readyToSpawn = true;
        Transform NodeToEnterOn = EnterExitNodes[Random.Range(0, EnterExitNodes.Length)];
        GameObject human = Instantiate(Humans[Random.Range(0, Humans.Length)], NodeToEnterOn.transform.position, NodeToEnterOn.rotation) as GameObject;

        // 75% walk, 25% jog 
        int speedRN = Random.Range(0, 4);
        human.GetComponent<AIPath>().speed = speedRN < 3 ? HumanWalkSpeed : HumanJogSpeed;
        human.GetComponent<HumanAI>().StateOfMovement = speedRN < 3
            ? HumanAI.MovementState.Walking
            : HumanAI.MovementState.Jogging;
        
        human.GetComponent<HumanAI>().PathFromExit = NodeToEnterOn.GetComponent<NodeOnPath>().PathFromNode;
    }
}
