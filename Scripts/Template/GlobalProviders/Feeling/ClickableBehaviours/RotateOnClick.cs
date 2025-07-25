using _Game.Scripts.Template.GlobalProviders.Feeling.BaseFeelingProviders;
using _Game.Scripts.Template.GlobalProviders.Input;

namespace _Game.Scripts.Template.GlobalProviders.Feeling.ClickableBehaviours
{
    public class RotateOnClick : ObjectRotateProvider, IClickableAction
    {
        #region Public Methods

        public void ClickableActionDown()
        {
            StartRotateCoroutine();
        }

        public void ClickableActionHold()
        {
        }

        public void ClickableActionUp()
        {
        }

        #endregion
    }
}