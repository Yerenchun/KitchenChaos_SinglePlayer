using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image barImage;

    private IHasProgress hasProgress;
    
    private void Start()
    {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        if (hasProgress == null)
        {
            Debug.LogError($"物体 {hasProgressGameObject} 没有组件实现了IHasProgress接口！");
        }
        hasProgress.OnProgressChanged += HasProgressOnProgressChanged;
        barImage.fillAmount = 0f;
        
        Hide();
    }

    private void HasProgressOnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        barImage.fillAmount = e.progressNormalized;

        if (barImage.fillAmount == 0 || barImage.fillAmount == 1f)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
