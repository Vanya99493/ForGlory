using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UIModule.Panels
{
    public class ConfirmWindow : MonoBehaviour
    {
        public event Action ConfirmAction;
        
        [SerializeField] private TextMeshProUGUI textField;
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button cancelButton;

        private Action _confirmAction;

        private void Start()
        {
            confirmButton.onClick.AddListener(OnConfirmAction);
            cancelButton.onClick.AddListener(OnCancelAction);
        }

        public void SubscribeAndActivate(string newText, Action confirmAction)
        {
            _confirmAction = confirmAction;
            ConfirmAction += _confirmAction;

            textField.text = newText;
            
            Activate();
        }

        private void OnConfirmAction()
        {
            ConfirmAction?.Invoke();
            Unsubscribe();
            Deactivate();
        }

        private void OnCancelAction()
        {
            Unsubscribe();
            Deactivate();
        }

        private void Unsubscribe()
        {
            ConfirmAction -= _confirmAction;
        }

        private void Activate()
        {
            gameObject.SetActive(true);
        }
        
        private void Deactivate()
        {
            gameObject.SetActive(false);
        }
    }
}