using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO KitchenObjectSO;
    }
    
    // 可被添加到盘子中的物品清单
    [SerializeField] private List<KitchenObjectSO> validKitchenObjectList;
    
    // 存储食材列表
    private List<KitchenObjectSO> kitchenObjectList;

    private void Awake()
    {
        kitchenObjectList = new List<KitchenObjectSO>();
    }

    /// <summary>
    /// 给盘子添加成分
    /// </summary>
    /// <param name="kitchenObject"></param>
    public bool TryAddIngreddient(KitchenObjectSO kitchenObjectSO)
    {
        if (!validKitchenObjectList.Contains(kitchenObjectSO))
        {
            // 不是有效的食材
            return false;
        }
        if (kitchenObjectList.Contains(kitchenObjectSO))
        {
            return false;
        }
        else
        {
            kitchenObjectList.Add(kitchenObjectSO);
            // 通知盘子，显示这个食材
            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
            {
                KitchenObjectSO = kitchenObjectSO
            });
            return true;
        }
    }

    public List<KitchenObjectSO> GetKitchenObjectSOList()
    {
        return kitchenObjectList;
    }
}
