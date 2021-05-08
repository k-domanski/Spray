using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proxy<T> : ScriptableObject where T : MonoBehaviour
{
    [System.NonSerialized] private T _subject;
    public T Get() => _subject;
    public void Register(T value) => _subject = value;
    public void Unregister() => _subject = null;
}
