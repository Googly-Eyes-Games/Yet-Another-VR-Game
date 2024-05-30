using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    [Scene, SerializeField]
    private string scenePath;

    public void LoadLevel()
    {
        SceneManager.LoadSceneAsync(scenePath);
    }
}
