using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour{
    // 单例模式
    public static Player Instance { get; private set; }

    // 是否高亮柜台事件
    public event EventHandler<OnSelectionChangedEventArgs> OnSelectedCounterChanged;
    // 高亮柜台事件的数据信息类
    public class OnSelectionChangedEventArgs: EventArgs {
        public ClearCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private GameInput gameInput;
    private const string COUNTERS_LAYER_NAME = "Counters";
    // 记录是否在进行移动
    private bool isWalking;
    // 记录是否最后的移动方向
    private Vector3 lastInteratDir;
    private ClearCounter selectedCounter;

    private void Awake() {
        if(Instance == null){
            Instance = this;
        }else{
            if(Instance != this){
                Destroy(gameObject);
            }
        }
        DontDestroyOnLoad(gameObject);
        // 下面是视频中使用的单例模式
        // if(Instance != null){
        //     Debug.LogError("Player already exists!");
        // }
        // Instance = this;
    }

    private void Start() {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    // 交互启动事件处理器，进行交互逻辑处理
    private void GameInput_OnInteractAction(object sender, EventArgs e) {
        // 如果检测到的可交互对象不为空，即调用该对象的交互逻辑
        if(selectedCounter != null){
            selectedCounter.Interact();
        }
    }

    private void Update() {
        HandleMovement();
        HandleInteraction();
    }

    public bool IsWalking(){
        return isWalking;
    }

    private void HandleInteraction(){
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        // 设置移动的方向
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        // 记录最后的移动朝向
        if(moveDir != Vector3.zero){
            lastInteratDir = moveDir;
        }

        float interactDistance = 2f;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, lastInteratDir, out hit, interactDistance, 1 << LayerMask.NameToLayer("Counters"))){
            Debug.Log("检测到柜台");
            // 尝试获取到 ClearCounter 组件，并且会判断是否为空
            if(hit.transform.TryGetComponent(out ClearCounter clearCounter)){
                if(this.selectedCounter != clearCounter){
                    SetSelectedCounter(clearCounter);
                }
            }else{
                // 获取不到 ClearCounter 组件，也置空
                SetSelectedCounter(null);
            }
        }else{
            // 如果什么都没有检测到
            SetSelectedCounter(null);
        }
    }

    private void HandleMovement(){
        
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

    // 记录传递的对象，并且启动高亮显示柜台事件
    private void SetSelectedCounter(ClearCounter counter){
        this.selectedCounter = counter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectionChangedEventArgs{
            selectedCounter = counter
        });
    }

}
