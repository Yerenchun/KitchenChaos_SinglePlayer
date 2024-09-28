
public class TrashCounter : BaseCounter
{
    /// <summary>
    /// 移除玩家手中的物品
    /// </summary>
    /// <param name="player"></param>
    public override void Interact(Player player){
        if (player.HasKitchenObject())
        {
            player.GetKitchenObject().DestroySelf();
        }
    }
}
