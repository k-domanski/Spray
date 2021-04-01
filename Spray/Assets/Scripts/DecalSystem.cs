using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalSystem : MonoBehaviour
{
    #region Properties
    [SerializeField] private GameObject _splatPrefab;
    [SerializeField] private GameObject _bulletHolePrefab;
    [Header("Params")]
    [SerializeField] private Vector2 _sizeRange;
    #endregion

    #region Public
    public void PlaceSplat(Vector3 position, Vector3 facingDirection, Material material = null)
    {
        var instance = Instantiate(_splatPrefab, position, Quaternion.identity, transform);
        instance.transform.Rotate(Vector3.up, Random.Range(-Mathf.PI, Mathf.PI), Space.Self);
        var scale = Random.Range(_sizeRange.x, _sizeRange.y);
        instance.transform.localScale = new Vector3(scale, 1.0f, scale);
        instance.transform.up = facingDirection;
        instance.isStatic = true;

        if(material!= null)
        {
            var renderer = instance.GetComponent<Renderer>();
            renderer.material = material;
        }
    }
    public void PlaceBulletHole(Vector3 position, Vector3 facingDirection)
    {
        // var rotation = Quaternion.AngleAxis(Random.Range(-180.0f, 180.0f), Vector3.up);
        var instance = Instantiate(_bulletHolePrefab, position, Quaternion.identity, transform);
        instance.transform.Rotate(Vector3.up, Random.Range(-Mathf.PI, Mathf.PI), Space.Self);
        var scale = Random.Range(0.06f, 0.01f);
        instance.transform.localScale = new Vector3(scale, 1.0f, scale);
        instance.transform.up = facingDirection;
        instance.transform.position += facingDirection * 0.01f;
        instance.isStatic = true;
    }
    #endregion
}
