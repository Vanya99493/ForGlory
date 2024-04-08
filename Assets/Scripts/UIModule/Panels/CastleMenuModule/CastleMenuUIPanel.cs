using System;
using CharacterModule.PresenterPart;
using Infrastructure.Providers;
using Infrastructure.ServiceLocatorModule;
using Infrastructure.Services;
using UIModule.Panels.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UIModule.Panels.CastleMenuModule
{
    public class CastleMenuUIPanel : BaseUIPanel
    {
        public event Action EnterCastleAction;
        public event Action ExitCastleAction;
        public event Action<int[]> AcceptCastleAction;

        [SerializeField] private Button exitCastleButton;
        [SerializeField] private Button acceptAndExitCastleButton;
        [SerializeField] private CastleGarrisonPanel castleGarrisonPanel;
        [SerializeField] private HeroInfoPanel heroInfoPanel;
        [SerializeField] private TeamPositionArea[] teamPositionArea;

        private GameObject _tempParent;
        private HeroCard[] _heroCards;
        private bool _isFirstEntry = true;

        private void Awake()
        {
            _heroCards = Array.Empty<HeroCard>();
            _tempParent = new GameObject
            {
                name = "TempParentForCard"
            };
            _tempParent.transform.SetParent(transform);
        }

        public void Enter(CharacterPresenter[] charactersInCastle, PlayerTeamPresenter playerTeam)
        {
            RemoveHeroCards();
            
            var heroCardPrefab = ServiceLocator.Instance.GetService<UIPrefabsProvider>().GetHeroCard();
            _heroCards = new HeroCard[charactersInCastle.Length + playerTeam.Model.GetAliveCharactersCount()];
            for (int i = 0; i < charactersInCastle.Length; i++)
            {
                _heroCards[i] = Instantiate(heroCardPrefab, castleGarrisonPanel.transform);
                _heroCards[i].Initialize(
                    charactersInCastle[i], 
                    ServiceLocator.Instance.GetService<GameScenePrefabsProvider>().
                        GetCharacterByName(charactersInCastle[i].Model.Name).Icon,
                    _tempParent
                    );
                _heroCards[i].ClickAction += heroInfoPanel.SetHeroInfo;
            }

            for (int i = 0, k = charactersInCastle.Length; i < 3; i++)
            {
                if (playerTeam.Model.GetCharacterPresenter(i) != null)
                {
                    _heroCards[k] = Instantiate(heroCardPrefab, teamPositionArea[i].transform);
                    _heroCards[k].Initialize(
                        playerTeam.Model.GetCharacterPresenter(i), 
                        ServiceLocator.Instance.GetService<GameScenePrefabsProvider>().
                            GetCharacterByName(playerTeam.Model.GetCharacterPresenter(i).Model.Name).Icon,
                        _tempParent
                    );
                    _heroCards[k].ClickAction += heroInfoPanel.SetHeroInfo;
                    k++;
                }
            }

            _isFirstEntry = playerTeam.Model.GetAliveCharactersCount() == 0;
        }

        private void RemoveHeroCards()
        {
            for (int i = 0; i < _heroCards.Length; i++)
            {
                _heroCards[i].Destroy();
            }
            _heroCards = Array.Empty<HeroCard>();
        }

        protected override void SubscribeActions()
        {
            ServiceLocator.Instance.GetService<PauseController>().TurnOnGamePause();
            exitCastleButton.onClick.AddListener(ExitCastle);
            acceptAndExitCastleButton.onClick.AddListener(AcceptAndExitCastle);
            EnterCastleAction?.Invoke();
        }

        protected override void UnsubscribeActions()
        {
            RemoveHeroCards();
            exitCastleButton.onClick.RemoveListener(ExitCastle);
            acceptAndExitCastleButton.onClick.RemoveListener(AcceptAndExitCastle);
            ServiceLocator.Instance.GetService<PauseController>().TurnOffGamePause();
        }

        private void AcceptAndExitCastle()
        {
            int[] selectedCharactersId = {-1, -1, -1};
            bool teamIsNotEmpty = false;

            foreach (var heroCard in _heroCards)
            {
                for (int i = 0; i < teamPositionArea.Length; i++)
                {
                    if (heroCard.transform.parent.Equals(teamPositionArea[i].transform))
                    {
                        selectedCharactersId[i] = heroCard.CharacterId;
                        teamIsNotEmpty = true;
                    }
                }
            }

            if (!teamIsNotEmpty)
                return;
            
            AcceptCastleAction?.Invoke(selectedCharactersId);
            ExitCastleAction?.Invoke();
        }
        
        private void ExitCastle()
        {
            if (_isFirstEntry)
                return;
            
            ExitCastleAction?.Invoke();
        }
    }
}