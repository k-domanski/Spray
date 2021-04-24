using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/ShootingAction")]
public class ShootingAction : Action
{
    public override void Act(Enemy enemy)
    {
        Vector3 pos = enemy.target.transform.position;
        pos.y = enemy.transform.position.y;
        enemy.transform.LookAt(pos);
        enemy.gunController.Shoot(enemy.transform.forward, 1f + (enemy.velocity.magnitude / enemy.settings.maxSpeed));
    }
}