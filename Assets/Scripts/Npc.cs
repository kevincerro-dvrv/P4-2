using UnityEngine;
using UnityEngine.AI;

public class Npc : MonoBehaviour {

    public GameObject target;
    private NavMeshAgent agent;

    void Start () {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update () {
        agent.SetDestination(target.transform.position);
    }
}
