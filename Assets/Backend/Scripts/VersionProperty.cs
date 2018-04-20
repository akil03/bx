[System.Serializable]
public class Version
{
    public string version;
}

[System.Serializable]
public class ScriptData
{
    public Version version;
}

[System.Serializable]
public class VersionProperty
{
    public ScriptData scriptData;
}