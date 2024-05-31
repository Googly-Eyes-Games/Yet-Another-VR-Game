using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public void LoadLevel()
    {
        SceneManager.LoadSceneAsync(GameplaySettings.Global.LevelScene);
    }
}
