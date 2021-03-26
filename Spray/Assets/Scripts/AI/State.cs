using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/State")]
public class State : ScriptableObject
{
    public List<Action> actions = new List<Action>();
    public List<Transition> transitions = new List<Transition>();
    public void UpdateState(Enemy enemy)
    {
        actions.ForEach(x => x.Act(enemy));
        foreach (var transition in transitions)
        {
            if (transition.decision.Decide(enemy))
            {
                enemy.stateController.currentState = transition.nextState;
            }
        }
    }
}