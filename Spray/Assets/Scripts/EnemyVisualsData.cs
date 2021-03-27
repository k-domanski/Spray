using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Enemy Visuals", fileName="Enemy Visuals")]
public class EnemyVisualsData : ScriptableObject
{
    #region Properties
    [SerializeField] private Color _baseColor;
    [SerializeField] private Material _particlesMaterial;
    [SerializeField] private Material _splatMaterial;

    public Color baseColor => _baseColor;
    public Material particlesMaterial => _particlesMaterial;
    public Material splatMaterial => _splatMaterial;
    #endregion
}
