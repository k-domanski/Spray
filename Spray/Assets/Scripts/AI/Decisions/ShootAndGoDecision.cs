using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/ShootAndGoDecision")]
public class ShootAndGoDecision : Decision
{
    public override bool Decide(Enemy enemy)
    {
        return Vector3.Distance(enemy.target.transform.position, enemy.transform.position) > enemy.settings.stopAndShootDistance;
    }
}