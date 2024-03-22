using System;
using CharacterModule.ModelPart;
using CharacterModule.PresenterPart;
using CharacterModule.ViewPart;
using Infrastructure.Providers;
using Infrastructure.ServiceLocatorModule;
using Infrastructure.Services;
using UIModule.Panels.Base;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace UIModule.Panels.CastleMenuModule
{
    public class CastleMenuUIPanel : BaseUIPanel
    {
        public event Action ExitCastleAction;

        [SerializeField] private Button exitCastleButton;
        [SerializeField] private Button acceptAndExitCastleButton;
        [SerializeField] private CastleGarrisonPanel castleGarrisonPanel;
        [SerializeField] private HeroInfoPanel heroInfoPanel;
        [SerializeField] private TeamPositionArea[] teamPositionArea;

        /*---*/ [SerializeField] /*---*/ private HeroCard[] _heroCards;

        private void Awake()
        {
            //*****
            for (int i = 0; i < teamPositionArea.Length; i++)
            {
                teamPositionArea[i].Initialize(castleGarrisonPanel);
            }

            for (int i = 0; i < _heroCards.Length; i++)
            {
                _heroCards[i].Initialize(new PlayerCharacterPresenter(
                    new PlayerCharacterModel(
                        100 + i,
                        i % 2 == 0 ? "PlayerMale" : "PlayerFemale",
                        40 + 20 * i,
                        4,
                        5,
                        20,
                        35
                        ),
                    Instantiate(new GameObject().AddComponent<PlayerCharacterView>())),
                    ServiceLocator.Instance.GetService<GameScenePrefabsProvider>().GetCharacterByName(
                        i % 2 == 0 ? "PlayerMale" : "PlayerFemale").CharacterData.Icon
                    );
                _heroCards[i].ClickAction += heroInfoPanel.SetHeroInfo;
            }
            //*****
        }

        protected override void SubscribeActions()
        {
            ServiceLocator.Instance.GetService<PauseController>().TurnOnPause();
            exitCastleButton.onClick.AddListener(ExitCastle);
        }

        protected override void UnsubscribeActions()
        {
            exitCastleButton.onClick.RemoveListener(ExitCastle);
            ServiceLocator.Instance.GetService<PauseController>().TurnOffPause();
        }
        
        

        private void ExitCastle()
        {
            ExitCastleAction?.Invoke();
        }
    }
}