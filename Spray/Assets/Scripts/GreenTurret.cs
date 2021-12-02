using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenTurret : MonoBehaviour
{
    [Header("Unity Setup Fields")]
    public Transform playerTarget;
    public string enemTag = "EnemyTarget";
    public GameObject enemyBullet;
    public Transform firePoint;
    public float rotationSpeed = 4f;

    [Header("Attributes")]
    public float AttackRange = 10.0f;
    public float fireRate = 2.0f;
    private float fireCoundown = 3.0f;

    private float distanceToPlayer;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateRangeToPlayer", 2.0f, 0.5f);
        playerTarget = null;
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
        GameObject[] enemyTarget = GameObject.FindGameObjectsWithTag(enemTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemyTarget = null;
        foreach(GameObject tar in enemyTarget)
        {
            distanceToPlayer = Vector3.Distance(transform.position, tar.transform.position);
            if(distanceToPlayer < shortestDistance)
            {
                shortestDistance = distanceToPlayer;
                nearestEnemyTarget = tar;
            }
        }
        if(nearestEnemyTarget != null && shortestDistance <= AttackRange)
        {
            playerTarget = nearestEnemyTarget.transform;
        }
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
