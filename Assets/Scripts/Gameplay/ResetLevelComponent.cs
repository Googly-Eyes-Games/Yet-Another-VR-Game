using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetLevelComponent : MonoBehaviour
{
    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
