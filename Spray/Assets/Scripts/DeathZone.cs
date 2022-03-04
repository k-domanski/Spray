using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        var le = other.GetComponent<LivingEntity>();
        le?.Kill();
    }

    void OnCollisionEnter(Collision collision)
    {
        var le = collision.transform.GetComponent<LivingEntity>();
        le?.Kill();
    }
}
