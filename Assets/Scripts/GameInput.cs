using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour {
    
    private PlayerInputActions playerInputActions;

    private void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }

    public Vector2 GetMovementVectorNormalized() {
        // 读取玩家输入
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        inputVector.Normalize();
        
        Debug.Log(inputVector);

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
