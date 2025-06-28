using UnityEditor;

public class WebGLCompressionConfigurator
{
    [MenuItem("WebGL/Disable Compression (for CI)")]
    public static void DisableCompression()
    {
        PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Disabled;
        PlayerSettings.WebGL.decompressionFallback = false;
        EditorUserBuildSettings.webGLUsePreBuiltUnityEngine = false;
        UnityEngine.Debug.Log("✅ WebGL compression disabled via script.");
    }
}
