using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Stop")]
class StopAction : Action
{
    public override void Act(Enemy enemy)
    {
        enemy.velocity = Vector3.MoveTowards(enemy.velocity, Vector3.zero, 5f * Time.deltaTime);
        //enemy.velocity = Vector3.Lerp(enemy.velocity,Vector3.zero, 0.2f);
    }
}