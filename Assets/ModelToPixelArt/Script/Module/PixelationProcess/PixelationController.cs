using System;
using Unity.Collections;
using UnityEngine;

namespace ModelToPixelArt.Module.PixelationProcess
{
    public static class PixelationController
    {
        public static void Pixelation(Texture2D inputTexture, PixelationOptions options, Action<Texture2D> onComplete)
        {
            var ratio = (float)options.TargetWidth / inputTexture.width;
            var targetHeight = Mathf.RoundToInt(inputTexture.height * ratio);

            var inputPixels = inputTexture.GetPixels();
            var inputPixelNativeArray = new NativeArray<Color>(inputPixels, Allocator.TempJob);

            var resizePixels = new Resize(inputTexture.width, inputTexture.height);
            resizePixels.Execute(inputPixelNativeArray, options.TargetWidth, targetHeight, OnResizeComplete);

            void OnResizeComplete(NativeArray<Color> resizedOutput)
            {
                var applyColorPalette = new ApplyColorPalette(options.ColorPalette);
                applyColorPalette.Execute(resizedOutput, options.TargetWidth, targetHeight,
                    OnApplyColorPaletteComplete);
            }

            void OnApplyColorPaletteComplete(NativeArray<Color> colorSwapOutput)
            {
                var addOutline = new AddOutLine(options.OutLineOption, options.OutLineColor);
                addOutline.Execute(colorSwapOutput, options.TargetWidth, targetHeight, OnAddOutLineComplete);
            }

            void OnAddOutLineComplete(NativeArray<Color> addOutlineOutput)
            {
                var outputPixels = addOutlineOutput.ToArray();
                var outputTexture = new Texture2D(options.TargetWidth, targetHeight);
                outputTexture.SetPixels(outputPixels);
                outputTexture.filterMode = FilterMode.Point;
                outputTexture.Apply();
                addOutlineOutput.Dispose();
                onComplete?.Invoke(outputTexture);
            }
        }
    }
}