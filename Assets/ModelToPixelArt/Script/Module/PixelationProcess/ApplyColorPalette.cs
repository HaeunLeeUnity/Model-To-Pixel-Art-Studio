using System;
using ModelToPixelArt.Interface;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace ModelToPixelArt.Module.PixelationProcess
{
    public class ApplyColorPalette : IPixelationProcess
    {
        private readonly Color[] _colorPalette;

        public ApplyColorPalette(Color[] colorPalette)
        {
            _colorPalette = colorPalette;
        }

        public void Execute(NativeArray<Color> inputPixels, int targetWidth, int targetHeight,
            Action<NativeArray<Color>> onComplete)
        {
            var outputPixelNativeArray = new NativeArray<Color>(targetWidth * targetHeight, Allocator.TempJob);
            var colorPaletteNativeArray = new NativeArray<Color>(_colorPalette, Allocator.TempJob);

            var job = new ApplyColorPaletteJob
            {
                InputPixels = inputPixels,
                OutputPixels = outputPixelNativeArray,
                ColorPalette = colorPaletteNativeArray
            };

            var handle = job.Schedule(targetWidth * targetHeight, 64);
            handle.Complete();

            inputPixels.Dispose();
            colorPaletteNativeArray.Dispose();

            onComplete?.Invoke(outputPixelNativeArray);
        }

        [BurstCompile]
        private struct ApplyColorPaletteJob : IJobParallelFor
        {
            [ReadOnly] public NativeArray<Color> InputPixels;
            [ReadOnly] public NativeArray<Color> ColorPalette;
            [WriteOnly] public NativeArray<Color> OutputPixels;

            public void Execute(int index)
            {
                var pixelColor = InputPixels[index];
                if (pixelColor.a < 0.5f)
                {
                    OutputPixels[index] = Color.clear;
                    return;
                }

                OutputPixels[index] = GetMostSimilarColor(pixelColor);
            }

            private Color GetMostSimilarColor(Color input)
            {
                Color mostSimilar = default;
                var maxSimilarity = float.MinValue;

                foreach (var selectedColor in ColorPalette)
                {
                    var similarity = CalculateSimilarity(input, selectedColor);

                    if (!(similarity > maxSimilarity)) continue;
                    maxSimilarity = similarity;
                    mostSimilar = selectedColor;
                }

                return mostSimilar;
            }

            private static float CalculateSimilarity(Color color1, Color color2)
            {
                var diffR = math.abs(color1.r - color2.r);
                var diffG = math.abs(color1.g - color2.g);
                var diffB = math.abs(color1.b - color2.b);

                return 3 - (diffR + diffG + diffB);
            }
        }
    }
}