using System.Collections.Generic;
using _Game.Scripts.Template.GlobalProviders.Interactable.Collectables;
using AssetKits.ParticleImage;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts.ScriptableObjects.Predefined
{
    [CreateAssetMenu(fileName = "UIAnimationPrefabSO", menuName = "ThisGame/UIAnimationPrefabSO", order = 0)]
    public class UIAnimationPrefabSO : SerializedScriptableObject
    {
        [Title("Collectable Animation Object Prefabs")]
        public Dictionary<GridObjectType, GameObject> _uiPrefabs;
        public Dictionary<CollectableType, GameObject> _collectablePrefabs;
        public IconProviderSO _iconProvider;
        
        public GameObject GetParticleImagePrefab(GridObjectType gridObjectType)
        {
            if (_uiPrefabs.TryGetValue(gridObjectType, out GameObject animationPrefab))
            {
                return animationPrefab;
            }
            else
            {
                Debug.LogWarning("Animation Prefab not found for the given CollectableType.");
                return null;
            }
        }
        
        public GameObject GetParticleImagePrefab(CollectableType collectableType)
        {
            if (_collectablePrefabs.TryGetValue(collectableType, out GameObject animationPrefab))
            {
                return animationPrefab;
            }
            else
            {
                Debug.LogWarning("Animation Prefab not found for the given CollectableType.");
                return null;
            }
        }
#if UNITY_EDITOR

        [Button]
        private void AssignSpriteToPrefabs()
        {
            foreach (var uiPrefab in _uiPrefabs)
            {
                var collectableType = uiPrefab.Key; // UI prefab'ın associated type'ını alır.
                var collectableIcon =
                    _iconProvider.GetGridObjectIcon(collectableType); // Icon'e sahip olan collectable type'ını alır.

                var particleImage = uiPrefab.Value.GetComponent<ParticleImage>();
                if (particleImage != null && collectableIcon != null) // Null kontrolü ekleyin.
                {
                    particleImage.sprite = collectableIcon; // UI prefab'ın sprite'ını değiştir.

                    //Set dirty ui prefab
                    UnityEditor.EditorUtility.SetDirty(uiPrefab.Value);
                }
            }
        }
#endif
    }
}