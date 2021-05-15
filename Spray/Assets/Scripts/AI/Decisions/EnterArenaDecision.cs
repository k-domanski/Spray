using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/EnteredArenaDecision")]
public class EnterArenaDecision : Decision
{
    public override bool Decide(Enemy enemy)
    {
        return enemy.transform.position.y < 2f; //HACK: for now its enough, change it
    }
}