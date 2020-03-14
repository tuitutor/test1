﻿// a simple location quest example
//
// how to use locations:
// - create an empty GameObject
// - add a SphereCollider with IsTrigger enabled
// - set the layer to IgnoreRaycast so that we can still click through it
// - set the tag to QuestLocation so that it's forwarded to the quest system
// - name it the same as the quest
//
// if you need multiple locations for one quest, then create another quest type
// with a list of location names and set the separate bits in 'field0' to keep
// track of what was visited.
using UnityEngine;
using System.Text;

[CreateAssetMenu(menuName="uRPG Quest/Location Quest", order=999)]
public class LocationQuest : ScriptableQuest
{
    // events //////////////////////////////////////////////////////////////////
    public override void OnLocation(GameObject player, int questIndex, Collider location)
    {
        PlayerQuests quests = player.GetComponent<PlayerQuests>();

        // the location counts if it has exactly the same name as the quest.
        // simple and stupid.
        if (location.name == name)
        {
            Quest quest = quests.quests[questIndex];
            quest.field0 = 1;
            quests.quests[questIndex] = quest;
        }
    }

    // fulfillment /////////////////////////////////////////////////////////////
    public override bool IsFulfilled(GameObject player, Quest quest)
    {
        return quest.field0 == 1;
    }

    // tooltip /////////////////////////////////////////////////////////////////
    public override string ToolTip(GameObject player, Quest quest)
    {
        // we use a StringBuilder so that addons can modify tooltips later too
        // ('string' itself can't be passed as a mutable object)
        StringBuilder tip = new StringBuilder(base.ToolTip(player, quest));
        tip.Replace("{LOCATIONSTATUS}", quest.field0 == 0 ? "Pending" : "Done");
        return tip.ToString();
    }
}
