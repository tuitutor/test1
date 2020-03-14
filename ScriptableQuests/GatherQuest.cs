// a simple gather quest example
using UnityEngine;
using System.Text;

[CreateAssetMenu(menuName="uRPG Quest/Gather Quest", order=999)]
public class GatherQuest : ScriptableQuest
{
    [Header("Fulfillment")]
    public ScriptableItem gatherItem;
    public int gatherAmount;

    // fulfillment /////////////////////////////////////////////////////////////
    public override bool IsFulfilled(GameObject player, Quest quest)
    {
        PlayerInventory inventory = player.GetComponent<PlayerInventory>();
        return gatherItem != null &&
               inventory.Count(new Item(gatherItem)) >= gatherAmount;
    }

    public override void OnCompleted(GameObject player, Quest quest)
    {
        PlayerInventory inventory = player.GetComponent<PlayerInventory>();
        // remove gathered items from player's inventory
        if (gatherItem != null)
            inventory.Remove(new Item(gatherItem), gatherAmount);
    }

    // tooltip /////////////////////////////////////////////////////////////////
    public override string ToolTip(GameObject player, Quest quest)
    {
        PlayerInventory inventory = player.GetComponent<PlayerInventory>();

        // we use a StringBuilder so that addons can modify tooltips later too
        // ('string' itself can't be passed as a mutable object)
        StringBuilder tip = new StringBuilder(base.ToolTip(player, quest));
        tip.Replace("{GATHERAMOUNT}", gatherAmount.ToString());
        if (gatherItem != null)
        {
            int gathered = inventory.Count(new Item(gatherItem));
            tip.Replace("{GATHERITEM}", gatherItem.name);
            tip.Replace("{GATHERED}", Mathf.Min(gathered, gatherAmount).ToString());
        }
        return tip.ToString();
    }
}
