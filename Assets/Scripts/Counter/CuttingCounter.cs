using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    // 相当于字典，存储所有处理前和处理后的食材数据
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;
    
    /// <summary>
    /// 与柜台交互，拿取
    /// </summary>
    /// <param name="player"></param>
    public override void Interact(Player player) {
        if (!this.HasKitchenObject()) {
            // 柜台本身没有物品，就可以放置物品
            if (player.HasKitchenObject()) {
                // 如果玩家拥有的食材可被处理，才可放置在柜台上
                if(HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
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
        
        if (this.HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO())) {
            // 只有柜台有食材后，并且该食材能够被处理，才能进行处理
            // 得到处理后的食材数据
            KitchenObjectSO outputKitchenObject = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
            GetKitchenObject().DestroySelf();
            // 生成食材，并且设置其拥有者
            KitchenObject.SpawnKitchenObject(outputKitchenObject, this);
        }
    }

    // 判断食材能否处理
    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
                return true;
        }
        return false;
    }
    
    // 返回处理后的食材
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
                return cuttingRecipeSO.output;
        }
        return null;
    }
}
