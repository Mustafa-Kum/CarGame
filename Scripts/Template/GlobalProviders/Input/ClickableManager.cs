using _Game.Scripts.Managers.Core;
using Handler.Extensions;
using UnityEngine;

namespace _Game.Scripts.Template.GlobalProviders.Input
{
    public class ClickableManager : InputProvider
    {
        private Camera _camera;
        private LayerMask _ignoreRaycastLayer = 1 << 2;

        #region Inherited Methods

        protected override void Initialize()
        {
            base.Initialize();
            _camera = Camera.main;
        }

        protected override void OnClickDown(ClickData clickData)
        {
            var ray = _camera!.ScreenPointToRay(UnityEngine.Input.mousePosition);

            if (Physics.Raycast(ray, out var hit))
            {
                IClickable clickable = hit.collider.GetComponent<IClickable>();
                
                if (clickable != null)
                {
                    clickable.OnClickedDown();
                }
            }

        }

        protected override void OnClickHold(ClickData clickData)
        {
            var ray = _camera.ScreenPointToRay(UnityEngine.Input.mousePosition);
            
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, ~_ignoreRaycastLayer.value))
            {
                IClickable clickable = hit.collider.GetComponent<IClickable>();
                if (clickable != null)
                {
                    clickable.OnClickedHold();
                }
            }
        }

        protected override void OnClickUp(ClickData clickData)
        {
            EventManager.InputEvents.MouseUp.Invoke();

        }

        #endregion
    }
}