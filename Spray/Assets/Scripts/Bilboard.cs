using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bilboard : MonoBehaviour
{
    [SerializeField] private Transform _cam;
    
    void LateUpdate()
    {
        transform.LookAt(transform.position + _cam.forward);
    }
}
