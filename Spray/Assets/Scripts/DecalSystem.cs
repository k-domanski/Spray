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
    [SerializeField] private int _maxCapacity = 300;
    [SerializeField] private int _batchRemoveAmount = 20;
    [Header("Editor")]
    [SerializeField] private bool _decalLimit = true;
    #endregion

    #region Private
    private Queue<GameObject> _decals;
    private int _stored = 0;
    #endregion

    #region Messages
    void Awake()
    {
        _decals = new Queue<GameObject>(_maxCapacity);
    }
    #endregion

    #region Public
    public void PlaceSplat(Vector3 position, Vector3 facingDirection, Material material = null)
    {
        if (_decalLimit && _stored >= _maxCapacity)
        {
            FreeDecals(_batchRemoveAmount);
        }
        var instance = Instantiate(_splatPrefab, position, Quaternion.identity, transform);
        instance.transform.Rotate(Vector3.up, Random.Range(-Mathf.PI, Mathf.PI), Space.Self);
        var scale = Random.Range(_sizeRange.x, _sizeRange.y);
        instance.transform.localScale = new Vector3(scale, 1.0f, scale);
        instance.transform.position -= facingDirection * 0.1f;
        var pos = instance.transform.position;
        // print($"Instance: [{pos.x}, {pos.y}, {pos.z}]");
        instance.transform.up = facingDirection;
        instance.isStatic = true;

        if (material != null)
        {
            var renderer = instance.GetComponent<Renderer>();
            renderer.material = material;
        }

        // if(_decalLimit)
        // {        
        //     _decals.Enqueue(instance.gameObject);
        //     ++_stored;
        // }
    }
    public void PlaceBulletHole(Vector3 position, Vector3 facingDirection)
    {
        if (_decalLimit && _stored >= _maxCapacity)
        {
            FreeDecals(_batchRemoveAmount);
        }

        var instance = Instantiate(_bulletHolePrefab, position, Quaternion.identity, transform);
        instance.transform.Rotate(Vector3.up, Random.Range(-Mathf.PI, Mathf.PI), Space.Self);
        var scale = Random.Range(0.06f, 0.01f);
        instance.transform.localScale = new Vector3(scale, 1.0f, scale);
        instance.transform.up = facingDirection;
        instance.transform.position += facingDirection * 0.01f;
        instance.isStatic = true;
        if (_decalLimit)
        {
            _decals.Enqueue(instance.gameObject);
            ++_stored;
        }
    }

    public void ClearDecals()
    {
        foreach (var child in gameObject.GetComponentsInChildren<MeshRenderer>())
        {
            Destroy(child.gameObject);
        }
        _decals.Clear();
    }
    #endregion

    #region Private
    private void FreeDecals(int amount)
    {
        for (int i = 0; i < _batchRemoveAmount; ++i)
        {
            Destroy(_decals.Dequeue());
        }
        _stored -= amount;
    }
    #endregion
}
