using System;
using _Game.Scripts._GameLogic.Data.Grid;
using _Game.Scripts._GameLogic.Logic.Grid;
using _Game.Scripts.Managers.Core;
using _Game.Scripts.Managers.Core.HapticManager;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Grid_Object
{
    public class ScaleOnHighlight : MonoBehaviour , HiglihtAction, DehighlightAction, IConnectedAction
    {
        private Vector3 originalScale;
        [SerializeField] private GameObject _circle;

        private void Awake()
        {
            originalScale = transform.localScale;
        }
        public void HighlightAction()
        {
            transform.DOMoveY(0.2f, 0.2f);
        }

        public void DehighlightAction()
        {
            transform.DOScale( originalScale, 0.2f);
            transform.DOMoveY(0f, 0.2f);
        }
        
        public void BackToNormal()
        {
            transform.DOScale( originalScale, 0.2f);
            transform.DOMoveY(0, 0.2f);
        }

        public void ConnectedAction()
        {
            if (_circle == null)
                return;
            
            _circle.SetActive(true);
            EventManager.AudioEvents.AudioPlay?.Invoke(SoundType.OnConnect, true, false);
            
            transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.3f).OnComplete(() =>
            {
                transform.DOPunchScale(new Vector3(0.03f, 0.03f, 0.03f), 0.5f, 1, 0.5f).SetLoops(-1);
            });
        }
        
        public void OnDisable()
        { 
            BackToNormal();
        }

        private void OnEnable()
        {
            if (_circle == null)
                return;
            
            _circle.SetActive(false);
        }
    }
}
