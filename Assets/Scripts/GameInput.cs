using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;

public class GameInput : MonoBehaviour {
    // 使用c#预定义的EventHandler委托类型，来作为事件的参数类型
    public event EventHandler OnInteractAction;
    private PlayerInputActions playerInputActions;

    private void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += Interact_Performed;
    }

    private void Interact_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        // Debug.Log("按下交互按钮");
        // 触发交互事件
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized() {
        // 读取玩家输入
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        inputVector.Normalize();
        
        // Debug.Log(inputVector);

        return inputVector;


        // if(Input.GetKey(KeyCode.W)){
        //     inputVector.y = +1;
        // }
        // if(Input.GetKey(KeyCode.A)){
        //     inputVector.x = -1;
        // }
        // if(Input.GetKey(KeyCode.S)){
        //     inputVector.y = -1;
        // }
        // if(Input.GetKey(KeyCode.D)){
        //     inputVector.x = +1;
        // }
        // inputVector.Normalize();
        // return inputVector;
    }
}
