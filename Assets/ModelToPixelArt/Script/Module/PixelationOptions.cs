using ModelToPixelArt.Definition;
using UnityEngine;

namespace ModelToPixelArt.Module
{
    [CreateAssetMenu(fileName = "Pixelation Options", menuName = "ModelToPixelArt/New Pixelation Options")]
    public class PixelationOptions : ScriptableObject
    {
        public Color[] ColorPalette = { Color.red };
        public Color OutLineColor = Color.black;
        public EOutLineOption OutLineOption = EOutLineOption.Cross;
        public int TargetWidth = 128;
    }
}