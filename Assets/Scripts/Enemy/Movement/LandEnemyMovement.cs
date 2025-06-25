using System.Collections;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CapsuleCollider2D), typeof(LayerMask), typeof(NavMeshAgent))]
public class LandEnemyMovement : MonoBehaviour
{
    //ScriptableObject
    [SerializeField]
    private EnemyInteractionCharacteristics stats;
    //Target detection
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float visionRange;
    [SerializeField]
    private LayerMask consideredMasks;
    [SerializeField]
    private LayerMask notGroundMasks;
    private string playerTag;
    //NavMesh parameters
    private NavMeshAgent agent;
    [SerializeField, Range(0f, 100f)]
    private float speed;
    [SerializeField, Range(0f, 50f)]
    private float acceleration;
    [SerializeField]
    private float stoppingDistance;
    //Physics
    [SerializeField]
    private float groundDetectionOffset;
    [SerializeField]
    private float jumpHeight;
    [SerializeField]
    private float minJumpHeight;
    [SerializeField]
    private float jumpTime;
    [SerializeField]
    private CapsuleCollider2D enemyCollider;
    [SerializeField]
    private bool onGround = false;
    [SerializeField, Range(-50f, 50f)]
    private float verticalSpeed;
    [SerializeField]
    private float gravity;
    //Other
    [SerializeField]
    private bool isJumping;
    [SerializeField]
    private Vector3[] corners;
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
                jumpHeight = stats.maxJumpHeight;
                minJumpHeight = stats.minJumpHeight;
                jumpTime = stats.jumpTime;
                gravity = stats.gravity;
            }
            else
            {
                Debug.Log("Wrong enemy type! :: LandEnemyMovement; OnValidate");
            }
        }
    }
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
        if (ShouldJump())
        {
            StartCoroutine(JumpBezier());
        }
        if (!isJumping)
        {
            IsStanding();
            HandleGravity();
            FollowPlayer();
        }
    }
    private IEnumerator JumpBezier()
    {
        isJumping = true;
        Vector2 startPos = corners[0];
        Vector2 endPos = corners[1];

        float peakY = Mathf.Max(startPos.y, endPos.y);
        Vector2 controlPoint = (startPos + endPos) * 0.5f;
        controlPoint.y = peakY;

        float time = 0f;
        while (time < jumpTime)
        {
            float t = time / jumpTime;
            Vector2 bezierPos = Mathf.Pow(1 - t, 2) * startPos + 2 * (1 - t) * t * controlPoint + Mathf.Pow(t, 2) * endPos;
            agent.transform.position = bezierPos;
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        agent.transform.position = endPos;
        isJumping = false;
    }
    private void IsStanding()
    {
        onGround = (Physics2D.CapsuleCast(enemyCollider.bounds.center, enemyCollider.size, enemyCollider.direction, 0, Vector2.down, groundDetectionOffset, ~notGroundMasks));
    }
    private bool ShouldJump()
    {
        corners = agent.path.corners;
        if (corners.Length >= 2 && onGround && !isJumping)
        {
            Vector3 pos1 = corners[0];
            Vector3 pos2 = corners[1];
            float heightDifference = Mathf.Abs(pos1.y - pos2.y);
            if (heightDifference <= jumpHeight && heightDifference >= minJumpHeight)
            {
                Debug.Log("Wanna jump");
                if (Mathf.Abs(pos1.x - pos2.x) <= speed * jumpTime && (GeneralEnemyBehaviour.LookingDirectlyAtPosition(pos1, pos2, consideredMasks) || GeneralEnemyBehaviour.LookingDirectlyAtPlayer(pos1, pos2, visionRange, consideredMasks, playerTag)))
                {
                    return true;
                }
            }
        }
        return false;
    }
    private void HandleGravity()
    {
        if (!onGround)
        {
            verticalSpeed -= gravity * Time.deltaTime;
        }
        else
        {
            verticalSpeed = 0f;
        }
        Vector2 currentPos = agent.transform.position;
        Vector2 nextPos = new Vector2(currentPos.x, currentPos.y + verticalSpeed * Time.deltaTime);
        if (IsPointOnNavMesh(nextPos))
        {
            agent.transform.position = nextPos;
        }
    }
    private void FollowPlayer()
    {
        Vector2 agentPos = agent.transform.position;
        Vector2 targetPos = target.position;
        if (GeneralEnemyBehaviour.LookingDirectlyAtPlayer(agentPos, targetPos, visionRange, consideredMasks, playerTag))
        {
            agent.stoppingDistance = stoppingDistance;
            Vector2 goalPosition = (Mathf.Abs(targetPos.y - agentPos.y) >= minJumpHeight) ? targetPos : new Vector2(targetPos.x, agentPos.y);
            if ((agentPos - targetPos).magnitude > stoppingDistance)
            {
                agent.SetDestination(goalPosition);
            }
            else
            {
                agent.SetDestination(agentPos);
            }
        }
    }
    private bool IsPointOnNavMesh(Vector2 point, float maxDistance = 1.0f)
    {
        NavMeshHit hit;
        return NavMesh.SamplePosition(point, out hit, maxDistance, NavMesh.AllAreas);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Vector2 startAbove = transform.position;
        Vector2 endAbove = startAbove + Vector2.up * jumpHeight;
        Gizmos.DrawLine(startAbove, endAbove);

        Vector2 startBelow = transform.position;
        Vector2 endBelow = startBelow - Vector2.up * jumpHeight;
        Gizmos.DrawLine(startBelow, endBelow);

        Gizmos.color = Color.blue;
        startAbove = transform.position;
        endAbove = startAbove + Vector2.up * minJumpHeight;
        Gizmos.DrawLine(startAbove, endAbove);

        float circleSegments = 36;
        float radius = visionRange;
        Vector2 center = transform.position;
        float angleStep = 360f / circleSegments;

        Vector2 prevPoint = center + new Vector2(Mathf.Cos(0), Mathf.Sin(0)) * radius;

        for (int i = 1; i <= circleSegments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector2 nextPoint = center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }
    }
}