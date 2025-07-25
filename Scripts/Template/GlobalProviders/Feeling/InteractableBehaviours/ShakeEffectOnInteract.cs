using Cinemachine;
using DG.Tweening;

namespace _Game.Scripts.Managers
{
    public class ShakeEffectOnInteract : ICameraShakeEffect
    {
        public void ApplyShake(CinemachineVirtualCamera camera)
        {
            var perlin = camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            perlin.m_AmplitudeGain = 1f;
            perlin.m_FrequencyGain = 1f;

            camera.transform.DOShakePosition(0.5f, 0.5f, 10, 90f, false);

            DOTween.Sequence().AppendInterval(0.5f).OnComplete(() =>
            {
                perlin.m_AmplitudeGain = 0f;
                perlin.m_FrequencyGain = 0f;
            });
        }
    }
    
    public interface ICameraShakeEffect
    {
        void ApplyShake(CinemachineVirtualCamera camera);
    }
}