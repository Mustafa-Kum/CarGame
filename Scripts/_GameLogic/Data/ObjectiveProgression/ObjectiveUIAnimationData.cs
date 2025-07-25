using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Data.ObjectiveProgression
{
    [CreateAssetMenu(fileName = nameof(ObjectiveUIAnimationData), menuName = nameof(ObjectiveUIAnimationData) + "/UIAnimationData", order = 0)]
    public class ObjectiveUIAnimationData : SerializedScriptableObject
    {
        #region PUBLIC VARIABLES
        
        public Dictionary<GridObjectType, Sprite> ObjectiveSpriteDictionary;
        
        #endregion
        
        #region PUBLIC METHODS
        
        public Sprite GetSprite(GridObjectType type)
        {
            return ObjectiveSpriteDictionary[type];
        }
        
        #endregion
    }
}