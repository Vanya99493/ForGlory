using System.Collections.Generic;
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

        private List<Pair<CharacterPresenter, Image>> _attackQueue;
        private Image _pickedImage;
        private int _currentIndex = 0;
        
        public void Instantiate(Queue<CharacterPresenter> attackQueue)
        {
            _attackQueue = new List<Pair<CharacterPresenter, Image>>();
            DestroyChildren();
            _currentIndex = 0;
            
            var attackQueueArray = attackQueue.ToArray();
            var characterImagePrefab = ServiceLocator.Instance.GetService<UIPrefabsProvider>().GetQueueCharacterIcon();

            for (int i = 0; i < attackQueueArray.Length; i++)
            {
                var instantiatedImage = Instantiate(characterImagePrefab, queueParent);
                instantiatedImage.sprite = ServiceLocator.Instance.GetService<GameScenePrefabsProvider>()
                    .GetCharacterByName(attackQueueArray[i].Model.Name).Icon;
                _attackQueue.Add(new Pair<CharacterPresenter, Image>(attackQueueArray[i], instantiatedImage));
            }
        }

        public void UpdateAttackingCharacter()
        {
            _pickedImage?.transform.SetParent(queueParent);
            _pickedImage = _attackQueue[_currentIndex].SecondValue;
            currentAttackImage.sprite = _pickedImage.sprite;
            _pickedImage.transform.SetParent(tempParent);
            do
            {
                _currentIndex = ++_currentIndex % _attackQueue.Count;
            } while (_attackQueue[_currentIndex].FirstValue.Model.Health <= 0);
            
            UpdateQueue();
        }

        public void UpdateQueue()
        {
            var newAttackQueue = new List<Pair<CharacterPresenter, Image>>();
            foreach (var pair in _attackQueue)
            {
                if (pair.FirstValue.Model.Health <= 0)
                    Destroy(pair.SecondValue.gameObject);
                else
                    newAttackQueue.Add(pair);
            }

            _attackQueue = newAttackQueue;
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