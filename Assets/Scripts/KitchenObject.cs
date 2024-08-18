using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    // 存储其父对象，即其所属的柜台
    private IKitchenObjectParent kitchenObjectParent;

    // 获取厨房物品的SO，实际上就是获取数据文件
    public KitchenObjectSO GetKitchenObjectSO() {
        return kitchenObjectSO;
    }
    
    /// <summary>
    /// 设置父对象
    /// </summary>
    /// <param name="clearCounter">该物品属于哪个柜台</param>
    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent){
        // 通知旧的父对象移除自己
        if(this.kitchenObjectParent != null){
            this.kitchenObjectParent.ClearKitchenObject();
        }

        // 逻辑上，设置其父对象
        this.kitchenObjectParent = kitchenObjectParent;

        // 检测新的父对象是否已经装了东西了
        if(kitchenObjectParent.GetKitchenObject()){
            Debug.LogError("CounterParent already has a KitchenObject");
        }

        // 通知新的父对象拥有自己
        kitchenObjectParent.SetKitchenObject(this);

        // 视觉上设置父对象，让模型现在到对应位置
        transform.parent = kitchenObjectParent.GettKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    /// <summary>
    /// 返回该物品的父对象
    /// </summary>
    /// <returns></returns>
    public IKitchenObjectParent GetKitchenObjectParent(){
        return kitchenObjectParent;
    }

    public void DestroySelf() {
        // 清除父物体的拥有关系
        kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }

    /// <summary>
    /// 生成KitchenObject，并且设置父拥有者
    /// </summary>
    /// <param name="kitchenObjectSO">需要被生成的物品数据</param>
    /// <param name="kitchenObjectParent">生成后的父拥有者</param>
    /// <returns></returns>
    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent) {
        // 创建一个物品，
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
        // 并且设置为counterTopPoint的子对象，马上设置给柜台拿着
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        // 设置父拥有者
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);
        return kitchenObject;
    }
}
