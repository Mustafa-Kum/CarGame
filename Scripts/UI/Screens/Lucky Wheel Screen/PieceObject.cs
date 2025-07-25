using UnityEngine;
using UnityEngine.UI;

namespace FortuneWheel
{
    public class PieceObject : MonoBehaviour
    {
        public Image backgroundImage;
        public Image rewardIcon;
        public Text rewardAmount;
        public SpinWheelController.RewardEnum rewardCategory;

        public int index;

        public void SetValues(int pieceNo)
        {
            index = pieceNo;

            if (SpinWheelController.ins.useCustomBackgrounds)
            {
                backgroundImage.color = Color.white;
                backgroundImage.sprite = SpinWheelController.ins.CustomBackgrounds[pieceNo];
            }
            else
            {
                backgroundImage.color = SpinWheelController.ins.PiecesOfWheel[pieceNo].backgroundColor;
                backgroundImage.sprite = SpinWheelController.ins.PiecesOfWheel[pieceNo].backgroundSprite;
            }

            rewardCategory = SpinWheelController.ins.PiecesOfWheel[pieceNo].rewardCategory;
            rewardAmount.text = SpinWheelController.ins.PiecesOfWheel[pieceNo].rewardAmount.ToString();

            for (int i = 0; i < SpinWheelController.ins.categoryIcons.Length; i++)
            {
                if (rewardCategory == SpinWheelController.ins.categoryIcons[i].category)
                {
                    rewardIcon.sprite = SpinWheelController.ins.categoryIcons[i].rewardIcon;
                    SpinWheelController.ins.PiecesOfWheel[pieceNo].rewardIcon = SpinWheelController.ins.categoryIcons[i].rewardIcon;
                }
            }
        }
    }
}