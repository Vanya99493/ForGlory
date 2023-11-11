using System;
using Infrastructure.InputHandlerModule;
using ScriptableObjects;
using UnityEngine;

namespace Infrastructure.Providers
{
    [Serializable]
    public class HandlersProvider
    {
        [SerializeField] private HandlersContainer handlersContainer;

        public InputHandler GetInputHandler() => handlersContainer.InputHandler;
    }
}