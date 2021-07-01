using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EHand
{
    Left = 0,
    Right = 1
}

public class MeleeAnimation : MonoBehaviour
{
    [Header("Params")]
    [SerializeField] private EnemyParams _params;

    [Header("Hands")]
    [SerializeField] private GameObject _leftHand;
    [SerializeField] private GameObject _rightHand;
    private Quaternion _qLBase, _qLInter;
    private Quaternion _qRBase, _qRInter;
    private Quaternion[] _lAngles =
        {
            Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f)),
            Quaternion.Euler(new Vector3(0.0f, 0.0f, -90.0f)),
            Quaternion.Euler(new Vector3(0.0f, 115.0f, -90.0f)),
            Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f))
        };
    private Quaternion[] _rAngles =
        {
            Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f)),
            Quaternion.Euler(new Vector3(0.0f, 0.0f, 90.0f)),
            Quaternion.Euler(new Vector3(0.0f, -115.0f, 90.0f)),
            Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f))
        };
    private EnemyArm[] _armScript = new EnemyArm[2];
    private EHand _currentHand = EHand.Left;
    private int _phaseIndex = 1;
    private Coroutine[] _coroutine = new Coroutine[2];
    private Transform[] _handTransform = new Transform[2];
    private float[] _duration;
    public bool isAnimating { get; private set; } = false;
    void Awake()
    {
        /* Hands */
        _qLBase = _leftHand.transform.localRotation;
        _qRBase = _rightHand.transform.localRotation;
        _qLInter = Quaternion.identity;
        _qRInter = Quaternion.identity;
        _handTransform[0] = _leftHand.transform;
        _handTransform[1] = _rightHand.transform;

        _duration = new float[]
        {
            _params.attackStartTime,
            _params.attackSwingTime,
            _params.attackEndTime
        };

        _armScript[0] = _leftHand.GetComponentInChildren<EnemyArm>();
        _armScript[1] = _rightHand.GetComponentInChildren<EnemyArm>();

        foreach(var arm in _armScript)
        {
            arm.damage = _params.attackDamage;
        }
    }

    [ContextMenu("Melee Attack")]
    public void MeleeAttack()
    {
        if (isAnimating)
        {
            return;
        }

        /* Swap hands */
        var side = _currentHand == EHand.Left ? EHand.Right : EHand.Left;
        var hand = side == EHand.Left ? _leftHand : _rightHand;
        _currentHand = side;

        isAnimating = true;
        NextAnimationPhase();
    }

    private IEnumerator AnimateAttack(Transform hand, Quaternion start, Quaternion end, float duration)
    {
        float time = 0;
        while (time <= duration)
        {
            var t = time / duration;
            hand.localRotation = Quaternion.Slerp(start, end, t);
            time += Time.deltaTime;
            yield return null;
        }
        hand.localRotation = end;
        /* 'Ping' animation end */
        NextAnimationPhase();
    }

    private void NextAnimationPhase()
    {
        var index = (int)_currentHand;
        if (_coroutine[index] != null)
        {
            StopCoroutine(_coroutine[index]);
        }
        var angles = _currentHand == EHand.Right ? _rAngles : _lAngles;
        if (_phaseIndex >= angles.Length)
        {
            _phaseIndex = 1;
            isAnimating = false;
            return;
        }

        /* Enable damage dealing */
        _armScript[index].canDealDamage = (_phaseIndex == 2);

        var start = angles[_phaseIndex - 1];
        var end = angles[_phaseIndex];
        var duration = _duration[_phaseIndex - 1];
        _coroutine[index] = StartCoroutine(AnimateAttack(_handTransform[index],
                                                         start,
                                                         end,
                                                         duration));
        _phaseIndex += 1;
    }
}
