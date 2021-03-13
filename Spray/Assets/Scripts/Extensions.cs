using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions 
{
    public static Coroutine Delay(this MonoBehaviour mono, Action method, float time)
    {
        return mono.StartCoroutine(DelayCoroutine(method, time));
    }


    private static IEnumerator DelayCoroutine(Action method, float time)
    {
        yield return new WaitForSeconds(time);
        method();
    }
}
