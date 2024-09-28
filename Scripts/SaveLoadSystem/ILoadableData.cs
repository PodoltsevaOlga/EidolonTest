using System;

namespace SaveLoadSystem
{
    public interface ILoadableData
    {
        public virtual void CreateFromString(string data)
        {
            Console.WriteLine("Not implemented");
        }
    }
}