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

            AsyncOperation waitSceneLoadedOperation = SceneManager.LoadSceneAsync(nextScene);
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