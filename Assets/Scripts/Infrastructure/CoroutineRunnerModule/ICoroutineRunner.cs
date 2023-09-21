using System.Collections;
using UnityEngine;

namespace Infrastructure.CoroutineRunnerModule
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
    }
}