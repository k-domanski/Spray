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
        public Enemy[] enemy;
        public int count;
        public float rate;
    }

    public float waveMultiplier = 1.0f;
    public Wave[] waves;
    private int _nextWave = 0;
    
    //waveNumber fields
    public int waveNumber = 0;
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

        if (_nextWave + 1 > waves.Length - 1)
        {
            //waveMultiplier += 1.5f;
            _nextWave = 0;
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

        for (int i = 0; i < _wave.count * waveMultiplier; i++)
        {
            var index = (int)Random.Range(0, _wave.enemy.Length);
            SpawnEnemy(_wave.enemy[index]);
            yield return new WaitForSeconds(1f / _wave.rate);
        }
        
        state = SpawnState.WAITING;
        yield break;
    }

    void SpawnEnemy(Enemy _enemy)
    {
        //Spawn enemy
        //Debug.Log(("spawning enemy: " + _enemy.name));
        Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        var createdEnemy = Instantiate(_enemy, _sp.position + new Vector3(Random.Range(-2f,2f), 0.0f, Random.Range(-2f, 2f)), _sp.rotation);
        Systems.aiManager.enemies.Add(createdEnemy);
    }
}
