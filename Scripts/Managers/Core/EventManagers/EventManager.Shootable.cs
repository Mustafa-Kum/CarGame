using Fluxy;
using UnityEngine.Events;

namespace _Game.Scripts.Managers.Core
{
    public static partial class EventManager
    {
        public static class ShootableEvents
        {
            public static UnityAction<FluxyTarget> BulletFluxyFluidOnShoot;
            
            public static UnityAction OnWeaponShootUpdate;
        }
    }
}