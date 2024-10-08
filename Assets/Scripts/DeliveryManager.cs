using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance { get; private set; }
    
    [SerializeField] private RecipeListSO recipeListSO;
    // 订单列表
    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipesMax = 4;
    
    private void Awake()
    {
        // 单例模式
        if(Instance == null){
            Instance = this;
        }else{
            if(Instance != this){
                Destroy(gameObject);
            }
        }
        
        waitingRecipeSOList = new List<RecipeSO>();
    }

    private void Update()
    {
        // 只有在订单列表的数量不够的时候，才会加入新的订单
        if (waitingRecipeSOList.Count < waitingRecipesMax)
        {
            spawnRecipeTimer -= Time.deltaTime;
            if (spawnRecipeTimer <= 0)
            {
                spawnRecipeTimer = spawnRecipeTimerMax;
                // 从食谱列表中，随机得到一份食谱
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[Random.Range(0, recipeListSO.recipeSOList.Count)];
                Debug.Log(waitingRecipeSO.recipeName);
                waitingRecipeSOList.Add(waitingRecipeSO);
            }
        }
    }

    public void DeliveryRecipe(PlateKitchenObject plateKitchenObject)
    {
        // 循环遍历食谱列表，找出客户下单的食谱。
        // 然后对比食谱和盘子里面的食材是否相同，从而做出验证
        for (int i = 0; i < waitingRecipeSOList.Count; i++) {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];
            if (waitingRecipeSO.KitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count) { 
                // 拥有相同的数量
                bool plateCountensMatchesRecipe = true;
                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.KitchenObjectSOList) {
                    bool ingredientFound = false;
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList()) {
                        if (plateKitchenObjectSO == recipeKitchenObjectSO) {
                            ingredientFound = true;
                            break;
                        }
                    }
                    if (!ingredientFound) {
                        // 当这个食谱成分在盘子中没有被发现时，就不匹配
                        plateCountensMatchesRecipe = false;
                    }
                }
                if (plateCountensMatchesRecipe) {
                    // 此时玩家就交付了正确的汉堡
                    Debug.Log("玩家交付了正确的东西！");
                    waitingRecipeSOList.RemoveAt(i);
                    return;
                }
            }
        }
        // 玩家提供的东西和订单的食谱不匹配
        Debug.Log("玩家没有提供正确的汉堡");
    }
}
