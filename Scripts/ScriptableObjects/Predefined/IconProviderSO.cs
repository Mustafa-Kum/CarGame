using System.Collections.Generic;
using _Game.Scripts.Template.GlobalProviders.Interactable.Collectables;
using _Game.Scripts.Template.GlobalProviders.Upgrade;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts.ScriptableObjects.Predefined
{
    [CreateAssetMenu(fileName = "IconProviderSO", menuName = "ThisGame/IconProviderSO", order = 0)]
    public class IconProviderSO : SerializedScriptableObject
    {
        public Dictionary<GridObjectType, Sprite> cubeIconDict;
        public Dictionary<UpgradeType, Sprite> upgradeIconDict;
        public Dictionary<CollectableType, Sprite> collectableIconDict;

        public Sprite GetGridObjectIcon(GridObjectType gridObjectType)
        {
            if (cubeIconDict == null)
            {
                Debug.LogError("cubeIconDict is not initialized.");
                return null;
            }

            if (cubeIconDict.TryGetValue(gridObjectType, out Sprite icon))
            {
                return icon;
            }
            else
            {
                Debug.LogWarning("Icon not found for the given CollectableType.");
                return null;
            }
        }
        
        
        public Sprite GetCollectableIcon(CollectableType gridObjectType)
        {
            if (collectableIconDict == null)
            {
                Debug.LogError("cubeIconDict is not initialized.");
                return null;
            }

            if (collectableIconDict.TryGetValue(gridObjectType, out Sprite icon))
            {
                return icon;
            }
            else
            {
                Debug.LogWarning("Icon not found for the given CollectableType.");
                return null;
            }
        }
        
        
        public Sprite GetUpgradeIcon(UpgradeType upgradableSoUpgradeType)
        {
            if (upgradeIconDict == null)
            {
                Debug.LogError("upgradeIconDict is not initialized.");
                return null;
            }

            if (upgradeIconDict.TryGetValue(upgradableSoUpgradeType, out Sprite icon))
            {
                return icon;
            }
            else
            {
                Debug.LogWarning("Icon not found for the given UpgradeType.");
                return null;
            }
        }
    }
}