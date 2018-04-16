using UnityEditor;
using UnityEditor.Build;

public class BuildSettings : IPreprocessBuild
{
    public int callbackOrder
    {
        get
        {
            return 0;
        }
    }

    public void OnPreprocessBuild(BuildTarget target, string path)
    {
        float currentVersion = float.Parse(PlayerSettings.bundleVersion);
        currentVersion += 0.01f;
        PlayerSettings.bundleVersion = currentVersion.ToString();
    }
}
