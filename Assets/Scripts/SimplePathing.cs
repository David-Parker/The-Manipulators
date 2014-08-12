using Pathfinding;
using UnityEngine;
using System.Collections;

public class SimplePathing : MonoBehaviour
{

    public Vector3 Target;

    // The calculated path
    public Path path;

    // The AI's speed per second
    public float speed = 100f;

    // The max distance from the AI to a waypoint for it to continue to the next waypoint
    private float nextWaypointDistance = 3f;

    // The waypoint we are currently moving towards
    private int currentWaypoint = 0;

    private Seeker seeker;
    private CharacterController controller;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        controller = GetComponent<CharacterController>();

        // Start a new path to the TargetPosition, return the result to the OnPathComplete function
        seeker.StartPath(transform.position, Target, OnPathComplete);
    }

    public void OnPathComplete(Path p)
    {
        Debug.Log("Yay, we got a path back. Did it have an error? " + p.error);
        if (!p.error)
        {
            path = p;
            // Reset the waypoint counter
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        if (path == null)
        {
            // We have no path to move after yet
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            Debug.Log("End of Path Reached");
            return;
        }

        // Direction to the next waypoint 
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        
        // Rotate the GameObject to look towards the direction it is moving
        Vector3 newForward = Vector3.RotateTowards(transform.forward, dir, 1.0f*Time.deltaTime, 0);
        
        // Modify the direction of its Z axis
        transform.forward = newForward;

        dir *= speed*Time.fixedDeltaTime;
        controller.SimpleMove(dir);

        // Check if we are close enough to the next waypoint 
        // If we are, proceed to follow the next waypoint
        if (Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]) < nextWaypointDistance)
        {
            currentWaypoint++;
            return;
        }
    }

    public void OnDisable()
    {
        GetComponent<Seeker>().pathCallback -= OnPathComplete;
    }
}
