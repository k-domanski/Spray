using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/ShootingAction")]
public class ShootingAction : Action
{
    public override void Act(Enemy enemy)
    {
        enemy.transform.LookAt(enemy.target.transform);
        enemy.gunController.Shoot(enemy.transform.forward);
    }
}