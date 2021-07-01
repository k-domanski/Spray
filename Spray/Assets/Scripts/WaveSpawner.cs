using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState
    {
        SPAWNING,
        WAITING,
        COUNTING
    };
    [System.Serializable]
    public class Wave
    {
        public string name;
        public EnemyWithColor[] enemies;
        public int count;
        public float rate;
        public int totalEnemiesSpawned { get => _totalEnemiesSpawned; set { _totalEnemiesSpawned = value; } }

        private int _totalEnemiesSpawned = 0;

        public void ResetWave()
        {
            _totalEnemiesSpawned = 0;
            foreach (var enemy in enemies)
                enemy.currentEnemyCount = 0;
        }
    }
    [System.Serializable]
    public class EnemyWithColor
    {
        public Enemy enemy;
        public int count;
        public int currentEnemyCount { get => _currentEnemyCount; set { _currentEnemyCount = value; } }
        public bool canSpawn => currentEnemyCount < count;
        private int _currentEnemyCount = 0;
    }
    public float waveMultiplier = 1.0f;
    public Wave[] waves;
    private int _nextWave = 0;
    
    //waveNumber fields
    private int waveNumber = 1;
    private WaveNumber _waveNumber;

    public Transform[] spawnPoints;

    public float timeBetweenWaves = 4f;
    public float waveCountdown;

    private float searchCountdown = 1f;

    private SpawnState state = SpawnState.COUNTING;


    void Start()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points referenced");
        }
        waveCountdown = timeBetweenWaves;
        _waveNumber = GameObject.FindObjectOfType<WaveNumber>();
        _waveNumber.updateWaveNumber(waveNumber);
    }

    void Update()
    {
        if (state == SpawnState.WAITING)
        {

            if (!EnemyIsAlive())
            {
                waveNumber++;
                _waveNumber.updateWaveNumber(waveNumber);

                WaveCompleted();
            }
            else
            {
                return;
            }
        }

        if (waveCountdown <= 0)
        {
            if (state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWave(waves[_nextWave]));
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    void WaveCompleted()
    {
        Debug.Log("Wave completed");
        
        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;
        int lastWaveIndex = waves.Length - 1;

        if (_nextWave + 1 > lastWaveIndex)
        {
            //waveMultiplier += 1.5f;
            waves[lastWaveIndex].ResetWave();
            _nextWave = lastWaveIndex;
            Debug.Log("Completed all waves! Looping...");
        }
        else
        {
            _nextWave++;
        }
    }
    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            if (Systems.aiManager.enemies.Count == 0)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        Debug.Log("Spawning wave: " + _wave.name);
        state = SpawnState.SPAWNING;

        do
        {
            var index = (int)Random.Range(0, _wave.enemies.Length);
            Debug.Log(index);
            if (_wave.enemies[index].canSpawn)
            {
                SpawnEnemy(_wave.enemies[index]);
                _wave.totalEnemiesSpawned++;
                yield return new WaitForSeconds(1f / _wave.rate);

            }
        } while (_wave.totalEnemiesSpawned < _wave.count * waveMultiplier);

        state = SpawnState.WAITING;
        yield break;
    }

    void SpawnEnemy(EnemyWithColor _enemy)
    {
        Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        var createdEnemy = Instantiate(_enemy.enemy, _sp.position + new Vector3(Random.Range(-2f, 2f), 0.0f, Random.Range(-2f, 2f)), _sp.rotation);
        Systems.aiManager.enemies.Add(createdEnemy);
        _enemy.currentEnemyCount++;
    }
}
