using System.Collections;
using Unity.Services.Analytics;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private Transform parentObject;
    private GameObject enemyInstance;
    [SerializeField]
    private float respawnTime;
    [SerializeField]
    private TimeUnit respawnTimeUnit = TimeUnit.Seconds;
    private enum TimeUnit
    {
        Milliseconds = 1,
        Seconds = 1000,
        Minutes = 60000
    }
    private float TimeInSeconds => (respawnTime * (int)respawnTimeUnit) / 1000f;
    [SerializeField]
    private Transform spawnPos;
    private Vector3 vectorPos;
    private void OnValidate()
    {
        if (spawnPos == null)
        {
            vectorPos = transform.position;
        }
        else
        {
            vectorPos = spawnPos.position;
        }
    }
    private void Start()
    {
        SpawnEnemy();
    }
    public void SpawnEnemy()
    {
        if (enemyInstance == null)
        {
            if (parentObject != null)
            {
                enemyInstance = Instantiate(enemyPrefab, vectorPos, Quaternion.identity, parentObject);
                StartCoroutine(WaitToInit(enemyInstance, Quaternion.identity));
            }
            else
            {
                enemyInstance = Instantiate(enemyPrefab, vectorPos, Quaternion.identity);
                StartCoroutine(WaitToInit(enemyInstance, Quaternion.identity));
            }
            if (enemyInstance.GetComponent<Enemy>())
            {
                enemyInstance.GetComponent<Enemy>().Spawn = this;
            }
        }
    }
    public void DeleteEnemy()
    {
        if (enemyInstance != null)
        {
            Destroy(enemyInstance);
            enemyInstance = null;
        }
        StartCoroutine(SpawnAfterSeconds(TimeInSeconds));
    }
    IEnumerator SpawnAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SpawnEnemy();
    }
    IEnumerator WaitToInit(GameObject obj, Quaternion rotation)
    {
        yield return new WaitForEndOfFrame();
        obj.transform.rotation = rotation;
    }
    //void Start()
    //{
    //    StartCoroutine(InitAgent());
    //}
    //IEnumerator InitAgent()
    //{
    //    yield return new WaitForEndOfFrame(); // Подождать 1 кадр

    //    if (!agent.isOnNavMesh)
    //    {
    //        Debug.LogWarning("NavMeshAgent не на NavMesh!");
    //    }
    //    playerTag = target.gameObject.tag;
    //    SetAgentParameters();
    //}
}
