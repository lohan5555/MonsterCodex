using UnityEngine;

public static class MonsterDataLoader
{
    public static MonsterDataList LoadData(string fileName)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>($"{fileName}");
        if (jsonFile == null)
        {
            Debug.LogError($"Impossible de charger {fileName}");
            return null;
        }
        var result = JsonUtility.FromJson<MonsterDataList>(jsonFile.text);
        return result;
    }
}


[System.Serializable]
public class MonsterEntry
{
    public int id;
    public string text;
}

[System.Serializable]
public class MonsterDataList
{
    public MonsterEntry[] data;
}
