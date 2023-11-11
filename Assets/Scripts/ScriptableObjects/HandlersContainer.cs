using Infrastructure.InputHandlerModule;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "HandlersContainer", menuName = "ScriptableObjects/Prefabs/HandlersContainer", order = 3)]
    public class HandlersContainer : ScriptableObject
    {
        public InputHandler InputHandler;
    }
}