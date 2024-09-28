using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Utilities;

namespace SaveLoadSystem
{
    public class LoadDataManager<T> where T : ILoadableData, new()
    {
        private readonly string FileName;
        
        public LoadDataManager(string fileName)
        {
            FileName = fileName;
        }
        
        public List<T> LoadData()
        {
            var data = new List<T>();
            string filePath = FileUtilities.GetDataFilePath(FileName);
            
            try
            {
                if (!File.Exists(filePath))
                {
                    Debug.LogWarning($"Can't read data. File {filePath} doesn't exist");
                    return data;
                }

                using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                {
                    using (var binaryReader = new BinaryReader(fileStream, Encoding.UTF8, false))
                    {
                        while (binaryReader.BaseStream.Position != binaryReader.BaseStream.Length)
                        {
                            var dataEntry = binaryReader.ReadString();
                            var newDataEntry = new T();

                            try
                            {
                                newDataEntry.CreateFromString(dataEntry);
                                data.Add(newDataEntry);
                            }
                            catch (ArgumentException e)
                            {
                                Console.WriteLine(e);
                            }
                            catch (Exception e)
                            {
                                throw e;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error while reading data from file {filePath}: {e}");
            }
            
            return data;
        }
    }
}