﻿// Guns, bows, etc.
using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName="uRPG Item/Weapon(Ranged)", order=999)]
public class RangedWeaponItem : WeaponItem
{
    public AmmoItem requiredAmmo;
    public float zoom = 20;

    [Header("Recoil")]
    [Range(0, 30)] public float recoilHorizontal;
    [Range(0, 30)] public float recoilVertical;

    [Header("Projectile")]
    public Projectile projectile; // Arrows, Bullets, Fireballs, ...

    // usage
    public override Usability CanUse(PlayerEquipment equipment, int equipmentIndex, Vector3 lookAt)
    {
        // not enough time since last attack?
        if (Time.time < equipment.usageEndTime)
            return Usability.Cooldown;

        // not enough ammo?
        if (requiredAmmo != null)
        {
            int index = equipment.GetItemIndexByName(requiredAmmo.name);
            if (index == -1 || equipment.slots[index].amount == 0)
                return Usability.Empty;
        }

        // otherwise we can use it
        return Usability.Usable;
    }

    // helper function
    WeaponDetails GetWeaponDetails(PlayerEquipment equipment)
    {
        Transform rightHandLocation = equipment.rightHandLocation;
        if (rightHandLocation != null && rightHandLocation.childCount > 0)
            return rightHandLocation.GetChild(0).GetComponentInChildren<WeaponDetails>();
        return null;
    }

    public override void Use(PlayerEquipment equipment, int equipmentIndex, Vector3 lookAt)
    {
        Combat combat = equipment.GetComponent<Combat>();

        // decrease ammo (if any is required)
        if (requiredAmmo != null)
        {
            int index = equipment.GetItemIndexByName(requiredAmmo.name);
            if (index != -1)
            {
                ItemSlot slot = equipment.slots[index];
                --slot.amount;
                equipment.slots[index] = slot;
            }
        }

        // spawn the projectile
        if (projectile != null)
        {
            // decide where to spawn it (right hand or weapon muzzle location)
            WeaponDetails details = GetWeaponDetails(equipment);
            Transform rightHandLocation = equipment.rightHandLocation;

            Vector3 spawnPosition = rightHandLocation.position;
            Quaternion spawnRotation = equipment.transform.rotation;
            if (details != null && details.muzzleLocation != null)
            {
                spawnPosition = details.muzzleLocation.position;
                spawnRotation = details.muzzleLocation.rotation;
            }

            GameObject go = Instantiate(projectile.gameObject, spawnPosition, spawnRotation);
            Projectile proj = go.GetComponent<Projectile>();
            proj.caster = combat.gameObject;
            proj.damage = damage;
            proj.direction = lookAt - spawnPosition;
        }
        else Debug.LogWarning(name + ": missing projectile");
    }

    public override void OnUsed(PlayerEquipment equipment, Vector3 lookAt)
    {
        // play shot sound in any case
        if (successfulUseSound) equipment.audioSource.PlayOneShot(successfulUseSound);

        // recoil:
        // horizontal from - to +
        // vertical from 0 to + (recoil never goes downwards)
        float horizontal = Random.Range(-recoilHorizontal/2, recoilHorizontal/2);
        float vertical = Random.Range(0, recoilVertical);

        // rotate player horizontally, rotate camera vertically
        equipment.transform.Rotate(new Vector3(0, horizontal, 0));
        Camera.main.transform.Rotate(new Vector3(-vertical, 0, 0));
    }

    // tooltip
    public override string ToolTip()
    {
        StringBuilder tip = new StringBuilder(base.ToolTip());
        tip.Replace("{REQUIREDAMMO}", requiredAmmo != null ? requiredAmmo.name : "");
        return tip.ToString();
    }
}
