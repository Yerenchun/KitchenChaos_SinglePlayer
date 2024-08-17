using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabbedObject;
    
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    
    /// <summary>
    /// 与柜台交互，拿取东西
    /// </summary>
    public override void Interact(Player player) {
        // 玩家没有拿着物品时，才能再拿物品
        if (!player.HasKitchenObject())
        {
            // 创建一个物品，并且设置为counterTopPoint的子对象
            Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
            // 马上设置给玩家拿着
            kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(player);
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
    }
    
}