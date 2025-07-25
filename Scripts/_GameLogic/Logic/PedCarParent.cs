using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace _Game.Scripts._GameLogic.Logic
{
    public class PedCarParent : MonoBehaviour
    {
        [BoxGroup("Prefabs")]
        public List<GameObject> PedCars;

        [BoxGroup("Position Ranges")]
        [LabelText("Z Range")]
        public Vector2 ZRange;

        [BoxGroup("Position Ranges")]
        [LabelText("X Range")]
        public Vector2 XRange;

        [BoxGroup("Settings")]
        public int CarCount = 5;

        [BoxGroup("Settings")]
        [Range(0, 10)]
        public int Hardness = 5; // Hardness level from 0 to 10

        [Button]
        public void GenerateCars()
        {
#if UNITY_EDITOR
            ClearCars();

            float spacingFactor = Hardness / 10f;
            float baseSpacing = (ZRange.y - ZRange.x) / (CarCount - 1);
            float adjustedSpacing = baseSpacing * (1 - spacingFactor);

            for (int i = 0; i < CarCount; i++)
            {
                var randomCarPrefab = PedCars[Random.Range(0, PedCars.Count)];
                float randomX = Random.Range(0, 2) == 0 ? -Random.Range(XRange.x, XRange.y) : Random.Range(XRange.x, XRange.y);
                float currentZ = ZRange.x + i * adjustedSpacing;

                // Small random offset in Z for variety
                float randomZOffset = Random.Range(-adjustedSpacing * 0.1f, adjustedSpacing * 0.1f);
                currentZ += randomZOffset;

                var car = (GameObject)PrefabUtility.InstantiatePrefab(randomCarPrefab, transform);
                car.transform.position = new Vector3(randomX, randomCarPrefab.transform.position.y, currentZ);
                car.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
#endif
        }

        [Button]
        public void ClearCars()
        {
#if UNITY_EDITOR
            foreach (Transform child in transform)
            {
                DestroyImmediate(child.gameObject);
            }
#endif
        }
    }
}