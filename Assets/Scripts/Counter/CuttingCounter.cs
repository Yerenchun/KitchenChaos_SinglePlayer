using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO cutKitchenObjectSO;
    
    /// <summary>
    /// 与柜台交互，拿取
    /// </summary>
    /// <param name="player"></param>
    public override void Interact(Player player) {
        if (!this.HasKitchenObject()) {
            // 柜台本身没有物品，就可以放置物品
            if (player.HasKitchenObject()) {
                // 如果玩家拥有物品，才可放置在柜台上
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }else {
                // 如果玩家没有物品，柜台也没有物品，就不能与柜台进行交互
            }
        }else {
            // 柜台已经有物品，就不能放置物品
            if (player.HasKitchenObject()) {
                // 如果玩家拿着某个物品
            }   
            else {
                // 如果柜台有东西，玩家没有拿着东西，才能拿取柜台的物品
                this.GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
    
    /// <summary>
    /// 与柜台交互，切菜
    /// </summary>
    /// <param name="player"></param>
    public override void InteractAlternate(Player player) {
        if (this.HasKitchenObject()) {
            // 柜台本身有食材，就可以处理食材
            GetKitchenObject().DestroySelf();
            // 生成食材，并且设置其拥有者
            KitchenObject.SpawnKitchenObject(cutKitchenObjectSO, this);
        }
    }
}
