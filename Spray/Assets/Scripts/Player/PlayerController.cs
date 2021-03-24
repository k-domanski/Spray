using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour, MainControlls.IPlayerActions
{
    #region Properties
    [SerializeField] private LayerMask _mouseHitLayer;
    [SerializeField] private CameraController _cameraController;
    public Vector3 moveDirection { get; private set; } = Vector3.zero;
    public Vector3 aimDirection { get; private set; } = Vector3.forward;
    public float cameraRotationDirection { get; private set; } = 0.0f;
    #endregion

    #region Private
    private Player _player;
    private Camera _camera;
    private Vector2 _lastStickOffset = Vector3.forward;
    #endregion

    private void Awake()
    {
        if (InputManager.instance != null)
            InputManager.instance.SetCallbacks(this);
        else
            this.Delay(() => InputManager.instance.SetCallbacks(this), 0.001f);

        _player = GetComponent<Player>();
        _camera = Camera.main;
    }
    void Start()
    {
        InputManager.instance.SetCallbacks(this);
    }
    private void FixedUpdate()
    {

        _player.LookAt(AdjustToCamera(aimDirection), Time.fixedDeltaTime);
        _player.Move(AdjustToCamera(moveDirection), Time.fixedDeltaTime);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 dir = context.ReadValue<Vector2>();
        moveDirection = new Vector3(dir.x, 0f, dir.y);
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

        if (stickOffset.sqrMagnitude < 0.01)
        {
            stickOffset = _lastStickOffset;
        }
        else
        {
            _lastStickOffset = stickOffset;
        }
        aimDirection = new Vector3(stickOffset.x, 0f, stickOffset.y).normalized;
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
            aimDirection = dir.normalized;

#if UNITY_EDITOR
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * hit.distance, Color.red);
            Debug.DrawLine(transform.position, transform.position + aimDirection * delta.magnitude, Color.blue);
#endif
        }
    }

    public void OnCameraRotate(InputAction.CallbackContext context)
    {
        if (context.started)
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