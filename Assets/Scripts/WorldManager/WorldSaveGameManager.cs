using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSaveGameManager : MonoBehaviour
{
    public static WorldSaveGameManager Instance { get; private set; }

    [SerializeField] private PlayerManager player;
    [Header("Save/Load Test")]
    public bool saveGame;
    public bool loadGame;

    [Header("World Scene Index")]
    [SerializeField] private int worldSceneIndex = 1;
    [Header("Target Frame Rate")]
    [SerializeField] private int targetFrameRate = 60;

    [Header("Save file data writer")]
    public SaveFileDataWriter saveFileDataWriter;

    [Header("Current character save data")]
    public int currentCharacterSlotIndex;
    public string saveFileName;
    public CharacterSaveData currentCharacterSaveData;

    [Header("Character Slots")]
    public CharacterSaveData[] characterSlots = new CharacterSaveData[10];


    private void Start()
    {
        Application.targetFrameRate = targetFrameRate;

    }

    private void Update()
    {
        if (saveGame)
        {
            saveGame = false;
            SaveGame();

        }
        if (loadGame)
        {
            loadGame = false;
            LoadGame();

        }
    }


    public void DecideCharacterSaveFileNameBaseOnSlotIndex()
    {
        saveFileName = $"characterSlot0{currentCharacterSlotIndex}";
    }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);

        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);


    }

    public void CreateNewGame()
    {

        DecideCharacterSaveFileNameBaseOnSlotIndex();

        currentCharacterSaveData = new CharacterSaveData();
    }
    public void LoadGame()
    {
        DecideCharacterSaveFileNameBaseOnSlotIndex();

        SaveFileDataWriter saveFileDataWriter = new SaveFileDataWriter();

        saveFileDataWriter.saveFileDirectoryPath = Application.persistentDataPath;
        saveFileDataWriter.saveFileName = saveFileName;

        currentCharacterSaveData = saveFileDataWriter.LoadCharacterSaveFile();
        StartCoroutine(LoadWorldScene());

    }
    public void SaveGame()
    {
        DecideCharacterSaveFileNameBaseOnSlotIndex();

        SaveFileDataWriter saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveFileDirectoryPath = Application.persistentDataPath;
        saveFileDataWriter.saveFileName = saveFileName;

        player.SaveGameFromCurrentCharacterData(ref currentCharacterSaveData);
        saveFileDataWriter.CreateNewCharacterSaveFile(currentCharacterSaveData);
    }

    public IEnumerator LoadWorldScene()
    {
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(worldSceneIndex);
        yield return null;
    }
    public int GetWorldSceneIndex()
    {
        return worldSceneIndex;
    }
}
