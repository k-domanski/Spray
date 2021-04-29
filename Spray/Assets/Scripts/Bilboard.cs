using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bilboard : MonoBehaviour
{
    [SerializeField] private Transform _cam;
    
    void LateUpdate()
    {
        transform.LookAt(transform.position + _cam.forward);
    }
}