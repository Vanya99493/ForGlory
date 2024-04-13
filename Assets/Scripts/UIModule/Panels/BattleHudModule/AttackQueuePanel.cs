using System.Collections.Generic;
using System.Linq;
using CharacterModule.PresenterPart;
using CustomClasses;
using Infrastructure.Providers;
using Infrastructure.ServiceLocatorModule;
using UnityEngine;
using UnityEngine.UI;

namespace UIModule.Panels.BattleHudModule
{
    public class AttackQueuePanel : MonoBehaviour
    {
        [SerializeField] private Image currentAttackImage;
        [SerializeField] private Transform queueParent;
        [SerializeField] private Transform tempParent;

        private Dictionary<string, Sprite> _sprites;
        private List<Image> _images;
        private Image _pickedImage;
        
        public void Instantiate(Queue<CharacterPresenter> attackQueue)
        {
            _sprites = new Dictionary<string, Sprite>();
            _images = new List<Image>();
            DestroyChildren();
            var attackQueueArray = attackQueue.ToArray();
            var characterImagePrefab = ServiceLocator.Instance.GetService<UIPrefabsProvider>().GetQueueCharacterIcon();
            var gameScenePrefabsProvider = ServiceLocator.Instance.GetService<GameScenePrefabsProvider>();

            for (int i = 0; i < attackQueueArray.Length; i++)
            {
                string characterName = attackQueueArray[i].Model.Name;
                _sprites.TryAdd(characterName, gameScenePrefabsProvider.GetCharacterByName(characterName).Icon);
                if(i < attackQueueArray.Length - 1)
                    _images.Add(Instantiate(characterImagePrefab, queueParent));
            }
        }

        public void UpdateAttackingCharacter(CharacterPresenter attackingCharacter, Queue<CharacterPresenter> attackQueue)
        {
            var attackQueueArray = attackQueue.ToArray();
            var attackQueueArrayAlive = attackQueueArray.Where(character => character.Model.Health > 0).ToArray();

            currentAttackImage.sprite = _sprites[attackingCharacter.Model.Name];
            
            int index = 0;
            foreach (var image in _images)
            {
                if(index >= attackQueueArrayAlive.Length)
                    image.gameObject.SetActive(false);
                else
                    image.sprite = _sprites[attackQueueArrayAlive[index].Model.Name];
                index++;
            }
        }

        private void DestroyChildren()
        {
            for (int i = 0; i < queueParent.childCount; i++)
                Destroy(queueParent.gameObject.transform.GetChild(i).gameObject);
            
            for (int i = 0; i < tempParent.childCount; i++)
                Destroy(tempParent.gameObject.transform.GetChild(i).gameObject);
        }
    }
}