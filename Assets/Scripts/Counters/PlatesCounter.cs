using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;// 发送消息，增加盘子显示数量
    public event EventHandler OnPlateRemoved;// 发送消息，减少盘子显示数量

    
    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;
    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f;
    private int platesSpawnedAmout;
    private int platesSpawnedAmoutMax = 4;

    private void Update()
    {
        if (platesSpawnedAmout < platesSpawnedAmoutMax)
        {
            spawnPlateTimer += Time.deltaTime;
            if (spawnPlateTimer > spawnPlateTimerMax)
            {
                spawnPlateTimer = 0f;   
                platesSpawnedAmout++;
                // 通知增加盘子
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    /// <summary>
    /// 一般交互，拿取盘子
    /// </summary>
    /// <param name="player"></param>
    public override void Interact(Player player)
    {
        // 如果玩家手中没有物品
        if (!player.HasKitchenObject())
        {
            if (platesSpawnedAmout > 0)
            {
                platesSpawnedAmout--;
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
                // 通知减少盘子
                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
