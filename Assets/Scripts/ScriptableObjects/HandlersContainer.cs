using CameraModule;
using Infrastructure.CoroutineRunnerModule;
using Infrastructure.InputHandlerModule;
using UIModule;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "HandlersContainer", menuName = "ScriptableObjects/Prefabs/HandlersContainer", order = 3)]
    public class HandlersContainer : ScriptableObject
    {
        public CameraFollower Camera;
        public CoroutineRunner CoroutineRunner;
        public InputHandler InputHandler;
        public UIController UIController;
    }
}