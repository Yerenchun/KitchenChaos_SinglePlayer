using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour{

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotateSpeed = 10f;
    private bool isWalking;

    private void Update() {
        Vector2 inputVector = Vector2.zero;

        if(Input.GetKey(KeyCode.W)){
            inputVector.y = +1;
        }
        if(Input.GetKey(KeyCode.A)){
            inputVector.x = -1;
        }
        if(Input.GetKey(KeyCode.S)){
            inputVector.y = -1;
        }
        if(Input.GetKey(KeyCode.D)){
            inputVector.x = +1;
        }
        inputVector.Normalize();

        // 设置移动的方向
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        transform.position += moveDir * Time.deltaTime * moveSpeed;

        isWalking = moveDir != Vector3.zero;

        // 通过设置自己的面朝向，来设置自己的旋转方向
        // transform.forward = moveDir;
        // 通过插值函数，使旋转更加平滑
        transform.forward = Vector3.Slerp(transform.forward, moveDir, rotateSpeed * Time.deltaTime);
    }

    public bool IsWalking(){
        return isWalking;
    }

}
