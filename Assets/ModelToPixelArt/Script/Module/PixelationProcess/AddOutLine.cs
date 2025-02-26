using System;
using ModelToPixelArt.Definition;
using ModelToPixelArt.Interface;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace ModelToPixelArt.Module.PixelationProcess
{
    public class AddOutLine : IPixelationProcess
    {
        private readonly Color _outLineColor;
        private readonly EOutLineOption _outLineOption;

        public AddOutLine(EOutLineOption outLineOption, Color outLineColor)
        {
            _outLineOption = outLineOption;
            _outLineColor = outLineColor;
        }

        public void Execute(NativeArray<Color> inputPixels, int targetWidth, int targetHeight,
            Action<NativeArray<Color>> onComplete)
        {
            var totalLength = targetWidth * targetHeight;
            var paintedArea = new NativeArray<bool>(totalLength, Allocator.TempJob);

            var jobPaintedArea = new GetPaintedArea
            {
                inputPixels = inputPixels,
                paintedArea = paintedArea
            };

            var handlePaintedArea = jobPaintedArea.Schedule(totalLength, 64);


            var jobAddOutline = new AddOutlineJob
            {
                OutputPixels = inputPixels,
                PaintedArea = paintedArea,
                OutLineOption = _outLineOption,
                OutLineColor = _outLineColor,
                TargetWidth = targetWidth
            };

            var handleOutline = jobAddOutline.Schedule(totalLength, 64, handlePaintedArea);
            handleOutline.Complete();

            paintedArea.Dispose();

            onComplete?.Invoke(inputPixels);
        }

        [BurstCompile]
        private struct GetPaintedArea : IJobParallelFor
        {
            [ReadOnly]
            public NativeArray<Color> inputPixels;

            [WriteOnly]
            public NativeArray<bool> paintedArea;

            public void Execute(int index)
            {
                paintedArea[index] = inputPixels[index].a > 0f;
            }
        }

        [BurstCompile]
        private struct AddOutlineJob : IJobParallelFor
        {
            [NativeDisableParallelForRestriction]
            [WriteOnly]
            public NativeArray<Color> OutputPixels;

            [NativeDisableParallelForRestriction]
            [ReadOnly]
            public NativeArray<bool> PaintedArea;

            public EOutLineOption OutLineOption;
            public Color OutLineColor;
            public int TargetWidth;

            public void Execute(int index)
            {
                if (!PaintedArea[index]) return;

                switch (OutLineOption)
                {
                    case EOutLineOption.Cross:
                        PaintUp(index);
                        PaintDown(index);
                        PaintLeft(index);
                        PaintRight(index);
                        break;
                    case EOutLineOption.Vertical:
                        PaintUp(index);
                        PaintDown(index);
                        break;
                    case EOutLineOption.Horizontal:
                        PaintLeft(index);
                        PaintRight(index);
                        break;
                }
            }


            private void PaintDown(int index)
            {
                var bottomSideIndex = index - TargetWidth;
                // 아래쪽 체크
                if (bottomSideIndex < 0) return;
                if (PaintedArea[bottomSideIndex]) return;

                OutputPixels[bottomSideIndex] = OutLineColor;
            }

            private void PaintUp(int index)
            {
                var upSideIndex = index + TargetWidth;
                // 아래쪽 체크
                if (PaintedArea.Length <= upSideIndex) return;
                if (PaintedArea[upSideIndex]) return;

                OutputPixels[upSideIndex] = OutLineColor;
            }

            private void PaintLeft(int index)
            {
                var leftSideIndex = index - 1;
                // 왼쪽 체크
                if (leftSideIndex < 0) return;

                if (PaintedArea[leftSideIndex]) return;

                if (index % TargetWidth == 0) return;

                OutputPixels[leftSideIndex] = OutLineColor;
            }

            private void PaintRight(int index)
            {
                var rightSideIndex = index + 1;
                // 오른쪽 체크
                if (PaintedArea.Length <= rightSideIndex) return;

                if (PaintedArea[rightSideIndex]) return;

                if ((index + 1) % TargetWidth == 0) return;

                OutputPixels[rightSideIndex] = OutLineColor;
            }
        }
    }
}