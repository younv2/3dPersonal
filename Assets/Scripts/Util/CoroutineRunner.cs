using System.Collections;
using UnityEngine;

/// <summary>
/// 코루틴 실행을 위한 클래스
/// </summary>
public class CoroutineRunner : MonoSingleton<CoroutineRunner>
{
    public Coroutine RunCoroutine(IEnumerator routine)
    {
        return StartCoroutine(routine);
    }
}