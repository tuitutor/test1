using UnityEngine;

[CreateAssetMenu(menuName="uRPG Skill/Passive Skill", order=999)]
public class PassiveSkill : BonusSkill
{
    public override void Apply(GameObject caster, int skillLevel, Vector3 lookAt) {}
}
