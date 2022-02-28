using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Stop")]
class StopAction : Action
{
    public override void Act(Enemy enemy)
    {
        enemy.SetDestination(enemy.transform.position);
        enemy.velocity = Vector3.MoveTowards(enemy.velocity, Vector3.zero, enemy.settings.acceleration * Time.deltaTime);
        enemy.Move(0f);
        //enemy.velocity = Vector3.Lerp(enemy.velocity,Vector3.zero, 0.2f);
    }

    public override void ActionEnd(Enemy enemy)
    {
        
    }

    public override void ActionStart(Enemy enemy)
    {
        
    }
}