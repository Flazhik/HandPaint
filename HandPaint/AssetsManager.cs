using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HandPaint.Scripts;
using UnityEngine;

namespace HandPaint
{
    public class AssetsManager : MonoSingleton<AssetsManager>
    {
        private const BindingFlags Flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
        private static readonly Dictionary<string, Object> Prefabs = new Dictionary<string, Object>();
        private AssetBundle _bundle;

        public void LoadAssets()
        {
            _bundle = AssetBundle.LoadFromMemory(Resources.HandPaint);
        }

        public void RegisterPrefabs()
        {
            foreach (var assetName in _bundle.GetAllAssetNames())
                Prefabs.Add(assetName, _bundle.LoadAsset<Object>(assetName));

            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
                CheckType(type);
        }
        
        private static void CheckType(IReflect type)
        {
            type.GetFields(Flags)
                .ToList()
                .ForEach(ProcessField);
        }

        private static void ProcessField(FieldInfo field)
        {
            if (field.FieldType.IsArray
                || !field.IsStatic)
                return;

            var assetTag = field.GetCustomAttribute<PrefabAsset>();
            if (assetTag == null)
                return;

            field.SetValue(null, Prefabs[assetTag.Path]);
        }
        
        public static Object GetAsset(string assetName)
        {
            return Prefabs[assetName];
        }
    }
}