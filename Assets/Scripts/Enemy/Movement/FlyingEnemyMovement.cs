using Unity.Android.Gradle.Manifest;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(LayerMask), typeof(NavMeshAgent))]
public class FlyingEnemyMovement : MonoBehaviour
{
    [SerializeField]
    private EnemyInteractionCharacteristics stats;
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
    private void OnValidate()
    {
        if (stats != null)
        {
            visionRange = stats.visionRange;
            speed = stats.speed;
            acceleration = stats.acceleration;
            stoppingDistance = stats.stoppingDistance;
            if (stats.isGround)
            {
                Debug.Log("Wrong enemy type! :: FlyingEnemyMovement; OnValidate");
            }
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
        playerTag = target.gameObject.tag;
        SetAgentParameters();
    }
    private void SetAgentParameters()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = speed;
        agent.acceleration = acceleration;
        agent.stoppingDistance = stoppingDistance;
    }

    void Update()
    {
        if (GeneralEnemyBehaviour.LookingDirectlyAtPlayer(agent.transform.position, target.position, visionRange, consideredMasks, playerTag))
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
