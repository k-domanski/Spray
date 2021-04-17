using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Animation/Animation Playback Timing", fileName = "Animation Playback Timing")]
public class AnimationPlaybackSpeed : ScriptableObject
{
    #region Properties
    [SerializeField] private float _baseMovementSpeed;
    [SerializeField] private float _baseAnimationSpeed;
    public float baseMovementSpeed => _baseMovementSpeed;
    public float baseAnimationSpeed => _baseAnimationSpeed;
    #endregion

    #region Public
    public float GetPlaybackSpeed(float movementSpeed)
    {
        return _baseAnimationSpeed * movementSpeed / _baseMovementSpeed;
    }
    #endregion
}
