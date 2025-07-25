using _Game.Scripts.Managers.Core;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Grid_Object
{
    public class BlackBgOnHighlight : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _blackScreen;

        private void OnEnable()
        {
            EventManager.GridEvents.OnGridObjectClicked += BlackScreenEvent;
            EventManager.InputEvents.MouseUp += BlackScreenBackToNormal;
        }

        private void OnDisable()
        {
            EventManager.GridEvents.OnGridObjectClicked -= BlackScreenEvent;
            EventManager.InputEvents.MouseUp -= BlackScreenBackToNormal;
        }

        private void BlackScreenEvent()
        {
            if (_blackScreen != null)
            {
                _blackScreen.DOFade(0.8f, 0.2f);
            }
        }
        
        private void BlackScreenBackToNormal()
        {
            if (_blackScreen != null)
                _blackScreen.DOFade(0f, 0.2f);
        }
        
        private void BlackScreenEvent(GridObject arg0)
        {
            BlackScreenEvent();
        }
    }
}