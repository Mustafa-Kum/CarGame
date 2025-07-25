using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.Scripts.Template.GlobalProviders.Combat
{
    public class WeaponUpgradeLevelingContainer : MonoBehaviour
    {
        #region Inspector Fields

        [SerializeField] private int defaultUpgradeLevel;

        [SerializeField] private int defaultWeaponLevel;

        #endregion

        #region Properties

        public int UpgradeLevel => defaultUpgradeLevel;
        
        public int WeaponLevel => defaultWeaponLevel;

        #endregion
    }
}