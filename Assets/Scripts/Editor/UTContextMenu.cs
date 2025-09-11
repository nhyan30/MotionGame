using System.IO;
using UnityEditor;
using UnityEngine;

public class UTContextMenu : MonoBehaviour
{
    private static string screenshotFolder => Application.dataPath.Replace("/Assets", "") + "/Screenshots";

    [MenuItem("GloTools/Screenshot", false, 50)]
    public static void Screenshot()
    {
        CheckAndCreateDirectory(screenshotFolder);
        string[] files = Directory.GetFiles(screenshotFolder);

        string index = files.Length > 0 ? "(" + files.Length + ")" : "";

        ScreenCapture.CaptureScreenshot(screenshotFolder + $"/screenshot{index}.png");
    }
    public static bool CheckAndCreateDirectory(string path)
    {
        bool DirCreated = false;

        path = PlatformSpecificPath(path);

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            DirCreated = true;
        }

        return DirCreated;
    }
    public static string PlatformSpecificPath(string path)
    {
#if PLATFORM_ANDROID && !UNITY_EDITOR
            return path.Replace(@"\", "/");
#else
        return path;
#endif
    }

    [MenuItem("GloTools/Folder/ScreenshotFolder", false, 0)]
    public static void OpenScreenshotFolder()
    {
        System.Diagnostics.Process.Start(screenshotFolder);
    }
}
