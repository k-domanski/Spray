using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "AI/Decisions/EnteredArenaDecision")]
public class EnterArenaDecision : Decision
{
    public override bool Decide(Enemy enemy)
    {
        //HACK: for now its enough, change it
        if (Mathf.Abs(enemy.transform.position.z) < 50f & Mathf.Abs(enemy.transform.position.x) < 50f)
        {
            enemy.SetDestination(enemy.transform.position);
            return true;
        }

        return false;
    }
}