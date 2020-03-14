﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// inventory, attributes etc. can influence max
// (no recovery bonus because it makes no sense (physically/biologically)
public interface IEnduranceBonus
{
    int GetEnduranceBonus(int baseEndurance);
}

[Serializable]
public class DrainState
{
    public MoveState state;
    public int drain;
}

[DisallowMultipleComponent]
public class Endurance : Energy
{
    public PlayerMovement movement;
    public int _recoveryPerTick = 1;
    public int baseEndurance = 10;

    public List<DrainState> drainStates = new List<DrainState>{
        new DrainState{state = MoveState.RUNNING, drain = -1},
        new DrainState{state = MoveState.AIRBORNE, drain = -1}
    };

    // cache components that give a bonus (attributes, inventory, etc.)
    // (assigned when needed. NOT in Awake because then prefab.max doesn't work)
    IEnduranceBonus[] _bonusComponents;
    IEnduranceBonus[] bonusComponents
    {
        get { return _bonusComponents ?? (_bonusComponents = GetComponents<IEnduranceBonus>()); }
    }

    // calculate max
    public override int max
    {
        get
        {
            int bonus = bonusComponents.Sum(b => b.GetEnduranceBonus(baseEndurance));
            return baseEndurance + bonus;
        }
    }

    public override int recoveryPerTick
    {
        get
        {
            // in a state that drains it? otherwise recover
            DrainState drainState = drainStates.Find(ds => ds.state == movement.state);
            return drainState != null ? drainState.drain : _recoveryPerTick;
        }
    }
}