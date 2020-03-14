using UnityEngine;
using System;
using System.Linq;

[Serializable]
public struct EquipmentInfo
{
    public string requiredCategory;
    public Transform location;
    public ScriptableItemAndAmount defaultItem;
}

[RequireComponent(typeof(Animator))]
public class PlayerEquipment : Equipment
{
    // Used components. Assign in Inspector. Easier than GetComponent caching.
    public Animator animator;
    public PlayerMovement movement;
    public PlayerLook look;
    public AudioSource audioSource;

    public EquipmentInfo[] slotInfo =
    {
        new EquipmentInfo{requiredCategory="LeftHand", location=null, defaultItem=new ScriptableItemAndAmount()},
        new EquipmentInfo{requiredCategory="Head", location=null, defaultItem=new ScriptableItemAndAmount()},
        new EquipmentInfo{requiredCategory="Chest", location=null, defaultItem=new ScriptableItemAndAmount()},
        new EquipmentInfo{requiredCategory="Ammo", location=null, defaultItem=new ScriptableItemAndAmount()},
        new EquipmentInfo{requiredCategory="RightHand", location=null, defaultItem=new ScriptableItemAndAmount()},
        new EquipmentInfo{requiredCategory="Legs", location=null, defaultItem=new ScriptableItemAndAmount()},
        new EquipmentInfo{requiredCategory="Feet", location=null, defaultItem=new ScriptableItemAndAmount()}
    };

    // punching: reusing 'melee weapon' makes sense because it's the same code anyway
    public MeleeWeaponItem hands;

    // usage (cooldown) end time for firing, etc.
    // doesn't need to be a [SyncVar] since we call RpcOnUsed anyway, resetting
    // it there saves 4 bytes of bandwidth each time, and we can locally reset
    // it without waiting for the server
    [HideInInspector] public double usageEndTime; // double for long term precision

    // left & right hand locations for easier access
    public Transform leftHandLocation
    {
        get
        {
            foreach (EquipmentInfo slot in slotInfo)
                if (slot.requiredCategory == "LeftHand")
                    return slot.location;
            return null;
        }
    }

    public Transform rightHandLocation
    {
        get
        {
            foreach (EquipmentInfo slot in slotInfo)
                if (slot.requiredCategory == "RightHand")
                    return slot.location;
            return null;
        }
    }

    // helpers /////////////////////////////////////////////////////////////////
    // returns current tool or hands
    public UsableItem GetUsableItemOrHands(int index)
    {
        ItemSlot slot = slots[index];
        return slot.amount > 0 ? (UsableItem)slot.item.data : hands;
    }

    // returns current tool or hands
    public UsableItem GetCurrentUsableItemOrHands()
    {
        // find right hand slot
        int index = GetEquipmentTypeIndex("RightHand");
        return index != -1 ? GetUsableItemOrHands(index) : null;
    }

    // TODO this is weird to pass slotindex too. needed because hands option though.
    void TryUseItem(UsableItem itemData, int slotIndex)
    {
        // note: no .amount > 0 check because it's either an item or hands

        // use current item or hands

        // repeated or one time use while holding mouse down?
        if (itemData.keepUsingWhileButtonDown || Input.GetMouseButtonDown(0))
        {
            // get the exact look position on whatever object we aim at
            Vector3 lookAt = look.lookPositionRaycasted;

            // use it
            Usability usability = itemData.CanUse(this, slotIndex, lookAt);
            if (usability == Usability.Usable)
            {
                // attack by using the weapon item
                //Debug.DrawLine(Camera.main.transform.position, lookAt, Color.gray, 1);
                UseItem(slotIndex, lookAt);

                // simulate OnUsed locally without waiting for the Rpc to avoid
                // latency effects:
                // - usedEndTime would be synced too slowly, hence fire interval
                //   would be too slow on clients
                // - TryUseItem would be called immediately again afterwards
                //   because useEndTime wouldn't be reset yet due to latency
                // - decals/muzzle flash would be delayed by latency and feel
                //   bad
                OnUsedItem(itemData, lookAt);
            }
            else if (usability == Usability.Empty)
            {
                // play empty sound locally (if any)
                // -> feels best to only play it when clicking the mouse button once, not while holding
                if (Input.GetMouseButtonDown(0))
                {
                    if (itemData.emptySound)
                        audioSource.PlayOneShot(itemData.emptySound);
                }
            }
            // do nothing if on cooldown (just wait) or if not usable at all
        }
    }

