using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private Button resumeButton;

    [SerializeField]
    private Button leaveButton;

    [SerializeField]
    private GameObject canvas;
    
    [SerializeField]
    private InputActionReference pauseInputAction;

    private bool isPaused = false;

    void OnEnable()
    {
        pauseInputAction.action.performed += HandlePauseInput;
        
        resumeButton.onClick.AddListener(OnResume);
        leaveButton.onClick.AddListener(OnLeave);
    }

    void OnDisable() 
    {
        pauseInputAction.action.performed -= HandlePauseInput;
        
        resumeButton.onClick.RemoveAllListeners();
        leaveButton.onClick.RemoveAllListeners();
    }

    void HandlePauseInput(InputAction.CallbackContext context)
    {
        if (!isPaused)
        {
            canvas.SetActive(true);
            Time.timeScale = 0.0f;
            isPaused = true;
        }
        else
        {
            OnResume();
        }
    }

    void OnResume()
    {
        canvas.SetActive(false);
        Time.timeScale = 1.0f;
        isPaused = false;
    }

    void OnLeave()
    {
        Time.timeScale = 1.0f;
        TransitionsSceneManger.Get().LoadMenu();
    }
}
