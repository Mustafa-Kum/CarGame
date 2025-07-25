using System;
using _Game.Scripts.Template.GlobalProviders.Feeling.BaseFeelingProviders;
using UnityEngine;

namespace _Game.Scripts.Template.GlobalProviders.Feeling.Generic
{
    public class PunchScaleOnEnable : PunchScaleProvider
    {
        #region Unity Events

        private void OnEnable()
        {
            PunchScale();
        }

        #endregion
    }
}