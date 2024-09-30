using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    
    public event EventHandler<OnStoveStateChangedEventArgs> OnStoveStateChanged;
    public class OnStoveStateChangedEventArgs : EventArgs
    {
        public StoveState stoveState;
    }
    
    /// <summary>
    /// 炉子状态枚举
    /// </summary>
    public enum StoveState
    {
        Idle,
        Frying,
        Fried,
        Burned,
    }
    
    // 存储处理前后食材的映射关系的列表
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    // 烧焦的食材对
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    private StoveState stoveState;
    private float fryingTimer;
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;
    private float burningTimer;

    private void Start()
    {
        stoveState = StoveState.Idle;
    }

    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (stoveState)
            {
                case StoveState.Idle:
                    break;
                case StoveState.Frying:
                    fryingTimer += Time.deltaTime;
                    // 推动进度条
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                    });
                    if (fryingTimer > fryingRecipeSO.fryingTimerMax)
                    {
                        GetKitchenObject().DestroySelf();
                        // 实例化输出的食材
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                        stoveState = StoveState.Fried;
                        burningTimer = 0f;
                        burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                        
                        // 改变为烤制完成的状态
                        OnStoveStateChanged?.Invoke(this, new OnStoveStateChangedEventArgs
                        {
                            stoveState = StoveState.Fried
                        });
                    }
                    break;
                case StoveState.Fried:
                    burningTimer += Time.deltaTime;
                    // 推动进度条
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = burningTimer / burningRecipeSO.burningTimerMax
                    });
                    if (burningTimer > burningRecipeSO.burningTimerMax)
                    {
                        GetKitchenObject().DestroySelf();
                        // 实例化输出的食材
                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);
                        stoveState = StoveState.Burned;
                        OnStoveStateChanged?.Invoke(this, new OnStoveStateChangedEventArgs
                        {
                            stoveState = StoveState.Burned
                        });
                        // 进度条重置
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0
                        });
                    }
                    break;
                case StoveState.Burned:
                    break;
            }
        }
    }

    /// <summary>
    /// 普通交互，拿取
    /// </summary>
    /// <param name="player"></param>
    public override void Interact(Player player)
    {
        if (!this.HasKitchenObject()) {
            // 柜台本身没有物品，就可以放置物品
            if (player.HasKitchenObject()) {
                // 如果玩家拥有的食材可烹饪，才可放置在炉灶上
                if(HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())){
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    stoveState = StoveState.Frying;// 开始烤制
                    fryingTimer = 0f;
                    // 启动烤制特效
                    OnStoveStateChanged?.Invoke(this,new OnStoveStateChangedEventArgs
                    {
                        stoveState = StoveState.Frying
                    });
                    // 初始化进度条
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                    });
                }
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
                // 拿取东西之后，炉子回到关火的状态
                stoveState = StoveState.Idle;
                OnStoveStateChanged?.Invoke(this, new OnStoveStateChangedEventArgs
                {
                    stoveState = StoveState.Idle
                });
                // 进度条重置
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0
                });
            }
        }
    }
    
    // 判断食材能否处理
    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        return fryingRecipeSO != null;
    }
    
    // 返回处理后的食材
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        if (fryingRecipeSO != null)
            return fryingRecipeSO.output;
        else
        {
            return null;
        }
    }
    
    // 获取食材对，即一种映射关系，存储处理前后的食材
    /// <summary>
    /// 获取能烤制的食材对
    /// </summary>
    /// <param name="inputKitchenObjectSO">输入的食材</param>
    /// <returns></returns>
    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == inputKitchenObjectSO)
            {
                return fryingRecipeSO;
            }
        }
        return null;
    }
    
    /// <summary>
    /// 获取能烤焦的食材对
    /// </summary>
    /// <param name="inputKitchenObjectSO">输入的食材</param>
    /// <returns></returns>
    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if (burningRecipeSO.input == inputKitchenObjectSO)
            {
                return burningRecipeSO;
            }
        }
        return null;
    }
}
