using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject[] enemyList;
    [SerializeField] private int maxEnemyCount = 10;
    [SerializeField] private float distance = 50;
    [SerializeField] private float spawnDelay = 5;
    [SerializeField] private float spawnDelayCounter;
    // Start is called before the first frame update
    void Start()
    {
        spawnDelayCounter = spawnDelay;
    }

    // Update is called once per frame
    void Update()
    {
        enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        if(enemyList.Length < maxEnemyCount && spawnDelayCounter < 0)
        {
            spawnDelayCounter = spawnDelay;
            Instantiate(prefab, new Vector3(transform.position.x + Random.Range(-distance, distance), transform.position.y,transform.position.z), Quaternion.identity);
        }
        else
        {
            spawnDelayCounter -= Time.deltaTime;
        }
    }
}