    void Awake()
    {
        // make sure that weaponmounts are empty transform without children.
        // if someone drags in the right hand, then all the fingers would be
        // destroyed by RefreshLocation.
        // => only check in awake once, because at runtime it will have children
        //    if a weapon is equipped (hence we don't check in OnValidate)
        if (leftHandLocation != null && leftHandLocation.childCount > 0)
            Debug.LogWarning(name + " PlayerEquipment.leftHandLocation should have no children, otherwise they will be destroyed.");
        if (rightHandLocation != null && rightHandLocation.childCount > 0)
            Debug.LogWarning(name + " PlayerEquipment.rightHandLocation should have no children, otherwise they will be destroyed.");
    }

    // update
    void Update()
    {
        // refresh equipment models all the time
        for (int i = 0; i < slots.Count; ++i)
            RefreshLocation(i);

        // left click to use weapon(s)
        if (Input.GetMouseButton(0) &&
            Cursor.lockState == CursorLockMode.Locked &&
            health.current > 0 &&
            movement.state != MoveState.CLIMBING &&
            !look.IsFreeLooking() &&
            !Utils.IsCursorOverUserInterface() &&
            Input.touchCount <= 1)
        {
            // find right hand item
            int index = GetEquipmentTypeIndex("RightHand");
            if (index != -1)
            {
                // use current weapon or hands
                TryUseItem(GetCurrentUsableItemOrHands(), index);
            }
        }
    }

    void RebindAnimators()
    {
        foreach (Animator anim in GetComponentsInChildren<Animator>())
            anim.Rebind();
    }

    public void RefreshLocation(Transform location, ItemSlot slot)
    {
        // valid item, not cleared?
        if (slot.amount > 0)
        {
            EquipmentItem itemData = (EquipmentItem)slot.item.data;
            // new model? (don't do anything if it's the same model, which
            // happens after only Item.ammo changed, etc.)
            // note: we compare .name because the current object and prefab
            // will never be equal
            if (location.childCount == 0 || itemData.modelPrefab == null ||
                location.GetChild(0).name != itemData.modelPrefab.name)
            {
                // delete old model (if any)
                if (location.childCount > 0)
                    Destroy(location.GetChild(0).gameObject);

                // use new model (if any)
                if (itemData.modelPrefab != null)
                {
                    // instantiate and parent
                    GameObject go = Instantiate(itemData.modelPrefab);
                    go.name = itemData.modelPrefab.name; // avoid "(Clone)"
                    go.transform.SetParent(location, false);

                    // is it a skinned mesh with an animator?
                    Animator anim = go.GetComponent<Animator>();
                    if (anim != null)
                    {
                        // assign main animation controller to it
                        anim.runtimeAnimatorController = animator.runtimeAnimatorController;

                        // restart all animators, so that skinned mesh equipment will be
                        // in sync with the main animation
                        RebindAnimators();
                    }
                }
            }
        }
        else
        {
            // empty now. delete old model (if any)
            if (location.childCount > 0)
                Destroy(location.GetChild(0).gameObject);
        }
    }
    // note: lookAt is available in PlayerLook, but we still pass the exact
    // uncompressed Vector3 here, because it needs to be PRECISE when shooting,
    // building structures, etc.
    public void UseItem(int index, Vector3 lookAt)
    {
        // validate
        if (0 <= index && index < slots.Count &&
            health.current > 0)
        {
            // use item at index, or hands
            // note: we don't decrease amount / destroy in all cases because
            // some items may swap to other slots in .Use()
            UsableItem itemData = GetUsableItemOrHands(index);
            if (itemData.CanUse(this, index, lookAt) == Usability.Usable)
            {
                // use it
                itemData.Use(this, index, lookAt);

                // reset usage time
                usageEndTime = Time.time + itemData.cooldown;

                // RpcUsedItem needs itemData, but we can't send that as Rpc
                // -> we could send the Item at slots[index], but .data is null
                //    for hands because hands actually live in '.hands' variable
                // -> we could create a new Item(itemData) and send, but it's
                //    kinda odd that it's different from slot
                // => only sending hash saves A LOT of bandwidth over time since
                //    this rpc is called very frequently (each weapon shot etc.)
                //    (we reuse Item's hash generation for simplicity)
                OnUsedItem(itemData, lookAt);
            }
            else
            {
                // CanUse is checked locally before calling this Cmd, so if we
                // get here then either our prediction is off (in which case we
                // really should show a message for easier debugging), or someone
                // tried to cheat, or there's some networking issue, etc.
                Debug.LogWarning("UseItem rejected for: " + name + " item=" + itemData.name + "@" + Time.time);
            }
        }
    }

