using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _spawner;
    [SerializeField] private bool _stopSpawning = false;
    [SerializeField] private float _spawnTime;
    [SerializeField] private float _spawnDelay;
    [SerializeField] private float _timeOfSpawning;
    private float _time = 0.0f;
    
    private void Start()
    {
        InvokeRepeating("SpawnObject", _spawnTime, _spawnDelay);
    }

    private void Update()
    {
        _time += Time.deltaTime;
    }

    private void SpawnObject()
    {
        Instantiate(_spawner, transform.position, transform.rotation);
        if (_stopSpawning)
        {
            CancelInvoke("SpawnObject");
        }
        else if (_timeOfSpawning < _time)
        {
            CancelInvoke("SpawnObject");
        }
    }
}
