using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer(Dictionary<string, object> values, string currentScene)
    {
        string path = Application.persistentDataPath + "/player.dat";

        Dictionary<string, object> existingData = new Dictionary<string, object>();

        if (File.Exists(path))
        {
            existingData = LoadPlayer();
            Debug.Log("Existing player data loaded for update:");
            Debug.Log("Previous Values:");

            foreach (var kvp in existingData)
            {
                Debug.Log($"{kvp.Key}: {kvp.Value}");
            }
        }

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        // Update existing data with new values
        foreach (var kvp in values)
        {
            existingData[kvp.Key] = kvp.Value;
        }

        // Add current scene to the dictionary
        existingData["currentScene"] = currentScene;

        Debug.Log("Updated Values:");

        foreach (var kvp in existingData)
        {
            Debug.Log($"{kvp.Key}: {kvp.Value}");
        }

        formatter.Serialize(stream, existingData);
        stream.Close();

        Debug.Log("Player data saved to: " + path);
    }

    public static Dictionary<string, object> LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.dat";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            Dictionary<string, object> data = formatter.Deserialize(stream) as Dictionary<string, object>;
            stream.Close();

            Debug.Log("Player data loaded from: " + path);
            Debug.Log("Loaded Values:");

            foreach (var kvp in data)
            {
                Debug.Log($"{kvp.Key}: {kvp.Value}");
            }

            return data;
        }
        else
        {
            Debug.Log("Save File Not Found in " + path);
            return null;
        }
    }
}