using UnityEngine;

public class SentinelNpc3 : SentinelNpcBase
{
    public bool goToLastKnownPosition = false;

    override protected void FixedUpdate()
    {
        // Set color based on status
        switch (status) {
            case SentinelNpcStatus.Follow:
                meshRenderer.material = followMaterial;
                break;
            case SentinelNpcStatus.Guard:
            default:
                meshRenderer.material = guardMaterial;
                break;
        }
        
        if (target == null) {
            DetectVisiblePlayers();
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
