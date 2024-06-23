using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button leaveButton;
    [SerializeField] private GameObject canvas;

    void OnEnable()
    {
        resumeButton.onClick.AddListener(OnResume);
        restartButton.onClick.AddListener(OnRestart);
        leaveButton.onClick.AddListener(OnLeave);

        InputActionAsset inputActionAsset = Resources.Load<InputActionAsset>("XRI Default Input Actions");
        inputActionAsset.FindAction("PauseMenu").performed += OnPause;
    }

    void OnDisable() 
    {
        resumeButton.onClick.RemoveAllListeners();
        restartButton.onClick.RemoveAllListeners();
        leaveButton.onClick.RemoveAllListeners();

        InputActionAsset inputActionAsset = Resources.Load<InputActionAsset>("XRI Default Input Actions");
        inputActionAsset.FindAction("PauseMenu").performed -= OnPause;    
    }

    void OnPause(InputAction.CallbackContext context)
    {
        Time.timeScale = 0.0f;
        canvas.SetActive(true);
    }

    void OnResume()
    {
        Time.timeScale = 1.0f;
        canvas.SetActive(false);
    }

    void OnRestart()
    {
        Time.timeScale = 1.0f;
        TransitionsSceneManger.Get().LoadScene("S_TestMap");
    }

    void OnLeave()
    {
        Time.timeScale = 1.0f;
        TransitionsSceneManger.Get().LoadScene("S_MainMenu");
    }
}
