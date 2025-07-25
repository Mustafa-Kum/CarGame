using System.Collections.Generic;
using _Game.Scripts.ScriptableObjects.RunTime;
using _Game.Scripts.ScriptableObjects.Saveable;
using UnityEngine;

namespace _Game.Scripts.Template.GlobalProviders.Upgrade
{
    [CreateAssetMenu(fileName = "PlayerUpgradeData", menuName = "ThisGame/Upgrade/PlayerUpgradeData", order = 1)]
    public class PlayerUpgradeData : PersistentSaveManager<PlayerUpgradeData>, IResettable
    {
        public Dictionary<UpgradeType, int> upgradeLevelDict;

        public int maxLevel = 10;

        public int GetUpgradeLevel(UpgradeType upgradeType)
        {
            if (upgradeLevelDict.TryGetValue(upgradeType, out var level))
            {
                return level;
            }
            upgradeLevelDict.Add(upgradeType, 1);

            return 1;
        }

        public void SetUpgradeLevel(UpgradeType upgradeType, int level)
        {
            var ClampedLevel = Mathf.Clamp(level, 1, maxLevel);

            if (upgradeLevelDict.ContainsKey(upgradeType))
            {
                upgradeLevelDict[upgradeType] = ClampedLevel;
            }
            else
            {
                upgradeLevelDict.Add(upgradeType, ClampedLevel);
            }
        }

        public void IncreaseUpgradeLevel(UpgradeType upgradeType, int increaseAmount = 1)
        {
            Debug.Log("IncreaseUpgradeLevel: " + upgradeType + " " + increaseAmount);
            
            if (upgradeLevelDict.ContainsKey(upgradeType))
            {
                upgradeLevelDict[upgradeType] += increaseAmount;
            }
            else
            {
                upgradeLevelDict.Add(upgradeType, increaseAmount);
            }
        }

        public void DecreaseUpgradeLevel(UpgradeType upgradeType, int decreaseAmount = 1)
        {
            if (upgradeLevelDict.ContainsKey(upgradeType))
            {
                upgradeLevelDict[upgradeType] -= decreaseAmount;
            }
            else
            {
                upgradeLevelDict.Add(upgradeType, -decreaseAmount);
            }
        }




        public int GetCurrentLevelIndex(UpgradeType upgradeType)
        {
            if (upgradeLevelDict.TryGetValue(upgradeType, out var level))
            {
                return level;
            }
            return 1;
        }

        public Dictionary<UpgradeType, int> GetUpgradeLevelDict()
        {
            return upgradeLevelDict;
        }


    }
}
