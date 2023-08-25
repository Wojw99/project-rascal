using UnityEngine;

namespace PLAYERTWO.ARPGProject
{
    public abstract class ItemWeapon : ItemEquippable
    {
        [Header("Weapon Settings")]
        [Tooltip("The base minimum damage of this Item.")]
        public int minDamage;

        [Tooltip("The base maximum damage of this Item.")]
        public int maxDamage;

        [Tooltip("The base attack speed of this Item.")]
        public int attackSpeed;

        [Tooltip("The list of audio clips this Item can play when used to perform attacks.")]
        public AudioClip[] attackClips;

        /// <summary>
        /// Get a random damage based on the maximum and minimum base damage settings.
        /// </summary>
        public virtual int GetDamage() => Random.Range(minDamage, maxDamage);
    }
}
