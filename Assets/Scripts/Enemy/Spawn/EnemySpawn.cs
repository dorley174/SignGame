using System.Collections;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private Transform parentObject;
    [SerializeField]
    private Transform player;
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
            }
            else
            {
                enemyInstance = Instantiate(enemyPrefab, vectorPos, Quaternion.identity);
            }
            if (player != null)
            {
                if (enemyInstance.GetComponent<FlyingEnemyMovement>())
                {
                    enemyInstance.GetComponent<FlyingEnemyMovement>().Target = player;
                }
                else if (enemyInstance.GetComponent<LandEnemyMovement>())
                {
                    enemyInstance.GetComponent<LandEnemyMovement>().Target = player;
                }
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
}
