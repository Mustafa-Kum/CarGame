using _Game.Scripts.Template.GlobalProviders.Feeling.BaseFeelingProviders;
using _Game.Scripts.Template.GlobalProviders.Input;
using Handler.Extensions;
using UnityEngine;

namespace _Game.Scripts.Template.GlobalProviders.Feeling.ClickableBehaviours
{
    public class PunchScaleOnClick : PunchScaleProvider, IClickableAction
    {
        public void ClickableActionDown()
        {
            PunchScale();
        }

        public void ClickableActionHold()
        {
        }

        public void ClickableActionUp()
        {
        }
    }
}