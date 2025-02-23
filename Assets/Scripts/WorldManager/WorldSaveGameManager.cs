using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSaveGameManager : MonoBehaviour
{
    public static WorldSaveGameManager Instance { get; private set; }
    [SerializeField] private int worldSceneIndex = 1;
    [SerializeField] private int targetFrameRate = 60;

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
    private void Start()
    {
        Application.targetFrameRate = targetFrameRate;
    }

    public IEnumerator LoadNewGame()
    {
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(worldSceneIndex);
        yield return null;
    }
    public int GetWorldSceneIndex()
    {
        return worldSceneIndex;
    }
}
