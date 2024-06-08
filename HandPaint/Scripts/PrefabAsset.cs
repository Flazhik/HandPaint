using System;

namespace HandPaint.Scripts
{
    [AttributeUsage(AttributeTargets.Field)]
    public class PrefabAsset : Attribute
    {
        public string Path { get; }

        public PrefabAsset(string path = "")
        {
            Path = path;
        }
    }
}