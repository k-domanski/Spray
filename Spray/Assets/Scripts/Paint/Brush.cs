using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Paint/Brush", fileName = "Brush")]
public class Brush : ScriptableObject
{
    [Header("Texture")]
    public Texture texture;

    [Header("Normals")]
    public Texture normalTexture;
    public bool randomNormals;
    public float normalStrength;

    [Header("Radius")]
    public bool randomRadius;
    public float radiusMin;
    public float radiusMax;

    [Header("Hardness")]
    public bool randomHardness;
    public float hardnessMin;
    public float hardnessMax;

    [Header("Strength")]
    public bool randomStrength;
    public float strengthMin;
    public float strengthMax;

    [Header("Rotation")]
    public bool randomRotation;
    public float rotationMin;
    public float rotationMax;


    public PaintData GetPaintData()
    {
        PaintData paintData = new PaintData();

        paintData.brush = texture;

        paintData.normal = normalTexture;
        paintData.randomNormals = randomNormals;
        paintData.normalStrength = normalStrength;

        paintData.radius = randomRadius ? Random.Range(radiusMin, radiusMax) : radiusMin;
        paintData.hardness = randomHardness ? Random.Range(hardnessMin, hardnessMax) : hardnessMin;
        paintData.strength = randomStrength ? Random.Range(strengthMin, strengthMax) : strengthMin;
        paintData.rotation = Mathf.Deg2Rad * (randomRotation ? Random.Range(rotationMin, rotationMax) : rotationMin);

        return paintData;
    }
}
