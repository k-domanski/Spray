using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/OutOfMeleeAttackDecision")]
public class OutOfMeleeAttackDecision : Decision
{
    public override bool Decide(Enemy enemy)
    {
        return !enemy.targetInMeleeRange && enemy.canAttack;
    }
}