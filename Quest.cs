﻿// The Quest struct only contains the dynamic quest properties, so that the
// static properties can be read from the scriptable object. The benefits are
// low bandwidth and easy Player database saving (saves always refer to the
// scriptable quest, so we can change that any time).
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[Serializable]
public partial struct Quest
{
    public string name;

    // fulfillment fields can be used by inheriting scriptablequests
    // (no arrays because this way database save/load is easier)
    // -> the field(s) can be:
    //    * kill counters for 1 monster
    //    * kill counters for 4 monsters (split into 4 bytes, so 4x255 kills)
    //    * simple boolean checks (1/0)
    //    * checklists (by setting the 32 bits to 1/0)
    // -> could add more fields in the future, but this is probably enough
    public int field0;

    // a quest is complete after finishing it at the npc and getting rewards
    public bool completed;

    // constructors
    public Quest(ScriptableQuest data)
    {
        name = data.name;
        field0 = 0;
        completed = false;
    }

    // wrappers for easier access
    public ScriptableQuest data
    {
        get
        {
            // show a useful error message if the key can't be found
            // note: ScriptableQuest.OnValidate 'is in resource folder' check
            //       causes Unity SendMessage warnings and false positives.
            //       this solution is a lot better.
            if (!ScriptableQuest.dict.ContainsKey(name))
                throw new KeyNotFoundException("There is no ScriptableQuest with name=" + name + ". Make sure that all ScriptableQuests are in the Resources folder so they are loaded properly.");
            return ScriptableQuest.dict[name];
        }
    }
    public int requiredLevel => data.requiredLevel;
    public string predecessor => data.predecessor != null ? data.predecessor.name : "";
    public long rewardGold => data.rewardGold;
    public long rewardExperience => data.rewardExperience;
    public ScriptableItem rewardItem => data.rewardItem;

    // events
    public void OnKilled(GameObject player, int questIndex, GameObject victim) { data.OnKilled(player, questIndex, victim); }
    public void OnLocation(GameObject player, int questIndex, Collider location) { data.OnLocation(player, questIndex, location); }

    // completion
    public bool IsFulfilled(GameObject player) { return data.IsFulfilled(player, this); }
    public void OnCompleted(GameObject player) { data.OnCompleted(player, this); }

    // fill in all variables into the tooltip
    // this saves us lots of ugly string concatenation code. we can't do it in
    // ScriptableQuest because some variables can only be replaced here, hence we
    // would end up with some variables not replaced in the string when calling
    // Tooltip() from the data.
    // -> note: each tooltip can have any variables, or none if needed
    public string ToolTip(GameObject player)
    {
        // we use a StringBuilder so that addons can modify tooltips later too
        // ('string' itself can't be passed as a mutable object)
        // note: field0 tooltip part is done in the scriptable quest, because it
        //       might be a number, might be 'Yes'/'No', etc.
        StringBuilder tip = new StringBuilder(data.ToolTip(player, this));
        tip.Replace("{STATUS}", IsFulfilled(player) ? "<i>Complete!</i>" : "");
        return tip.ToString();
    }
}
