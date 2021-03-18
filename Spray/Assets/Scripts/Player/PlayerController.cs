using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour, MainControlls.IPlayerActions
{
    public Vector3 moveDirection { get; private set; } = Vector3.zero;
    public Vector3 aimDirection { get; private set; } = Vector3.zero;

    private Player _player;

    private void Awake()
    {
        if (InputManager.instance != null)
            InputManager.instance.SetCallbacks(this);
        else
            this.Delay(() => InputManager.instance.SetCallbacks(this), 0.001f);

        _player = GetComponent<Player>();
    }

    private void FixedUpdate()
    {
        _player.LookAt(aimDirection, Time.fixedDeltaTime);
        _player.Move(moveDirection, Time.fixedDeltaTime);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 dir = context.ReadValue<Vector2>();
        moveDirection = new Vector3(dir.x, 0f, dir.y);
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        //might be invoke two times on start
        _player.Shoot(context.ReadValueAsButton());
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        Vector2 dir = context.ReadValue<Vector2>();

        aimDirection = new Vector3(dir.x, 0f, dir.y).normalized;
    }

    public void OnMouse(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = context.ReadValue<Vector2>();
        Vector2 playerPosition = Camera.main.WorldToScreenPoint(_player.transform.position);

        Vector2 dir = (mousePosition - playerPosition).normalized;

        aimDirection = new Vector3(dir.x, 0f, dir.y);

    }


}