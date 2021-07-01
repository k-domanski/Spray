using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleUI : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;

    [ContextMenu("Open")]
    public void Awake()
    {
        Close();
    }
    public void Open()
    {
        _canvas.enabled = true;
    }

    [ContextMenu("Close")]
    public void Close()
    {
        _canvas.enabled = false;
    }
}
