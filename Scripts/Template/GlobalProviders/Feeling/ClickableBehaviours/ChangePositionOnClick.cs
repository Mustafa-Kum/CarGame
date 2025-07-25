using _Game.Scripts.Template.GlobalProviders.Feeling.BaseFeelingProviders;
using _Game.Scripts.Template.GlobalProviders.Input;

namespace _Game.Scripts.Template.GlobalProviders.Feeling.ClickableBehaviours
{
    public class ChangePositionOnClick : TransformUpdateProvider, IClickableAction
    {
        #region Public Methods

        public void ClickableActionDown()
        {
            ChangePosition();
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