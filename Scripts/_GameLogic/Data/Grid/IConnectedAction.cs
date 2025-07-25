namespace _Game.Scripts._GameLogic.Data.Grid
{
    public interface IConnectedAction
    {
        void ConnectedAction();
    }
    
    public interface HiglihtAction
    {
        void HighlightAction();
        void BackToNormal();
    }
    
    public interface DehighlightAction
    {
        void DehighlightAction();
    }
}
