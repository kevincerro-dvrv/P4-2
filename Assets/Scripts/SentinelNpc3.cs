using UnityEngine;

public class SentinelNpc3 : SentinelNpcBase
{
    public bool goToLastKnownPosition = false;

    override protected void FixedUpdate()
    {
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

                if (!goToLastKnownPosition) {
                    agent.ResetPath();
                }
            }
        } else {
            // The target is not visible
            Debug.Log("Player lost!");
            target = null;
            
            
            if (!goToLastKnownPosition) {
                agent.ResetPath();
            }
        }
    }
}
