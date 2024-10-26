using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeliveryManager : MonoBehaviour
{
    // 生成订单
    public event EventHandler OnrecipeSpawned;
    // 销毁订单
    public event EventHandler OnRecipeCompleted;
    // 物品交付成功
    public event EventHandler OnRecipeSuccess;
    // 物品交付失败
    public event EventHandler OnRecipeFailed;
    
    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private RecipeListSO recipeListSO;

    // 订单列表
    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipesMax = 4;
    private int successfulRecipesAmount;

    private void Awake()
    {
        // 单例模式
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
        DontDestroyOnLoad(gameObject);

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
                Debug.Log($"顾客下单 + {waitingRecipeSO.recipeName}");
                waitingRecipeSOList.Add(waitingRecipeSO);
                // 启动事件
                OnrecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void DeliveryRecipe(PlateKitchenObject plateKitchenObject)
    {
        // 循环遍历食谱列表，找出客户下单的食谱。
        // 然后对比食谱和盘子里面的食材是否相同，从而做出验证
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];
            if (waitingRecipeSO.KitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                // 先查找出和玩家交付的汉堡分成拥有相同的种数的订单
                bool plateCountersMatchesRecipe = true;
                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.KitchenObjectSOList)
                {
                    // 接下来会检查玩家制作的菜品中的食材是否与食谱中的食材完全匹配
                    bool ingredientFound = false;// 判断汉堡成分和订单要的汉堡的成分是否相同
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        if (plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            ingredientFound = true;
                            break;
                        }
                    }

                    if (!ingredientFound)
                    {
                        // 当这个食谱成分在盘子中没有被找到时，就不匹配
                        plateCountersMatchesRecipe = false;
                    }
                }

                if (plateCountersMatchesRecipe)
                {
                    // 此时玩家就交付了正确的汉堡
                    Debug.Log("玩家交付了正确的东西！");
                    successfulRecipesAmount++;// 记录
                    waitingRecipeSOList.RemoveAt(i);
                    // 启动事件，移除订单
                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
        }

        // 玩家提供的东西和订单的食谱不匹配
        Debug.Log("玩家没有提供正确的汉堡");
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);

    }

    /// <summary>
    /// 返回订单列表
    /// </summary>
    /// <returns></returns>
    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }

    public int GetsuccessfulRecipesAmount()
    {
        return successfulRecipesAmount;
    }

}
