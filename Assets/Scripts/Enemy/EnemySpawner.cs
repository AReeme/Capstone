using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject gopplePrefab;
    [SerializeField]
    private GameObject gooplePrefab;
    [SerializeField]
    private GameObject hammerHeadPrefab;

    public Transform spawnPointsParent; // Reference to the parent GameObject containing spawn points.

    private int goppleCount = 0;
    private int goopleCount = 0;
    private int hammerHeadCount = 0;

    private float goppleTimer = 3.5f;
    private float goopleTimer = 5.5f;
    private float hammerHeadTimer = 10.0f;

    private void Start()
    {
        // Start spawning the different types of enemies
        StartCoroutine(SpawnEnemy(goppleTimer, gopplePrefab, goppleCount));
        StartCoroutine(SpawnEnemy(goopleTimer, gooplePrefab, goopleCount));
        StartCoroutine(SpawnEnemy(hammerHeadTimer, hammerHeadPrefab, hammerHeadCount));
    }

    private IEnumerator SpawnEnemy(float interval, GameObject enemy, int enemyCount)
    {
        yield return new WaitForSeconds(interval);

        interval -= 0.25f;
        interval = Mathf.Max(interval, 0.25f);

        if (enemy == gopplePrefab)
        {
            goppleCount++;
        }
        else if (enemy == gooplePrefab)
        {
            goopleCount++;
        }
        else if (enemy == hammerHeadPrefab)
        {
            hammerHeadCount++;
        }

        Transform[] spawnPoints = spawnPointsParent.GetComponentsInChildren<Transform>();
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject newEnemy = Instantiate(enemy, randomSpawnPoint.position, Quaternion.identity);
        StartCoroutine(SpawnEnemy(interval, enemy, enemyCount));
    }
}