using CameraModule;
using Infrastructure.CoroutineRunnerModule;
using Infrastructure.InputHandlerModule;
using Infrastructure.Providers;
using UIModule;
using UnityEngine;

namespace Infrastructure
{
    public class Bootstrap : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private HandlersProvider handlersProvider;
        [SerializeField] private GameScenePrefabsProvider gameScenePrefabsProvider;
        [SerializeField] private CellDataProvider cellDataProvider;
        
        private Game _game;
        private CoroutineRunner _coroutineRunner;
        private InputHandler _inputHandler;
        private UIController _uiController;
        private CameraFollower _mainCamera;
        
        private void Awake()
        {
            InstantiateComponents();

            _game = new Game(_uiController, _mainCamera, _coroutineRunner, _inputHandler, cellDataProvider, gameScenePrefabsProvider);
            _game.StartGame();
        }
        
        private void InstantiateComponents()
        {
            _coroutineRunner = Instantiate(handlersProvider.GetCoroutineRunner());
            _inputHandler = Instantiate(handlersProvider.GetInputHandler());
            _uiController = Instantiate(handlersProvider.GetUIController());
            _mainCamera = Instantiate(handlersProvider.GetCamera());
        }
    }
}