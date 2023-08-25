using UnityEngine;
using System.Collections.Generic;

namespace PLAYERTWO.ARPGProject
{
    [AddComponentMenu("PLAYER TWO/ARPG Project/Game/Game Database")]
    public class GameDatabase : Singleton<GameDatabase>
    {
        [Tooltip("The list of all available Characters.")]
        public List<Character> characters;

        [Tooltip("The list of all available Items.")]
        public List<Item> items;

        [Tooltip("The list of all available Skills.")]
        public List<Skill> skills;

        [Tooltip("The list of all available Quests.")]
        public List<Quest> quests;

        /// <summary>
        /// Returns the index of a given element on a list of a given type.
        /// If the element was not found, this method returns -1.
        /// </summary>
        /// <param name="element">The element you want to find the index.</param>
        /// <typeparam name="T">The type of the list you want to find the element from.</typeparam>
        public int GetElementId<T>(T element) where T : ScriptableObject
        {
            if (element is Character)
                return characters.IndexOf(element as Character);
            else if (element is Item)
                return items.IndexOf(element as Item);
            else if (element is Skill)
                return skills.IndexOf(element as Skill);
            else if (element is Quest)
                return quests.IndexOf(element as Quest);

            return -1;
        }

        /// <summary>
        /// Returns an element by its id from a list of a given type.
        /// </summary>
        /// <param name="id">The id of the element you're looking for.</param>
        /// <typeparam name="T">The type of the list from which you want to find the element.</typeparam>
        public T FindElementById<T>(int id) where T : ScriptableObject
        {
            var type = typeof(T);

            if (type == typeof(Character) && !OutOfRangeFor<Character>(id, characters))
                return characters[id] as T;
            else if (type == typeof(Item) && !OutOfRangeFor<Item>(id, items))
                return items[id] as T;
            else if (type == typeof(Skill) && !OutOfRangeFor<Skill>(id, skills))
                return skills[id] as T;
            else if (type == typeof(Quest) && !OutOfRangeFor<Quest>(id, quests))
                return quests[id] as T;

            return default(T);
        }

        /// <summary>
        /// Returns true if the given index is out of range on a given list.
        /// </summary>
        /// <param name="index">The index you want to check.</param>
        /// <param name="list">The list you want to check.</param>
        /// <typeparam name="T">The type of the data of the list.</typeparam>
        protected virtual bool OutOfRangeFor<T>(int index, List<T> list) =>
            index < 0 || index >= list.Count;
    }
}
