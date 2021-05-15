using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/StopAndShootDecision")]
public class StopAndShootDecision : Decision
{
    public override bool Decide(Enemy enemy)
    {
        return Vector3.Distance(enemy.target.transform.position, enemy.transform.position) < enemy.settings.stopAndShootDistance;
    }
}