using System;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour, MainControlls.IPlayerActions
{
    #region Properties
    [SerializeField] private LayerMask _mouseHitLayer;
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private GameObject _gunPoint;
    [SerializeField] private GameObject _secondGunPoint;
    public Vector3 moveDirection { get; private set; } = Vector3.zero;
    public Vector3 aimDirection { get; private set; } = Vector3.forward;
    public Vector3 lookDirection => transform.forward;
    public Vector3 fireDirection => _fireDirection;
    public float cameraRotationDirection { get; private set; } = 0.0f;
    #endregion

    #region Private
    private Player _player;
    private Camera _camera;
    private Vector3 _fireDirection;
    private Vector2 _lastStickOffset = Vector3.forward;
    private Vector3 _aimOffset = Vector3.forward;
    private Vector3 _moveOffset = Vector3.zero;
    private bool _shootOnAim;
    #endregion

    private void Awake()
    {
        Systems.inputManager.SetCallbacks(this);
        _player = GetComponent<Player>();
        _camera = Camera.main;
    }
    private void FixedUpdate()
    {
        aimDirection = AdjustToCamera(_aimOffset);
        moveDirection = AdjustToCamera(_moveOffset);
        _player.Move(moveDirection, Time.deltaTime);
        _player.LookAt(aimDirection, Time.deltaTime);
        _fireDirection = transform.forward;
        _fireDirection.y = 0;
        _fireDirection.Normalize();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 dir = context.ReadValue<Vector2>();
        _moveOffset = new Vector3(dir.x, 0f, dir.y);
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        _player.Shoot(!context.canceled);

        //might be invoke two times on start
        //_player.Shoot(context.ReadValueAsButton());
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        var stickOffset = context.ReadValue<Vector2>();

        if (stickOffset.sqrMagnitude < _player.playerSettings.controllerAimDeadZone)
        {
            stickOffset = _lastStickOffset;
            _shootOnAim = false;
        }
        else
        {
            _lastStickOffset = stickOffset;
            _shootOnAim = true;
        }
        _aimOffset = new Vector3(stickOffset.x, 0f, stickOffset.y).normalized;
        if (_player.playerSettings.autoShoot)
        {
            this.Delay(() => _player.Shoot(_shootOnAim), 0.1f);
        }
    }

    public void OnMouse(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = context.ReadValue<Vector2>();
        var ray = _camera.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000.0f, _mouseHitLayer))
        {
            var delta = hit.point - transform.position;

            var m = _camera.transform.worldToLocalMatrix;
            Vector3 dir = m * delta.normalized;
            // aimDirection = dir.normalized;
            _aimOffset = dir.normalized;

#if UNITY_EDITOR
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * hit.distance, Color.red);
            Debug.DrawLine(transform.position, transform.position + aimDirection * delta.magnitude, Color.blue);
#endif
        }
    }

    public void OnCameraRotate(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            cameraRotationDirection = context.ReadValue<float>();
            _cameraController.RotateCamera(cameraRotationDirection);
        }
        else if (context.canceled)
        {
            cameraRotationDirection = 0.0f;
            _cameraController.RotateCamera(cameraRotationDirection);
        }
    }

    public void OnWeaponSwitch(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<float>();
        if (value != 0)
        {
            _player.ChangeWeapon();
        }
    }
    #region Private Methods
    private Vector3 AdjustToCamera(Vector3 dir)
    {
        Vector3 direction = AdjustToCameraMatrix() * dir;
        direction.y = 0;
        return direction;
    }
    private Matrix4x4 AdjustToCameraMatrix()
    {
        var right = _camera.transform.right;
        var up = _camera.transform.up;
        var forward = _camera.transform.forward;

        return new Matrix4x4(right, up, forward, Vector3.zero);
    }

    #endregion
}