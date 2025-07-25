using _Game.Scripts._GameLogic.Data.Grid;
using _Game.Scripts._GameLogic.Logic.Grid;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Manager.Visual.Logic
{
    public class DisableFlowObjectOnMatch : MonoBehaviour, IMatchableAction
    {
        #region INSCPETOR VARIABLES

        public GameObject flowObject;

        #endregion
        
        public void MatchAction(GridTile gridTile, GridObjectType gridObjectType)
        {
            DOVirtual.DelayedCall(0.5f, () => Destroy(flowObject));
        }

        public void ExcludePathFoundAction(GridTile gridTile, GridObjectType gridObjectType)
        {
            
        }
    }
}