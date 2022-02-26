using UnityEngine;

public abstract class Action : ScriptableObject
{
    public abstract void ActionStart(Enemy enemy);
    public abstract void Act(Enemy enemy);
    public abstract void ActionEnd(Enemy enemy);
}