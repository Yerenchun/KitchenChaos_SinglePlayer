using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour{

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private GameInput gameInput;
    private bool isWalking;

    private void Update() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        // 设置移动的方向
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        // 使用 CapsuleCast来检测前方是否有障碍物，检测的距离是 即将移动的距离
        bool canMove = !Physics.CapsuleCast(transform.position, 
                        transform.position + Vector3.up * playerHeight, 
                        playerRadius, moveDir, moveDistance);
        // TODO 这种移动方式，仍有不妥，在方体的拐角会有卡主的情况
        if(!canMove) {
            // 如果不能斜着向左或者向右移动
            
            // 判断一下能否朝水平移动
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight,
                                        playerRadius, moveDirX, moveDistance);
            if(canMove) {
                // 只能朝水平移动
                moveDir = moveDirX;
            }else{
                // 不能朝水平移动

                // 尝试在垂直方向上移动
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight,
                                        playerRadius, moveDirZ, moveDistance);
                if(canMove) {
                    // 只能朝垂直方向移动
                    moveDir = moveDirZ;
                }else{
                    // 否则任何方向都不能移动过去
                }
            }
        }
        if(canMove) {
            transform.position += moveDir * moveDistance;
        }

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
