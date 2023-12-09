using System;
using CameraModule;
using Infrastructure.CoroutineRunnerModule;
using Infrastructure.InputHandlerModule;
using ScriptableObjects;
using UIModule;
using UnityEngine;

namespace Infrastructure.Providers
{
    [Serializable]
    public class HandlersProvider
    {
        [SerializeField] private HandlersContainer handlersContainer;

        public CameraFollower GetCamera() => handlersContainer.Camera;
        public CoroutineRunner GetCoroutineRunner() => handlersContainer.CoroutineRunner;
        public InputHandler GetInputHandler() => handlersContainer.InputHandler;
        public UIController GetUIController() => handlersContainer.UIController;
    }
}