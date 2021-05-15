using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/ShootingAction")]
public class ShootingAction : Action
{
    public override void Act(Enemy enemy)
    {
        Vector3 pos = enemy.target.transform.position;
        var enemyPos = enemy.transform.position;
        pos.y = enemyPos.y;
        Transform transform;
        var time = Vector3.Distance(pos, enemyPos) / enemy.gunController.weaponStats.speed;
        (transform = enemy.transform).LookAt( enemy.settings.predictPosition ? pos + (enemy.target.velocity * time ): pos);
        enemy.gunController.Shoot(transform.forward, 1f + (enemy.velocity.magnitude / enemy.settings.maxSpeed));
    }
}