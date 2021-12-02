using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Paint/Colors Library", fileName = "Colors Library")]
public class ColorsLib : ScriptableObject
{
    public List<Pair<ColorType, Color>> colors;

    public Color GetColor(ColorType type)
    {
        foreach (var pair in colors)
        {
            if (pair.first == type)
            {
                return pair.second;
            }
        }

        return new Color(255,255,255);
    }
}

[System.Serializable]
public class Pair<T1, T2>
{
    public T1 first;
    public T2 second;
}