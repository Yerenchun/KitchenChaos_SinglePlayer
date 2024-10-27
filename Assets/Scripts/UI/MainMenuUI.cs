using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    
    private void Start()
    {
        playButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.SceneType.GameScene);
        });
        
        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}
