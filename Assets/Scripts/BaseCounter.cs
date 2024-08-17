using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private Transform counterTopPoint;

    private KitchenObject kitchenObject;
    
    /// <summary>
    /// 与柜台交互
    /// </summary>
    public virtual void Interact(Player player) {
        Debug.LogError("BaseCounter.Interact()");
    }
    
    // 获取物品对象的位置
    public Transform GettKitchenObjectFollowTransform(){
        return counterTopPoint;
    }

    // 设置物品对象
    public void SetKitchenObject(KitchenObject kitchenObject){
        this.kitchenObject = kitchenObject;
    }

    // 获取物品对象
    public KitchenObject GetKitchenObject(){
        return this.kitchenObject;
    }

    // 移除物品对象
    public void ClearKitchenObject(){
        this.kitchenObject = null;
    }
    
    // 检测kitchenObject是否为空
    public bool HasKitchenObject() {
        return kitchenObject != null;
    }
}
