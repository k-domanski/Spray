using System;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public List<Enemy> enemies { get; private set; } = new List<Enemy>();

    private void Start()
    {
        enemies.AddRange(FindObjectsOfType<Enemy>());
    }

    public void Clear()
    {
        enemies.Clear();
    }

    public Enemy GetNeighbour(Enemy enemy)
    {
        Vector3 velocity = enemy.velocity.normalized;

        Enemy neighbour = null;

        velocity *= enemy.settings.maxQueueAhead;

        var ahead = enemy.transform.position + velocity;

        foreach (var enemyNeighbour in enemies)
        {
            if(enemyNeighbour == enemy)
                continue;
            var distance = Vector3.Distance(ahead, enemyNeighbour.transform.position);

            if (distance < enemy.settings.maxQueueRadius)
            {
                neighbour = enemyNeighbour;
                break;
            }
        }

        return neighbour;
    }
}