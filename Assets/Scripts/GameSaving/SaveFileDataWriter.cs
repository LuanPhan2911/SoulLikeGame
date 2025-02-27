using System.IO;
using UnityEngine;

public class SaveFileDataWriter
{
    public string saveFileName = "";
    public string saveFileDirectoryPath = "";


    // before save file, check to see if it exist one of this character slot already exist (max character slot 10)
    public bool CheckToSeeIfExist()
    {
        if (File.Exists(Path.Combine(saveFileDirectoryPath, saveFileName)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void DeleteSaveFile()
    {
        File.Delete(Path.Combine(saveFileDirectoryPath, saveFileName));
    }

    // use to create save file upon starting new game
    public void CreateNewCharacterSaveFile(CharacterSaveData characterSaveData)
    {
        string savePath = Path.Combine(saveFileDirectoryPath, saveFileName);
        try
        {
            // create direction if not exist
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            Debug.Log("File save at " + savePath);
            string dataToStore = JsonUtility.ToJson(characterSaveData);

            using (FileStream fs = new FileStream(savePath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error saving file: " + e.Message);
        }
    }

    // use to load save file upon loading game
    public CharacterSaveData LoadCharacterSaveFile()
    {
        string loadPath = Path.Combine(saveFileDirectoryPath, saveFileName);
        try
        {
            if (File.Exists(loadPath))
            {
                using (FileStream fs = new FileStream(loadPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(fs))
                    {
                        string data = reader.ReadToEnd();
                        return JsonUtility.FromJson<CharacterSaveData>(data);
                    }
                }
            }
            else
            {
                Debug.LogError("File not found: " + loadPath);
                return null;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error loading file: " + e.Message);
            return null;
        }
    }
}
