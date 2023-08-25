using UnityEngine;

namespace PLAYERTWO.ARPGProject
{
    [CreateAssetMenu(fileName = "New Potion", menuName = "PLAYER TWO/ARPG Project/Item/Potion")]
    public class ItemPotion : ItemConsumable
    {
        [Header("Healing Settings")]
        [Tooltip("The amount of health points this Potion recovers.")]
        public int healthAmount;

        [Tooltip("The amount of mana points this Potion recovers.")]
        public int manaAmount;
    }
}
