using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DontDestroyBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    #region Static
    static private T _instance = null;
    static public T instance
    {
        get => _instance;
        private set => _instance = value;
    }
    #endregion

    #region Messages
    void Awake()
    {
        if(instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(this);
        }
        else if(instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        OnAwake();
        Debug.Log("dont destriy awake");
    }

    private void OnDestroy()
    {
        Destroy();
    }

    #endregion

    #region Protected
    protected abstract void OnAwake();
    protected abstract void Destroy();
    #endregion
}
