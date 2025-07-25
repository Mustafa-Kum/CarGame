using _Game.Scripts.ScriptableObjects.Saveable;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.Scripts.Template.GlobalProviders.Combat
{
    [CreateAssetMenu(fileName = "FireRateUpgradable", menuName = "ThisGame/FireRateUpgradable", order = 5)]
    public class FireRateUpgradable : UpgradableSO
    {
        public override float GetValue(int currentLevel)
        {
            return base.GetValue(currentLevel);
        }

    }
}