using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Paint/Colors Library", fileName = "Colors Library")]
public class ColorsLib : ScriptableObject
{
    public List<Pair<ColorType, Color>> colors;
}

[System.Serializable]
public class Pair<T1, T2>
{
    public T1 firts;
    public T2 second;
}