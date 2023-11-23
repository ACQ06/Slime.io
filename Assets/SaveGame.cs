using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveGame
{
   public static void SaveGameData(Player player, SwordAttack swordAttack)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.battery";
        FileStream stream = new FileStream(path, FileMode.Create);

        GameData data = new GameData(player, swordAttack);
            
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static GameData LoadGameData()
    {
        string path = Application.persistentDataPath + "/player.battery";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameData data = formatter.Deserialize(stream) as GameData;
            stream.Close();

            return data;
        }

        else
        {
            Debug.LogError("No Game Data");
            return null;
        }
    }
}
