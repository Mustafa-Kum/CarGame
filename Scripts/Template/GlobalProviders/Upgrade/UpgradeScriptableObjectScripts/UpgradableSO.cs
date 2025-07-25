using _Game.Scripts.Template.GlobalProviders.Interactable.Collectables;
using _Game.Scripts.Template.GlobalProviders.Upgrade;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts.ScriptableObjects.Saveable
{
    public class UpgradableSO : SerializedScriptableObject
    {
        public UpgradeLevelData[] upgradeLevelData;
        
        public UpgradeType upgradeType;
        public CollectableType collectableType;
        
        [SerializeField] private bool isFormulaWillBeUsed;
        [SerializeField] private float formulaValueA;
        [SerializeField] private float formulaValueB;
        [SerializeField] private float formulaValueC;
        [SerializeField] private float formulaCostA;
        [SerializeField] private float formulaCostB;
        [SerializeField] private float formulaCostC;
        
        
        [PropertyOrder(-1)]
        [SerializeField, LabelText("Level Index")]
        private int testLevelIndex;

        private string infoBoxContent = "";

        [Button, PropertyOrder(0)]
        [LabelText("Check Level Value")]
        private void CheckLevelValue()
        {
            float value = GetValue(testLevelIndex);
            // You can format the infoBoxContent string as per your requirement
            infoBoxContent = $"Level {testLevelIndex} Value: {value}";
        }

        [InfoBox("$infoBoxContent", InfoMessageType.Info), PropertyOrder(1)]
        [ReadOnly]
        public string InfoBoxField;
        
        
        
        
        
        [Button]
        public virtual void SetAllLevelsCollecabletype()
        {
            for (int i = 0; i < upgradeLevelData.Length; i++)
            {
                upgradeLevelData[i].collectableType = collectableType;
            }
        }
        public virtual int GetRequiredCurrencyForNextLevel(int currentLevel)
        {
            if (isFormulaWillBeUsed)
            {
                return Mathf.RoundToInt(formulaCostA * Mathf.Pow(currentLevel,2)+ formulaCostB*currentLevel+ formulaCostC);
            }
            else
            {
                return upgradeLevelData[currentLevel].requiredCurrencyForNextLevel;
            }
        }
    
        
        public virtual CollectableType GetCollectableType(int currentLevel)
        {
            if (isFormulaWillBeUsed)
                return collectableType;
            
            return upgradeLevelData[currentLevel].collectableType;
        }
        
        public virtual float GetValue(int currentLevel)
        {
            if (isFormulaWillBeUsed)
            {
                //ax^2+bx+c
                return formulaValueA * Mathf.Pow(currentLevel,2)+formulaValueB*currentLevel+formulaValueC;
            }
            else
            {
                return upgradeLevelData[currentLevel].value;
            }
        }
        
        public virtual int GetMaxLevel()
        {
            return upgradeLevelData.Length - 1;
        }
        
    }
    
    
    [System.Serializable]
    public class UpgradeLevelData
    {
        public int requiredCurrencyForNextLevel;
        public float value;
        public CollectableType collectableType;
    }
}
