using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : MonoBehaviour, IKitchenObjectParent{

    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] private Transform counterTopPoint;

    private KitchenObject kitchenObject;

    /// <summary>
    /// 与柜台交互，创建一个柜台拥有的物品
    /// </summary>
    public void Interact(Player player){
        // 避免无限实例化
        if(kitchenObject == null){
            // 创建一个物品，并且设置为counterTopPoint的子对象
            Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab, counterTopPoint);
            kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(this);
            
        }else{
            // 让玩家可以拿着物品
            kitchenObject.SetKitchenObjectParent(player);
            // Debug.Log(kitchenObject.GetKitchenObjectParent());
        }
    }
    #region 实现物品交互
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
    #endregion
    
}