    // used by local simulation and Rpc, so we might as well put it in a function
    void OnUsedItem(UsableItem itemData, Vector3 lookAt)
    {
        // reset usage time
        usageEndTime = Time.time + itemData.cooldown;

        // call OnUsed
        itemData.OnUsed(this, lookAt);

        // trigger upperbody usage animation
        // (trigger works best for usage, especially for repeated usage to)
        // (only for weapons, not for potions until we can hold potions in hand
        //  later on)
        if (itemData is WeaponItem)
            animator.SetTrigger("UPPERBODY_USED");
    }

    void RefreshLocation(int index)
    {
        ItemSlot slot = slots[index];
        EquipmentInfo info = slotInfo[index];

        // valid category and valid location? otherwise don't bother
        if (info.requiredCategory != "" && info.location != null)
            RefreshLocation(info.location, slot);
    }

    public void SwapInventoryEquip(int inventoryIndex, int equipmentIndex)
    {
        // validate: make sure that the slots actually exist in the inventory
        // and in the equipment
        if (health.current > 0 &&
            0 <= inventoryIndex && inventoryIndex < inventory.slots.Count &&
            0 <= equipmentIndex && equipmentIndex < slots.Count)
        {
            // item slot has to be empty (unequip) or equipable
            ItemSlot slot = inventory.slots[inventoryIndex];
            if (slot.amount == 0 ||
                slot.item.data is EquipmentItem &&
                ((EquipmentItem)slot.item.data).CanEquip(this, inventoryIndex, equipmentIndex))
            {
                // swap them
                ItemSlot temp = slots[equipmentIndex];
                slots[equipmentIndex] = slot;
                inventory.slots[inventoryIndex] = temp;
            }
        }
    }

    public void MergeInventoryEquip(int inventoryIndex, int equipmentIndex)
    {
        // validate: make sure that the slots actually exist in the inventory
        // and in the equipment
        if (health.current > 0 &&
            0 <= inventoryIndex && inventoryIndex < inventory.slots.Count &&
            0 <= equipmentIndex && equipmentIndex < slots.Count)
        {
            // both items have to be valid
            ItemSlot slotFrom = inventory.slots[inventoryIndex];
            ItemSlot slotTo = slots[equipmentIndex];
            if (slotFrom.amount > 0 && slotTo.amount > 0)
            {
                // make sure that items are the same type
                // note: .Equals because name AND dynamic variables matter (petLevel etc.)
                if (slotFrom.item.Equals(slotTo.item))
                {
                    // merge from -> to
                    // put as many as possible into 'To' slot
                    int put = slotTo.IncreaseAmount(slotFrom.amount);
                    slotFrom.DecreaseAmount(put);

                    // put back into the lists
                    inventory.slots[inventoryIndex] = slotFrom;
                    slots[equipmentIndex] = slotTo;
                }
            }
        }
    }

