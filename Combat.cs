using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

// inventory, attributes etc. can influence max health
public interface ICombatBonus
{
    int GetDamageBonus();
    int GetDefenseBonus();
}

[Serializable] public class UnityEventGameObjectInt : UnityEvent<GameObject, int> {}

[RequireComponent(typeof(Level))]
public class Combat : MonoBehaviour
{
    // components to be assigned in the inspector
    public Level level;

    // invincibility is useful for GMs etc.
    public bool invincible;
    public LevelBasedInt baseDamage = new LevelBasedInt{baseValue=1};
    public LevelBasedInt baseDefense = new LevelBasedInt{baseValue=1};
    public GameObject onDamageEffect;

    // events
    public UnityEventGameObjectInt onReceivedDamage;
    public UnityEventGameObject onKilledEnemy;

    // cache components that give a bonus (attributes, inventory, etc.)
    ICombatBonus[] bonusComponents;
    void Awake()
    {
        bonusComponents = GetComponentsInChildren<ICombatBonus>();
    }

    // calculate damage
    public int damage => baseDamage.Get(level.current) + bonusComponents.Sum(b => b.GetDamageBonus());

    // calculate defense
    public int defense => baseDefense.Get(level.current) + bonusComponents.Sum(b => b.GetDefenseBonus());

    // deal damage while acknowledging the target's defense etc.
    public void DealDamageAt(GameObject other, int amount, Vector3 hitPoint, Vector3 hitNormal, Collider hitCollider)
    {
        if (other != null)
        {
            Health otherHealth = other.GetComponent<Health>();
            Combat otherCombat = other.GetComponent<Combat>();
            if (otherHealth != null && otherCombat != null)
            {
                // not dead yet? and not invincible?
                if (otherHealth.current > 0 && !otherCombat.invincible)
                {
                    // extra damage on that collider? (e.g. on head)
                    DamageArea damageArea = hitCollider.GetComponent<DamageArea>();
                    float multiplier = damageArea != null ? damageArea.multiplier : 1;
                    int amountMultiplied = Mathf.RoundToInt(amount * multiplier);

                    // subtract defense (but leave at least 1 damage, otherwise
                    // it may be frustrating for weaker players)
                    int damageDealt = Mathf.Max(amountMultiplied - otherCombat.defense, 1);

                    // deal the damage
                    otherHealth.current -= damageDealt;

                    // show effect on the other end
                    otherCombat.ShowDamageEffect(damageDealt, hitPoint, hitNormal);

                    // call OnReceivedDamage event on the target
                    // -> can be used for monsters to pull aggro
                    // -> can be used by equipment to decrease durability etc.
                    otherCombat.onReceivedDamage.Invoke(gameObject, damageDealt);

                    // killed it? then call OnKilledEnemy(other)
                    if (otherHealth.current == 0)
                        onKilledEnemy.Invoke(other);
                }
            }
        }
    }

    public void ShowDamageEffect(int amount, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (onDamageEffect)
            Instantiate(onDamageEffect, hitPoint, Quaternion.LookRotation(-hitNormal));
    }
}