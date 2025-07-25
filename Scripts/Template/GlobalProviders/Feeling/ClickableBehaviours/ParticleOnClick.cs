using _Game.Scripts.Template.GlobalProviders.Feeling.BaseFeelingProviders;
using _Game.Scripts.Template.GlobalProviders.Input;

namespace _Game.Scripts.Template.GlobalProviders.Feeling.ClickableBehaviours
{
    public class ParticleOnClick : ParticleProvider, IClickableAction
    {
        #region Public Methods
        
        public void ClickableActionDown()
        {
            PlayParticle();
        }

        public void ClickableActionHold()
        {
        }

        public void ClickableActionUp()
        {
        }
        
        #endregion
    }
}