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
    }
    #endregion

    #region Protected
    protected abstract void OnAwake();
    #endregion
}
