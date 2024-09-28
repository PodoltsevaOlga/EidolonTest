using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Utilities;

namespace SaveLoadSystem
{
    public class SaveDataManager<T> where T : ISavableData
    {
        private readonly string FileName;
        
        public SaveDataManager(string fileName)
        {
            FileName = fileName;
        }
        
        public bool SaveEvents(List<T> data)
        {
            if (data == null || data.Count == 0)
            {
                return true;
            }
            
            string filePath = FileUtilities.GetDataFilePath(FileName);
            try
            {
                if (!File.Exists(filePath))
                {
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                    {
                    }
                }

                using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                {
                    using (var binaryWriter = new BinaryWriter(fileStream, Encoding.UTF8, false))
                    {
                        foreach (var d in data)
                        {
                            binaryWriter.Write(d.ConvertToString());
                        }
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error while writing events to file: {e}");
                return false;
            }
        }
    }
}