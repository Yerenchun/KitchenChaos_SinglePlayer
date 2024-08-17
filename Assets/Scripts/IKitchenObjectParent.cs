using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKitchenObjectParent {
    /// <summary>
    /// 获取物品对象的位置
    /// </summary>
    /// <returns></returns>
    public Transform GettKitchenObjectFollowTransform();

    /// <summary>
    /// 设置物品对象
    /// </summary>
    /// <param name="kitchenObject"></param>
    public void SetKitchenObject(KitchenObject kitchenObject);

    /// <summary>
    /// 获取物品对象
    /// </summary>
    /// <returns></returns>
    public KitchenObject GetKitchenObject();

    /// <summary>
    /// 移除物品对象
    /// </summary>
    public void ClearKitchenObject();

    /// <summary>
    /// 判断是否拥有物品
    /// </summary>
    /// <returns></returns>
    public bool HasKitchenObject();
}
