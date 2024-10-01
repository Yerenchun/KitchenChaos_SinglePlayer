using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter{

    // 只是临时存取物品的柜台，不需要知道物品的具体数据
    // [SerializeField] private KitchenObjectSO kitchenObjectSO;

    /// <summary>
    /// 与普通柜台交互，存取物品
    /// </summary>
    public override void Interact(Player player){
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
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // 当玩家拿着一个盘子的时候
                    if (plateKitchenObject.TryAddIngreddient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
                else
                {
                    // 如果玩家没有拿盘子，拿着其他东西
                    if (GetKitchenObject().TryGetPlate(out  plateKitchenObject))
                    {
                        // 柜台上面放着盘子，就可以尝试让盘子装东西
                        if (plateKitchenObject.TryAddIngreddient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            }   
            else {
                // 如果柜台有东西，玩家没有拿着东西，才能拿取柜台的物品
                this.GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
    
}
