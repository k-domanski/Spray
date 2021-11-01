using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MousePainter : MonoBehaviour
{
    public bool toggle = false;
    new private Camera camera;
    public PaintManager paintManager;
    private bool pressed = false;
    void Awake()
    {
        camera = GetComponent<Camera>();
    }

    void Update()
    {
        if (Mouse.current["leftButton"].IsPressed())
        {
            if(pressed && toggle)
            {
                return;
            }
            var ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out var hit, 100000))
            {
                float rotation = Random.Range(-Mathf.PI, Mathf.PI);
                paintManager.Paint(hit.transform.GetComponent<Paintable>(), hit.point, 1.0f, 0.5f, 1.0f, rotation);
            }
            pressed = true;
        }
        else
        {
            pressed = false;
        }
    }
}
