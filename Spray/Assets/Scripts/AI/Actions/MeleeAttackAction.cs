using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/MeleeAttackAction")]
public class MeleeAttackAction : Action
{
    public override void Act(Enemy enemy)
    {
        if (enemy.canAttack && enemy.targetInMeleeRange)
        {
            enemy.canAttack = false;
            enemy.Delay(() => Attack(enemy), enemy.settings.preAttackTime);
            enemy.ShowAttack();
        }
    }

    private void Attack(Enemy enemy)
    {
        var target = enemy.target;
        var targetDir = target.transform.position - enemy.transform.position;

        //Debug.Log(Vector3.SignedAngle(enemy.transform.forward, targetDir, Vector3.up));

        if (Mathf.Abs(Vector3.SignedAngle(enemy.transform.forward, targetDir, Vector3.up)) >
              (enemy.settings.attackAngle / 2f)) return;
        //Debug.Log("distance: " + targetDir.magnitude);

        if (targetDir.magnitude > enemy.settings.attackRange) return;
        //Debug.Log("in range");
        var livingEntity = target.GetComponent<LivingEntity>();
        livingEntity.DealDamage(enemy.settings.attackDamage, Vector3.zero);
    }
}