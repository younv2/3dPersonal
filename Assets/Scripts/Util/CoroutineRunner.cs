using System.Collections;
using UnityEngine;

public class CoroutineRunner : MonoSingleton<CoroutineRunner>
{
    public Coroutine RunCoroutine(IEnumerator routine)
    {
        return StartCoroutine(routine);
    }
}