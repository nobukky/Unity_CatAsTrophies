using UnityEngine;
using UnityEngine.SceneManagement;

public class Init : MonoBehaviour
{
    public CatsConfig catConfig;
    public GameSettings gameSettings;
    public PlayerConfig playerConfig;

    private void Start()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(gameSettings.startingScene);
        
        Registry.catConfig = catConfig;
        Registry.gameSettings = gameSettings;
        Registry.playerConfig = playerConfig;

        Registry.isInitialized = true;
        asyncLoad.allowSceneActivation = true;
    }
}