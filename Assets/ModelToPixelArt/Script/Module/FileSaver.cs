using System;
using System.IO;
using UnityEngine;

namespace ModelToPixelArt.Module
{
    public static class FileSaver
    {
        public static void OpenFolder()
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = Application.persistentDataPath,
                    UseShellExecute = true,
                    Verb = "open"
                });
            }
            catch (Exception e)
            {
                Debug.LogError($"경로 열기 실패: {e.Message}");
            }
        }

        public static void SaveFileInPersistentDataPath(string fileName, byte[] data, Action onComplete = null)
        {
            var filePath = Path.Combine(Application.persistentDataPath, fileName);
            File.WriteAllBytes(filePath, data);
            onComplete?.Invoke();
        }
    }
}