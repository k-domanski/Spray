using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "AI/Decisions/EnteredArenaDecision")]
public class EnterArenaDecision : Decision
{
    public override bool Decide(Enemy enemy)
    {
        //HACK: for now its enough, change it
        if (enemy.transform.position.y < 2f)
        {
            enemy.SetDestination(enemy.transform.position);
            return true;
        }

        return false;
    }
}