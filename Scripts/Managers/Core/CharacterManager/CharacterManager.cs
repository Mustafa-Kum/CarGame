using System;
using _Game.Scripts.Helper.Extensions.System;
using _Game.Scripts.InGame.ReferenceHolder;
using Handler.Extensions;
using UnityEngine;

namespace _Game.Scripts.Managers.Core.CharacterManager
{
    public class CharacterManager : MonoBehaviour
    {
        [SerializeField] private GameObject _character;
        private GameObject _characterMasterVariant;

        private void OnEnable()
        {
            EventManager.InGameEvents.BeforeLevelLoaded += HandleOnBeforeLoadLevel;
            EventManager.InGameEvents.LevelLoaded += HandleOnLevelLoaded;
        }
        
        private void OnDisable()
        {
            EventManager.InGameEvents.BeforeLevelLoaded -= HandleOnBeforeLoadLevel;
            EventManager.InGameEvents.LevelLoaded -= HandleOnLevelLoaded;
        }
        
        private void HandleOnBeforeLoadLevel()
        {
            if(_character.transform.parent)
                _characterMasterVariant = _character.transform.parent.gameObject;
            
            if(_characterMasterVariant.activeSelf) return;
            _characterMasterVariant.SetActive(true);
            
            TDebug.LogGreen("Character Master Variant Activated");
        }

        private void HandleOnLevelLoaded(GameObject level)
        {
            if(_character == null) return;
            _character.transform.position = level.GetComponent<LevelReferenceHolder>().CharSpawnPoint != null ? level.GetComponent<LevelReferenceHolder>().CharSpawnPoint.position : Vector3.zero;
        }
    }
}
