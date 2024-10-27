using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum SceneType
    {
        MainMenuScene,
        GameScene,
        LoadingScene
    }
    
    // private static SceneType targetScene;
    private static Action onLoaderCallback;
    
    /// <summary>
    /// 加载过渡场景
    /// </summary>
    /// <param name="targetScene"></param>
    public static void Load(SceneType targetScene)
    {
        // Loader.targetScene = targetScene;   
        SceneManager.LoadScene(SceneType.LoadingScene.ToString());

        onLoaderCallback = () =>
        {
            SceneManager.LoadScene(targetScene.ToString());
        };
    }

    /// <summary>
    /// 真正开始加载游戏场景
    /// </summary>
    /// <param></param>
    public static void LoaderCallback()
    {
        // SceneManager.LoadScene(targetScene.ToString());
        onLoaderCallback?.Invoke();
        onLoaderCallback = null;
    }
}
