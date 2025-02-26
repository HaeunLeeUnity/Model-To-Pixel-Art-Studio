using UnityEngine;

namespace ModelToPixelArt.Interface
{
    public interface IUserRequestHandler
    {
        public int ViewportWidth { set; }
        public int ViewportHeight { set; }

        public void MoveViewport(Vector2 direction);
        public void AdjustViewportSize(float offset);
        public void NextAnimation();
        public void PrevAnimation();
        public void PlayAnimation();
        public void PauseAnimation();
        public void PixelationSingleImage();
        public void Rollback();
        public void PixelationAnimation(int targetFrame, string fileName);
        public void SaveSingleImage(string fileName);
        public void OpenPixelationOption();

        public void OpenSavePath();
    }
}