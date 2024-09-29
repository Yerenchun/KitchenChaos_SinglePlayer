using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SelectedCounterVisual : MonoBehaviour {
    // 对于Counter的引用
    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] visualGameObjects;


    // 如果将下面这句代码，放在Awake方法中，那么可能Player还没有初始化，就会报空引用异常
    // 这是因为，代码时序的问题，各个物体的Awake方法执行的优先级是按照加载顺序来调用的
    private void Start(){
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectionChangedEventArgs e) {
        // 如果当前选中的Counter是Counter，则显示，否则隐藏
        if(e.selectedCounter == baseCounter){
            Show();
        }else{
            Hide();
        }
    }

    private void Show(){
        for (int i = 0; i < visualGameObjects.Length; i++)
        {
            visualGameObjects[i].SetActive(true);
        }
    }

    private void Hide(){
        for (int i = 0; i < visualGameObjects.Length; i++)
        {
            visualGameObjects[i].SetActive(false);
        }
    }
}
