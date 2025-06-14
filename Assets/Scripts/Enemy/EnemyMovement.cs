using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] 
    private Transform target;
    [SerializeField]
    private string[] masksToRaycast;
    private LayerMask consideredMasks;
    private string playerTag;
    private NavMeshAgent agent;
    [SerializeField, Range(0f, 10f)]
    private float speed;
    [SerializeField, Range(0f, 20f)]
    private float acceleration;
    [SerializeField]
    private float stoppingDistance;
    [SerializeField]
    private float visionRange;
    public Transform Target
    {
        set
        {
            target = value;
        }
        get
        {
            return target;
        }
    }
    void Start()
    {
        if (agent == null) {
            agent = GetComponent<NavMeshAgent>();
        }
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = speed;
        agent.acceleration = acceleration;
        agent.stoppingDistance = stoppingDistance;
        if (masksToRaycast != null)
        {
            consideredMasks = LayerMask.GetMask(masksToRaycast);
            Debug.Log(consideredMasks.ToString());
        }
        playerTag = target.gameObject.tag;
        Debug.Log(playerTag);
        agent.SetDestination(agent.transform.position);
    }

    void Update()
    {
        if ((agent.transform.position - target.position).magnitude <= visionRange)
        {
            Debug.Log((agent.transform.position - target.position).magnitude);
            if (LookingDirectlyAtPlayer(agent.transform.position, target.position, consideredMasks, playerTag))
            {
                agent.SetDestination(target.position);
                agent.stoppingDistance = stoppingDistance;
            }
            else
            {
                agent.stoppingDistance = 0;
            }
        }
    }
    private bool LookingDirectlyAtPlayer(Vector3 p1, Vector3 p2, LayerMask masks, string tag)
    {
        RaycastHit2D hit = Physics2D.Linecast(p1, p2, masks);
        Debug.Log(hit);
        if (hit.collider.tag == tag)
        {
            return true;
        }
        return false;
    }
}
