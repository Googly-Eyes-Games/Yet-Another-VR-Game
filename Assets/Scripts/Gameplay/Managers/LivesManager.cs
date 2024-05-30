using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LivesManager : MonoBehaviour
{
    public event Action<int> OnLivesNumberChanged;
    
    [field: SerializeField]
    public int availableLives { get; private set; }

    [Scene, SerializeField]
    private string gameOverScene;

    [field: SerializeField]
    private BrokenMugsDetector brokenMugsDetector;

    public void OnEnable()
    {
        brokenMugsDetector.OnMugBroken += HandleBrokenMug;
    }

    public void OnDisable()
    {
        brokenMugsDetector.OnMugBroken -= HandleBrokenMug;
    }

    private void HandleBrokenMug()
    {
        availableLives--;
        OnLivesNumberChanged?.Invoke(availableLives);

        if (availableLives < 0)
        {
            SceneManager.LoadSceneAsync(gameOverScene);
        }
    }
}