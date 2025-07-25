using _Game.Scripts._GameLogic.Data.Grid;
using _Game.Scripts._GameLogic.Logic.Grid;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Manager.Visual.Managers
{
    public class GridObjectMaterialManager : MonoBehaviour, IMatchableAction
    {
        #region Public Variables
        
        public MeshRenderer parentMeshRenderer;
        
        public Color matchColor;
        
        #endregion

        #region Private Variables

        private Material gridObjectMaterial;
        private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

        #endregion

        #region Unity Methods

        private void Awake() => SetGridObjectMaterial();

        #endregion

        #region Private Methods

        private void SetGridObjectMaterial()
        {
            gridObjectMaterial = GetComponent<MeshRenderer>().material;
        }
        
        private MeshRenderer GetMeshRenderer()
        {
            return parentMeshRenderer;
        }

        #endregion

        #region Public Methods
        
        public void MatchAction(GridTile gridTile, GridObjectType gridObjectType)
        {
            GetMeshRenderer().enabled = false;
            
            gridObjectMaterial.SetColor(
                BaseColor, 
                matchColor
            );
        }

        public void ExcludePathFoundAction(GridTile gridTile, GridObjectType gridObjectType)
        {
            GetMeshRenderer().enabled = false;
            
            gridObjectMaterial.SetColor(
                BaseColor, 
                matchColor
            );
        }

        #endregion
        
    }
}