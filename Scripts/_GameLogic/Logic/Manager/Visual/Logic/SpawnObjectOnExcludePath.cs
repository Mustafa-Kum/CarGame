using _Game.Scripts._GameLogic.Data.Grid;
using _Game.Scripts._GameLogic.Logic.Grid;
using _Game.Scripts.Template.GlobalProviders.Feeling.BaseFeelingProviders;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Manager.Visual.Logic
{
    public class SpawnObjectOnExcludePath : MonoBehaviour, IMatchableAction
    {
        #region PUBLIC VARIABLES

        [SerializeField] private ObjectSpawnProvider _objectSpawnProvider;

        #endregion
        
        public void MatchAction(GridTile gridTile, GridObjectType gridObjectType) { }

        public void ExcludePathFoundAction(GridTile gridTile, GridObjectType gridObjectType)
        {
          
        }
    }
}