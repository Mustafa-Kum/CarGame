﻿using _Game.Scripts.Template.GlobalProviders.Feeling.BaseFeelingProviders;
using _Game.Scripts.Template.GlobalProviders.Input;
using UnityEngine;

namespace _Game.Scripts.Template.GlobalProviders.Feeling.ClickableBehaviours
{
    public class ColorCycleOnClick : ColorCycleProvider, IClickableAction
    {
        public void ClickableActionDown()
        {
            StartColorCycle();
        }

        public void ClickableActionHold()
        {
        }

        public void ClickableActionUp()
        {
        }
    }
}