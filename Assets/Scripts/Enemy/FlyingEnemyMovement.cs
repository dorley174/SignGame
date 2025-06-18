using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(LayerMask), typeof(NavMeshAgent))]
public class FlyingEnemyMovement : MonoBehaviour
{
    [SerializeField] 
    private Transform target;
    [SerializeField]
    private LayerMask consideredMasks;
    private string playerTag;
    private NavMeshAgent agent;
    [SerializeField, Range(0f, 100f)]
    private float speed;
    [SerializeField, Range(0f, 50f)]
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
    public NavMeshAgent EnemyAgent
    {
        get
        {
            return agent;
        }
    }
    private void Awake()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
    }
    void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = speed;
        agent.acceleration = acceleration;
        agent.stoppingDistance = stoppingDistance;
        playerTag = target.gameObject.tag;
    }

    void Update()
    {
        if ((agent.transform.position - target.position).magnitude <= visionRange)
        {
            if (GeneralEnemyBehaviour.LookingDirectlyAtPlayer(agent.transform.position, target.position, consideredMasks, playerTag))
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
}
