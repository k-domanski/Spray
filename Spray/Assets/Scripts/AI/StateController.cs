using System;
using UnityEngine;

public class StateController : MonoBehaviour
{
    public State startState;
    public State currentState;
    private Enemy _enemy;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
    }

    private void Start()
    {
        currentState = startState;
    }

    private void Update()
    {
        currentState.UpdateState(_enemy);
    }
}