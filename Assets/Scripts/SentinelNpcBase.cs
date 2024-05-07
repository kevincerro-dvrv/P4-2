using System.Collections;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;

public class SentinelNpcBase : MonoBehaviour
{
    public float viewDegrees;
    public float viewMaxDistance;
    public LayerMask targetLayer;

    public Material guardMaterial;
    public Material followMaterial;
    public Material sleepMaterial;

    protected NavMeshAgent agent;
    protected MeshRenderer meshRenderer;

    protected GameObject target;

    protected SentinelNpcStatus status = SentinelNpcStatus.Guard;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        meshRenderer = GetComponent<MeshRenderer>();

        StartCoroutine(RandomSleep());
    }

    protected virtual void FixedUpdate()
    {
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

        if (agent.isStopped) {
            return;
        }

        if (target == null) {
            DetectVisiblePlayers();
            return;
        }

        agent.SetDestination(target.transform.position);
    }

    protected void DetectVisiblePlayers()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, viewMaxDistance, targetLayer);

        foreach (Collider collider in colliders)
        {
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
                        // The target is within the cone
                        Debug.Log("Player detected!");
                        target = hit.collider.gameObject;
                        status = SentinelNpcStatus.Follow;
                        agent.isStopped = false;
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
        Vector3 leftRayPoint = transform.position + (Quaternion.Euler(0, -viewDegrees/2, 0) * transform.forward * viewMaxDistance);
        Vector3 rightRayPoint = transform.position + (Quaternion.Euler(0, viewDegrees/2, 0) * transform.forward * viewMaxDistance);

        Gizmos.DrawLine(transform.position, frontRayPoint);
        Gizmos.DrawLine(transform.position, leftRayPoint);
        Gizmos.DrawLine(transform.position, rightRayPoint);

        Gizmos.DrawLine(frontRayPoint, leftRayPoint);
        Gizmos.DrawLine(frontRayPoint, rightRayPoint);
        Gizmos.DrawLine(rightRayPoint, leftRayPoint);
    }

    private IEnumerator RandomSleep()
    {
        while(true) {
            if (status != SentinelNpcStatus.Follow) {
                yield return new WaitUntil(() => status == SentinelNpcStatus.Follow);
            }

            if (Random.value <= 0.15f) {
                Debug.Log("Sentinel is going to sleep!");
                target = null;
                agent.ResetPath();
                status = SentinelNpcStatus.Sleep;
                agent.isStopped = true;
                Invoke("AwakeSentinel", 5f);
            }

            yield return new WaitForSeconds(2.5f);
        }
    }

    protected void AwakeSentinel()
    {
        agent.isStopped = false;
        status = SentinelNpcStatus.Guard;
    }
}

public enum SentinelNpcStatus {
    Guard,
    Follow,
    Sleep,
    Stop,
}
