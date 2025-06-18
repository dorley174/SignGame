using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CapsuleCollider2D), typeof(LayerMask), typeof(NavMeshAgent))]
public class LandEnemyMovement : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private LayerMask consideredMasks;
    [SerializeField]
    private LayerMask notGroundMasks;
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
    [SerializeField]
    private float groundDetectionOffset;
    [SerializeField]
    private float jumpHeight;
    [SerializeField]
    private float jumpTime;
    [SerializeField]
    private CapsuleCollider2D enemyCollider;
    [SerializeField]
    private bool onGround = false;

    [SerializeField, Range(-10f, 50f)]
    private float verticalSpeed;
    [SerializeField] private float gravity;
    void Awake()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
        if (enemyCollider == null)
        {
            enemyCollider = GetComponent<CapsuleCollider2D>();
        }
    }
    private void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.autoTraverseOffMeshLink = false;
        agent.speed = speed;
        agent.acceleration = acceleration;
        agent.stoppingDistance = stoppingDistance;
        playerTag = target.gameObject.tag;
    }
    void Update()
    {
        IsStanding();
        Jump();
        HandleGravity();
    }
    private void IsStanding()
    {
        if (!agent.isOnOffMeshLink)
        {
            onGround = (Physics2D.CapsuleCast(enemyCollider.bounds.center, enemyCollider.size, enemyCollider.direction, 0, Vector2.down, groundDetectionOffset, ~notGroundMasks));
        }
        else
        {
            onGround = true;
        }
    }
    private void Jump()
    {
        if (agent.isOnOffMeshLink)
        {
            StartCoroutine(TraverseLinkWithBezier());
        }
    }
    private IEnumerator TraverseLinkWithBezier()
    {
        OffMeshLinkData linkData = agent.currentOffMeshLinkData;
        Vector2 startPos = agent.transform.position;
        Vector2 endPos = linkData.endPos;
        float curveHeight = (endPos.y - startPos.y) * 1f;
        Vector2 controlPoint = (startPos + endPos) * 0.5f + Vector2.up * curveHeight * ((startPos.y < endPos.y) ? 1f : -1f);

        float time = 0f;
        while (time < jumpTime)
        {
            float t = time / jumpTime;
            Vector2 bezierPos = Mathf.Pow(1 - t, 2) * startPos + 2 * (1 - t) * t * controlPoint + Mathf.Pow(t, 2) * endPos;
            agent.transform.position = bezierPos;
            time += Time.deltaTime;
            yield return null;
        }
        agent.transform.position = endPos;
        agent.CompleteOffMeshLink();
    }
    private void HandleGravity()
    {
        if (!agent.isOnOffMeshLink) {
            if (!onGround)
            {
                verticalSpeed -= gravity * Time.deltaTime;
            }
            else
            {
                verticalSpeed = -speed;
            }
            FindPlayer();
            Vector2 currentPos = agent.transform.position;
            Vector2 nextPos = new Vector2(currentPos.x, currentPos.y + verticalSpeed * Time.deltaTime);
            if (IsPointOnNavMesh(nextPos))
            {
                agent.transform.position = nextPos;
            }
        }
    }
    private void FindPlayer()
    {
        Vector2 targetPos = target.position;
        Vector2 agentPos = agent.transform.position;
        if ((agentPos - targetPos).magnitude <= visionRange)
        {
            if (GeneralEnemyBehaviour.LookingDirectlyAtPlayer(agentPos, targetPos, consideredMasks, playerTag))
            {
                agent.SetDestination(targetPos);
                agent.stoppingDistance = stoppingDistance;
            }
            else
            {
                agent.stoppingDistance = 0;
            }
        }
    }
    private bool IsPointOnNavMesh(Vector2 point, float maxDistance = 1.0f)
    {
        NavMeshHit hit;
        return NavMesh.SamplePosition(point, out hit, maxDistance, NavMesh.AllAreas);
    }
}