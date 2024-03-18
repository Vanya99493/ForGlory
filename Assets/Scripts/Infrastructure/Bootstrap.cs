using CameraModule;
using Infrastructure.CoroutineRunnerModule;
using Infrastructure.InputHandlerModule;
using Infrastructure.Providers;
using LevelModule.Data;
using UIModule;
using UnityEngine;

namespace Infrastructure
{
    public class Bootstrap : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private HandlersProvider handlersProvider;
        [SerializeField] private GameScenePrefabsProvider gameScenePrefabsProvider;
        [SerializeField] private CellDataProvider cellDataProvider;
        [Header("New level data")]
        [SerializeField] private LevelData newLevelData;
        
        private Game _game;
        private CoroutineRunner _coroutineRunner;
        private InputHandler _inputHandler;
        private UIController _uiController;
        private CameraFollower _mainCamera;
        
        private void Awake()
        {
            InstantiateComponents();

            _game = new Game(_uiController, _mainCamera, _coroutineRunner, _inputHandler, cellDataProvider, gameScenePrefabsProvider, newLevelData);
            _game.StartGame();
        }
        
        private void InstantiateComponents()
        {
            var parent = new GameObject("HandlersParent");
            
            _coroutineRunner = Instantiate(handlersProvider.GetCoroutineRunner(), parent.transform);
            _inputHandler = Instantiate(handlersProvider.GetInputHandler(), parent.transform);
            _uiController = Instantiate(handlersProvider.GetUIController(), parent.transform);
            _mainCamera = Instantiate(handlersProvider.GetCamera(), parent.transform);
        }
    }
}