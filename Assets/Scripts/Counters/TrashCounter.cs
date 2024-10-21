using System;

public class TrashCounter : BaseCounter
{
    public static event EventHandler OnAnyObjectTrashed;
    
    /// <summary>
    /// 移除玩家手中的物品
    /// </summary>
    /// <param name="player"></param>
    public override void Interact(Player player){
        if (player.HasKitchenObject())
        {
            player.GetKitchenObject().DestroySelf();
            OnAnyObjectTrashed?.Invoke(this,EventArgs.Empty);
        }
    }
}
