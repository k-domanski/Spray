using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleShooting : MonoBehaviour
{
    #region Properties
    [SerializeField] private GameObject _shootPos;
    [SerializeField] private Rigidbody _bullet;
    [SerializeField] private float _force;
    #endregion

    #region Public
    public void Fire()
    {
        var bullet = Instantiate(_bullet, _shootPos.transform.position, Quaternion.identity);
        bullet.AddForce(_shootPos.transform.forward.normalized * _force, ForceMode.Impulse);
    }
    #endregion
}
