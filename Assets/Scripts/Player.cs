using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour {

    private NavMeshAgent agent;

    private Vector3 lastDestination;
    private Vector3 destination;

    void Start () {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update () {
        if (lastDestination == destination) {
            return;
        }

        lastDestination = destination;
        agent.SetDestination(destination);
    }

    public void SetDestination(Vector3 destination)
    {
        //Instantiate()
        this.destination = destination;
    }
}
