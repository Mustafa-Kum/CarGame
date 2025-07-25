using UnityEngine;

namespace _Game.Scripts
{
    [CreateAssetMenu(fileName = "ButtonImage", menuName = "ThisGame/ButtonImage", order = 0)]
    public class ButtonImageSO : ScriptableObject
    {
        [SerializeField] private Sprite _defaultImage;
        [SerializeField] private Sprite _clickedImage;
        
        public Sprite DefaultImage 
        {
            get => _defaultImage;
            set => _defaultImage = value;
        }
        
        public Sprite ClickedImage 
        {
            get => _clickedImage;
            set => _clickedImage = value;
        } 
    }
}