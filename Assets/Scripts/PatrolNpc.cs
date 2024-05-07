using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolNpc : MonoBehaviour {

    public List<GameObject> targets;
    private NavMeshAgent agent;
    private int currentTarget = 0;

    void Start () {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(targets[currentTarget].transform.position);
    }

    void Update () {
        if (!agent.pathPending && agent.remainingDistance <= 0.5f) {
            currentTarget = (currentTarget + 1) % targets.Count;
            agent.SetDestination(targets[currentTarget].transform.position);
        }
    }
}
