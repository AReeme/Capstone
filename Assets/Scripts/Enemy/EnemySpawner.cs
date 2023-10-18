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

    private int goppleCount = 0;
    private int goopleCount = 0;
    private int hammerHeadCount = 0;

    private float goppleTimer = 3.5f;
    private float goopleTimer = 5.5f;
    private float hammerHeadTimer = 10.0f;

    private void Start()
    {
        StartCoroutine(SpawnEnemy(goppleTimer, gopplePrefab, goppleCount));
        StartCoroutine(SpawnEnemy(goopleTimer, gooplePrefab, goopleCount));
        StartCoroutine(SpawnEnemy(hammerHeadTimer, hammerHeadPrefab, hammerHeadCount));
    }

    private IEnumerator SpawnEnemy(float interval, GameObject enemy, int enemyCount)
    {
        yield return new WaitForSeconds(interval);

        if (enemy == gopplePrefab)
        {
            goppleTimer -= 0.05f;
            interval = goppleTimer;
            if (interval < 1)
            {
                interval = 1;
            }
            goppleCount++;
        }
        else if (enemy == gooplePrefab)
        {
            goopleTimer -= 0.05f;
            interval = goopleTimer;
            if (interval < 1)
            {
                interval = 1;
            }
            goopleCount++;
        }
        else if (enemy == hammerHeadPrefab)
        {
            hammerHeadTimer -= 0.05f;
            interval = hammerHeadTimer;
            if (interval < 1)
            {
                interval = 1;
            }
            hammerHeadCount++;
        }

        GameObject newEnemy = Instantiate(enemy, new Vector3(Random.Range(-81.0f, 89.0f), Random.Range(-7.5f, 75.5f), -3), Quaternion.identity);
        StartCoroutine(SpawnEnemy(interval, enemy, enemyCount));
    }
}