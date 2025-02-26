using System;
using Unity.Collections;
using UnityEngine;

namespace ModelToPixelArt.Interface
{
    public interface IPixelationProcess
    {
        public void Execute(NativeArray<Color> inputPixels, int targetWidth, int targetHeight, Action<NativeArray<Color>> onComplete);
    }
}