    public void MergeEquipInventory(int equipmentIndex, int inventoryIndex)
    {
        // validate: make sure that the slots actually exist in the inventory
        // and in the equipment
        if (health.current > 0 &&
            0 <= inventoryIndex && inventoryIndex < inventory.slots.Count &&
            0 <= equipmentIndex && equipmentIndex < slots.Count)
        {
            // both items have to be valid
            ItemSlot slotFrom = slots[equipmentIndex];
            ItemSlot slotTo = inventory.slots[inventoryIndex];
            if (slotFrom.amount > 0 && slotTo.amount > 0)
            {
                // make sure that items are the same type
                // note: .Equals because name AND dynamic variables matter (petLevel etc.)
                if (slotFrom.item.Equals(slotTo.item))
                {
                    // merge from -> to
                    // put as many as possible into 'To' slot
                    int put = slotTo.IncreaseAmount(slotFrom.amount);
                    slotFrom.DecreaseAmount(put);

                    // put back into the lists
                    slots[equipmentIndex] = slotFrom;
                    inventory.slots[inventoryIndex] = slotTo;
                }
            }
        }
    }

    // helpers for easier slot access //////////////////////////////////////////
    // GetEquipmentTypeIndex("Chest") etc.
    public int GetEquipmentTypeIndex(string category)
    {
        return slotInfo.ToList().FindIndex(slot => slot.requiredCategory == category);
    }

    // death & respawn /////////////////////////////////////////////////////////
    public void DropItemAndClearSlot(int index)
    {
        // drop and remove from inventory
        ItemSlot slot = slots[index];
        ((PlayerInventory)inventory).DropItem(slot.item, slot.amount);
        slot.amount = 0;
        slots[index] = slot;
    }

    // drop all equipment on death, so others can loot us
    public void OnDeath()
    {
        for (int i = 0; i < slots.Count; ++i)
            if (slots[i].amount > 0)
                DropItemAndClearSlot(i);
    }

    // we don't clear items on death so that others can still loot us. we clear
    // them on respawn.
    public void OnRespawn()
    {
        // for each slot: make empty slot or default item if any
        for (int i = 0; i < slotInfo.Length; ++i)
            slots[i] = slotInfo[i].defaultItem.item != null ? new ItemSlot(new Item(slotInfo[i].defaultItem.item), slotInfo[i].defaultItem.amount) : new ItemSlot();
    }

    // drag & drop /////////////////////////////////////////////////////////////
    void OnDragAndDrop_InventorySlot_EquipmentSlot(int[] slotIndices)
    {
        // slotIndices[0] = slotFrom; slotIndices[1] = slotTo

        // merge? (just check equality, rest is done server sided)
        if (inventory.slots[slotIndices[0]].amount > 0 && slots[slotIndices[1]].amount > 0 &&
            inventory.slots[slotIndices[0]].item.Equals(slots[slotIndices[1]].item))
        {
            MergeInventoryEquip(slotIndices[0], slotIndices[1]);
        }
        // swap?
        else
        {
            SwapInventoryEquip(slotIndices[0], slotIndices[1]);
        }
    }

    void OnDragAndDrop_EquipmentSlot_InventorySlot(int[] slotIndices)
    {
        // slotIndices[0] = slotFrom; slotIndices[1] = slotTo

        // merge? (just check equality, rest is done server sided)
        if (slots[slotIndices[0]].amount > 0 && inventory.slots[slotIndices[1]].amount > 0 &&
            slots[slotIndices[0]].item.Equals(inventory.slots[slotIndices[1]].item))
        {
            MergeEquipInventory(slotIndices[0], slotIndices[1]);
        }
        // swap?
        else
        {
            SwapInventoryEquip(slotIndices[1], slotIndices[0]);
        }
    }
}