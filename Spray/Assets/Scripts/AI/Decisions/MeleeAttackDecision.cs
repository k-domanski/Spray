using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/MeleeAttackDecision")]
public class MeleeAttackDecision : Decision
{
    public override bool Decide(Enemy enemy)
    {
        return enemy.targetInMeleeRange;
    }
}