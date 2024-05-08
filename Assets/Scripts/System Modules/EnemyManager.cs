using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    public GameObject RandomEnemy => enemyList.Count == 0 ? null : enemyList[Random.Range(0, enemyList.Count)];
    public int WaveNumber => waveNumber;//属性 等同于get
    public float TimeBetweenWaves => timeBetweenWaves;//属性
    [SerializeField] bool isSpawnEnemy = true;
    [SerializeField] GameObject waveUI;
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] float timeBetweenSpawns = 1f;//敌人生成间隔
    [SerializeField] float timeBetweenWaves = 1f;//每波敌人生成前的等待时间
    [SerializeField] int minEnemyAmount = 4;
    [SerializeField] int maxEnemyAmount = 10;

    [Header("---- Boss ----")]
    [SerializeField] GameObject bossPrefab;
    [SerializeField] int bossWaveNumber;


    int waveNumber = 1;
    int enemyAmount;
    WaitForSeconds waitTimeBetweenSpawns;
    WaitForSeconds waitTimeBetweenWaves;
    WaitUntil waitUntilNoEnemy;

    List<GameObject> enemyList;
    protected override void Awake()
    {
        base.Awake();
        enemyList = new List<GameObject>();
        waitTimeBetweenSpawns = new WaitForSeconds(timeBetweenSpawns);
        waitTimeBetweenWaves = new WaitForSeconds(timeBetweenWaves);
        waitUntilNoEnemy = new WaitUntil(() => enemyList.Count == 0);
    }

    IEnumerator Start()
    {
        while (isSpawnEnemy && GameManager.GameState != GameState.GameOver)
        {
            waveUI.SetActive(true);
            yield return waitTimeBetweenWaves;
            waveUI.SetActive(false);
            yield return StartCoroutine(RandomRespawnCoroutine());
        }
    }
    IEnumerator RandomRespawnCoroutine()
    {
        if (waveNumber % bossWaveNumber == 0)
        {
            var boss = PoolManager.Release(bossPrefab);
            enemyList.Add(boss);
        }
        else
        {
            enemyAmount = Mathf.Clamp(enemyAmount, minEnemyAmount + waveNumber / bossWaveNumber, maxEnemyAmount);
            for (int i = 0; i < enemyAmount; i++)
            {
                enemyList.Add(PoolManager.Release(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]));

                yield return waitTimeBetweenSpawns;
            }
        }
        yield return waitUntilNoEnemy;
        waveNumber++;
    }

    public void RemoveFromList(GameObject enemy) => enemyList.Remove(enemy);
}
