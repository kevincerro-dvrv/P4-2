using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SentinelNpc : MonoBehaviour
{
    public float viewDegrees;
    public float viewMaxDistance;
    public LayerMask targetLayer;

    private NavMeshAgent agent;

    private GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        if (target == null) {
            DetectVisiblePlayers();
            return;
        }

        agent.SetDestination(target.transform.position);
    }

    private void DetectVisiblePlayers()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, viewMaxDistance, targetLayer);

        foreach (Collider collider in colliders)
        {
            Debug.Log("Collider" + collider.gameObject.name);

            // Check if the collider is within the cone angle
            Vector3 toCollider = collider.transform.position - transform.position;
            float angleToCollider = Vector3.Angle(transform.forward, toCollider);
            
            if (angleToCollider <= viewDegrees)
            {
                // Check if the collider is in the cone's line of sight
                RaycastHit hit;
                if (Physics.Raycast(transform.position, toCollider.normalized, out hit, viewMaxDistance, targetLayer))
                {
                    if (hit.collider.gameObject == collider.gameObject)
                    {
                        // The player is within the cone
                        Debug.Log("Player detected!");
                        target = hit.collider.gameObject;
                    }
                }
            }
        }
 
    }

    public void OnDrawGizmos()
    {
        // Draw cone shape in the editor
        Gizmos.color = Color.red;
        Vector3 frontRayPoint = transform.position + (transform.forward * viewMaxDistance);
        Vector3 leftRayPoint = transform.position + (Quaternion.Euler(0, -viewDegrees, 0) * transform.forward * viewMaxDistance);
        Vector3 rightRayPoint = transform.position + (Quaternion.Euler(0, viewDegrees, 0) * transform.forward * viewMaxDistance);

        Gizmos.DrawLine(transform.position, frontRayPoint);
        Gizmos.DrawLine(transform.position, leftRayPoint);
        Gizmos.DrawLine(transform.position, rightRayPoint);

        Gizmos.DrawLine(frontRayPoint, leftRayPoint);
        Gizmos.DrawLine(frontRayPoint, rightRayPoint);
        Gizmos.DrawLine(rightRayPoint, leftRayPoint);
    }
}
