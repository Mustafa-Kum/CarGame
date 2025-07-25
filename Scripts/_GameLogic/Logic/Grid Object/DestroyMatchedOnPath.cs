using _Game.Scripts._GameLogic.Data.Grid;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Grid_Object
{
    public class DestroyMatchedOnPath : MonoBehaviour, IPathAction
    {
        public void PathAction()
        {
            gameObject.SetActive(false);
        }
    }
}