using System.Collections.Generic;
using UnityEngine;

public class SentinelNpc4 : SentinelNpcBase
{
    public bool goToLastKnownPosition = false;

    public List<GameObject> targets;

    private int currentTarget = 0;

    override protected void Start()
    {
        base.Start();

        agent.SetDestination(targets[currentTarget].transform.position);
    }

    override protected void FixedUpdate()
    {
        if (!agent.pathPending && agent.remainingDistance <= 0.5f) {
            currentTarget = (currentTarget + 1) % targets.Count;
            agent.SetDestination(targets[currentTarget].transform.position);
            agent.isStopped = true;
            status = SentinelNpcStatus.Stop;
            Invoke("AwakeSentinel", 5f);
        }

        // Set color based on status
        switch (status) {
            case SentinelNpcStatus.Follow:
                meshRenderer.material = followMaterial;
                break;
            case SentinelNpcStatus.Sleep:
                meshRenderer.material = sleepMaterial;
                break;
            case SentinelNpcStatus.Guard:
            case SentinelNpcStatus.Stop:
            default:
                meshRenderer.material = guardMaterial;
                break;
        }
        
        if (target == null) {
            DetectVisiblePlayers();
            return;
        }

        if (agent.isStopped) {
            return;
        }

        agent.SetDestination(target.transform.position);

        CheckTargetVisibility();
    }

    private void CheckTargetVisibility()
    {
        Vector3 directionToTarget = target.transform.position - transform.position;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, directionToTarget, out hit, viewMaxDistance)){
            if (hit.collider.gameObject != target.gameObject)
            {
                // The target is not visible
                Debug.Log("Player lost!");
                target = null;
                status = SentinelNpcStatus.Guard;

                if (!goToLastKnownPosition) {
                    agent.ResetPath();
                }
            }
        } else {
            // The target is not visible
            Debug.Log("Player lost!");
            target = null;
            status = SentinelNpcStatus.Guard;
            
            
            if (!goToLastKnownPosition) {
                agent.ResetPath();
            }
        }
    }
}
