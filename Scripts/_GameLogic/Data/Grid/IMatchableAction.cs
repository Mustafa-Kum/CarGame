using _Game.Scripts._GameLogic.Logic.Grid;

namespace _Game.Scripts._GameLogic.Data.Grid
{
    public interface IMatchableAction
    {
        void MatchAction(GridTile gridTile, GridObjectType gridObjectType);
        
        void ExcludePathFoundAction(GridTile gridTile, GridObjectType gridObjectType);
    }
}