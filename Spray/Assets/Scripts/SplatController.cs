using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatController : MonoBehaviour
{
    #region Properties
    [SerializeField] private GameObject _splatPrefab;
    [Header("Params")]
    [SerializeField] private Vector2 _sizeRange;
    #endregion

    #region Public
    public void PlaceSplat(Vector3 position, Vector3 facingDirection)
    {
        var instance = Instantiate(_splatPrefab, position, Quaternion.identity, transform);
        instance.transform.Rotate(Vector3.up, Random.Range(-Mathf.PI, Mathf.PI), Space.Self);
        var scale = Random.Range(_sizeRange.x, _sizeRange.y);
        instance.transform.localScale = new Vector3(scale, 1.0f, scale);
        instance.transform.up = facingDirection;
        instance.isStatic = true;
    }
    #endregion
}
