using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic
{
    public abstract class BaseInputProvider : MonoBehaviour
    {
        protected abstract void ClickedDown();

        protected abstract void ClickedHold();

        protected abstract void ClickedUp();

        private void Update()
        {
            CheckInput();
        }

        private void CheckInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                ClickedDown();
            }
            if (Input.GetMouseButton(0))
            {
                ClickedHold();
            }
            if (Input.GetMouseButtonUp(0))
            {
                ClickedUp();
            }
        }
    }
}