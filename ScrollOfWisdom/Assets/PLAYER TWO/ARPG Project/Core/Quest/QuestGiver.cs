using UnityEngine;

namespace PLAYERTWO.ARPGProject
{
    [AddComponentMenu("PLAYER TWO/ARPG Project/Quest/Quest Giver")]
    public class QuestGiver : Interactive
    {
        [Tooltip("The list of Quests this Quest Giver offers to the Player.")]
        public Quest[] quests;

        /// <summary>
        /// Returns the first non-completed Quest from the quests array.
        /// </summary>
        public virtual Quest CurrentQuest()
        {
            foreach (var quest in quests)
            {
                if (!Game.instance.quests
                    .TryGetQuest(quest, out var instance) ||
                    !instance.completed)
                    return quest;
            }

            return null;
        }

        protected override void OnInteract(object _)
        {
            var current = CurrentQuest();

            if (!current) return;

            GUIWindowsManager.instance.quest.SetQuest(current);
        }
    }
}
