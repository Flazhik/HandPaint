using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateAssetBundle : MonoBehaviour
{
    [MenuItem("AssetsBundle/Build AssetBundles")]
    static void BuildAllAssetBundles () {
        string dir = "C:\\Users\\User\\RiderProjects\\HandPaint\\HandPaint\\Resources";
        foreach (var file in new DirectoryInfo(dir).EnumerateFiles())
        {
            file.Delete();
        }
        
        BuildPipeline.BuildAssetBundles(dir, BuildAssetBundleOptions.UncompressedAssetBundle,BuildTarget.StandaloneWindows64);
        new FileInfo(Path.Combine(dir, "handpaint")).MoveTo(Path.Combine(dir, "HandPaint.resource"));
    }
}
