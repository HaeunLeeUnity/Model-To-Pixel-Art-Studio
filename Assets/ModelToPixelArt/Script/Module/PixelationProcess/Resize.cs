using System;
using ModelToPixelArt.Interface;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace ModelToPixelArt.Module.PixelationProcess
{
    public class Resize : IPixelationProcess
    {
        private int _inputWidth;
        private int _inputHeight;
        
        public Resize(int inputWidth, int inputHeight)
        {
            _inputWidth = inputWidth;
            _inputHeight = inputHeight;
        }
        
        public void Execute(NativeArray<Color> inputPixels, int targetWidth, int targetHeight, Action<NativeArray<Color>> onComplete)
        {
            var outputPixelNativeArray = new NativeArray<Color>(targetWidth * targetHeight, Allocator.TempJob);
            
            var resizeJob = new ResizeJob
            {
                InputPixels = inputPixels,
                OutputPixels = outputPixelNativeArray,

                InputWidth = _inputWidth,
                TargetWidth   = targetWidth,
                TargetHeight  = targetHeight,

                XRatio = (float)_inputWidth / targetWidth,
                YRatio = (float)_inputHeight / targetHeight
            };
            
            var handle = resizeJob.Schedule(targetWidth * targetHeight, 64);
            handle.Complete();
            
            inputPixels.Dispose();
            
            onComplete?.Invoke(outputPixelNativeArray);
        }
    }
    
    
    

    [BurstCompile]
    public struct ResizeJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<Color> InputPixels;
        [WriteOnly] public NativeArray<Color> OutputPixels;

        public int InputWidth;
        public int TargetWidth;
        public int TargetHeight;

        public float XRatio;
        public float YRatio;

        public void Execute(int index)
        {
            var x = index % TargetWidth;
            var y = index / TargetWidth;
            
            var origX = Mathf.FloorToInt(x * XRatio);
            var origY = Mathf.FloorToInt(y * YRatio);

            var origIndex = origY * InputWidth + origX;
            OutputPixels[index] = InputPixels[origIndex];
        }
    }

}