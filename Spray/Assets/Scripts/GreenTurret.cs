using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenTurret : MonoBehaviour
{
    [Header("Unity Setup Fields")]
    public Transform playerTarget;
    public GameObject enemyBullet;
    public Transform firePoint;
    public float rotationSpeed = 4f;

    [Header("Attributes")]
    public float AttackRange = 10.0f;
    public float fireRate = 1.0f;
    private float fireCoundown = 3.0f;

    private float distanceToPlayer;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateRangeToPlayer", 2.0f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTarget == null)
            return;

        if (distanceToPlayer <= AttackRange)
        {
            Vector3 direction = playerTarget.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed).eulerAngles;
            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
            if (fireCoundown <= 0.0f)
            {
                Shoot();
                fireCoundown = 1.0f / fireRate;
            }
        }

        fireCoundown -= Time.deltaTime;
    }

    void UpdateRangeToPlayer()
    {
        distanceToPlayer = Vector3.Distance(transform.position, playerTarget.position);
    }

    void Shoot()
    {
        GameObject bulletGO = (GameObject)Instantiate(enemyBullet, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.Seek(playerTarget);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }
}
