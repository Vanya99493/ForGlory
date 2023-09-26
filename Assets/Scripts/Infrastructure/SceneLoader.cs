using System;
using System.Collections;
using Infrastructure.CoroutineRunnerModule;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure
{
    public class SceneLoader
    {
        public void LoadScene(string nextScene, CoroutineRunner coroutineRunner, Action callback)
        {
            if (SceneManager.GetActiveScene().name == nextScene)
                return;

            AsyncOperation waitSceneLoadedOperation = SceneManager.LoadSceneAsync(nextScene/*, LoadSceneMode.Additive*/);
            // need to change this part of code to additive scene load and remove DontDestroyOnLoad() part in Bootstrap.cs 

            coroutineRunner.StartCoroutine(WaitSceneLoad(waitSceneLoadedOperation, callback));
        }

        private IEnumerator WaitSceneLoad(AsyncOperation waitSceneLoadedOperation, Action callback)
        {
            while (!waitSceneLoadedOperation.isDone)
            {
                yield return null;
            }
            callback?.Invoke();
        }
    }
}