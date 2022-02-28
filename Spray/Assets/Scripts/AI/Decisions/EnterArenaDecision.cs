using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "AI/Decisions/EnteredArenaDecision")]
public class EnterArenaDecision : Decision
{
    public override bool Decide(Enemy enemy)
    {
        //HACK: for now its enough, change it
        if (Mathf.Abs(enemy.transform.position.z) < 22f & Mathf.Abs(enemy.transform.position.x) < 22f)
        {
            enemy.SetDestination(enemy.transform.position);
            return true;
        }

        return false;
    }
}