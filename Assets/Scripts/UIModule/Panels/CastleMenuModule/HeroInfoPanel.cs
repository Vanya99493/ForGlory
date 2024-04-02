using System;
using CharacterModule.PresenterPart;
using Infrastructure.Providers;
using Infrastructure.ServiceLocatorModule;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UIModule.Panels.CastleMenuModule
{
    public class HeroInfoPanel : MonoBehaviour
    {
        [SerializeField] private Image heroIcon;
        [SerializeField] private TextMeshProUGUI heroInfo;

        private void OnEnable()
        {
            heroIcon.gameObject.SetActive(false);
            heroInfo.gameObject.SetActive(false);
        }

        public void SetHeroInfo(CharacterPresenter character, Sprite heroSprite)
        {
            heroIcon.gameObject.SetActive(true);
            heroInfo.gameObject.SetActive(true);

            heroIcon.sprite = heroSprite;
            heroInfo.text = $"Name: {character.Model.Name}\n" +
                            $"Health: {character.Model.Health}/{character.Model.MaxHealth}\n" +
                            $"Damage: {character.Model.Damage}\n" +
                            $"Energy: {character.Model.Energy}/{character.Model.MaxEnergy}\n" +
                            $"Initiative: {character.Model.Initiative}";
        }

        public void ResetHeroInfo()
        {
            heroIcon.gameObject.SetActive(false);
            heroInfo.gameObject.SetActive(false);
        }
    }
}