using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using GeneratorClass;

public static class SaveSystem
{
    public static void SaveLevel(TileGenerationV2 Instance)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/Level.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        LevelData data = new LevelData(Instance);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static LevelData LoadLevel()
    {
        string path = Application.persistentDataPath + "/Level.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            LevelData data = formatter.Deserialize(stream) as LevelData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file is not found in " + path);
            return null;
        }
    }
}
