using System;
using System.IO;
using UnityEngine;

namespace Utilities
{
    public static class FileUtilities
    {
        public static string GetDataFilePath(string fileName)
        {
            return Application.persistentDataPath + $"/{fileName}";
        }

        public static void ClearFile(string fileName)
        {
            string filePath = GetDataFilePath(fileName);
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Can't clear file {filePath} doesn't exist");
                return;
            }

            using (FileStream fileStream = new FileStream(filePath, FileMode.Truncate))
            {
            }
        }
    }
}