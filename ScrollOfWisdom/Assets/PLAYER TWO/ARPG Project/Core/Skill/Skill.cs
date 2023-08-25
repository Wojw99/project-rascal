using UnityEngine;

namespace PLAYERTWO.ARPGProject
{
    [CreateAssetMenu(fileName = "New Skill", menuName = "PLAYER TWO/ARPG Project/Skills/Skill")]
    public class Skill : ScriptableObject
    {
        public enum CastingOrigin { Center, Hands, Floor, TargetCenter, TargetFloor }

        [Header("General Settings")]
        [Tooltip("A small image to visually represent the Skill on the interface.")]
        public Sprite icon;

        [Tooltip("The audio that plays when this SKill is used.")]
        public AudioClip sound;

        [Tooltip("The minimum duration in seconds between each usage of this Skill.")]
        public float coolDown;

        [Header("Casting Settings")]
        [Tooltip("A prefab to automatically instantiate when this Skill is used.")]
        public GameObject castObject;

        [Tooltip("The origin of the cast object when instantiated.")]
        public CastingOrigin castingOrigin;

        [Header("Target Settings")]
        [Tooltip("If true, this Skill can only be used against a target.")]
        public bool requireTarget = true;

        [Tooltip("If true, the Skill caster will look at the casting direction.")]
        public bool faceInterestDirection = true;

        [Header("Animation Setting")]
        [Tooltip("If true, the 'performing skill' animation will use the same Animation Clip of regular attacks.")]
        public bool useRegularAttackClip;

        [Tooltip("An Animation Clip to override the 'performing skill' animation of the Skill caster.")]
        public AnimationClip overrideClip;

        [Header("Mana Settings")]
        [Tooltip("If true, the usage of this Skill will decrease the amount of available mana points from the Skill caster.")]
        public bool useMana = true;

        [Tooltip("The amount of mana points this Skill will consume.")]
        public int manaCost;

        [Header("Healing Settings")]
        [Tooltip("If true, using this Skill will increase the health points of the SKill caster.")]
        public bool useHealing;

        [Tooltip("The amount of health points to increase.")]
        public int healingAmount;

        /// <summary>
        /// Returns true if this Skill is a attacking skill.
        /// </summary>
        public virtual bool IsAttack() => this is SkillAttack;

        /// <summary>
        /// Returns the Skill casted to the Skill Attack type.
        /// </summary>
        public virtual SkillAttack AsAttack() => (SkillAttack)this;
    }
}
