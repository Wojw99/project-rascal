using System.Collections.Generic;

namespace PLAYERTWO.ARPGProject
{
    public class QuestsManager
    {
        /// <summary>
        /// Invoked when a new Quest Instance was added to the active quests list.
        /// </summary>
        public System.Action<QuestInstance> onQuestAdded;

        /// <summary>
        /// Invoked when the progress of any Quest Instance was changed.
        /// </summary>
        public System.Action<QuestInstance> onProgressChanged;

        /// <summary>
        /// Invoked when any Quest Instance was completed.
        /// </summary>
        public System.Action<QuestInstance> onQuestCompleted;

        protected List<QuestInstance> m_quests = new List<QuestInstance>();

        /// <summary>
        /// Returns an array as copy of the active quests list.
        /// </summary>
        public QuestInstance[] list => m_quests.ToArray();

        /// <summary>
        /// Overrides the list of active quests from a given array.
        /// </summary>
        /// <param name="quests">The array you want to read from.</param>
        public virtual void SetQuests(QuestInstance[] quests)
        {
            m_quests = new List<QuestInstance>(quests);
        }

        /// <summary>
        /// Adds a new Quest Instance to the active quests list.
        /// </summary>
        /// <param name="quest">The Quest you want to create the instance from.</param>
        public virtual void AddQuest(Quest quest)
        {
            if (ContainsQuest(quest)) return;

            m_quests.Add(new QuestInstance(quest));
            onQuestAdded?.Invoke(m_quests[m_quests.Count - 1]);
        }

        /// <summary>
        /// Gets a Quest Instance from the active quests list.
        /// </summary>
        /// <param name="quest">The data you are trying to find.</param>
        /// <param name="instance">The instance representing the given Quest.</param>
        /// <returns>Returns true if the Quest in the the active quests list.</returns>
        public virtual bool TryGetQuest(Quest quest, out QuestInstance instance)
        {
            instance = m_quests.Find(q => q.data == quest);
            return instance != null;
        }

        /// <summary>
        /// Completes a given Quest Instance.
        /// </summary>
        /// <param name="quest">The Quest Instance you want to complete.</param>
        protected virtual void CompleteQuest(QuestInstance quest)
        {
            if (quest.completed) return;

            if (Level.instance.player)
                quest.Reward(Level.instance.player);

            quest.Complete();
            onQuestCompleted?.Invoke(quest);
        }

        /// <summary>
        /// Returns true if the active quests list contains a given Quest.
        /// </summary>
        /// <param name="quest">The Quest you want to search on the list.</param>
        public virtual bool ContainsQuest(Quest quest) =>
            m_quests.Exists((q) => q.data == quest);

        /// <summary>
        /// Completes all quests from tue active quests list containing a given destination scene name.
        /// </summary>
        /// <param name="scene">The destination scene name.</param>
        public virtual void ReachedScene(string scene)
        {
            foreach (var quest in m_quests)
            {
                if (quest.CanCompleteOnScene(scene))
                    CompleteQuest(quest);
            }
        }

        /// <summary>
        /// Adds progress to all quests from the active quests list containing a given progress key.
        /// It also triggers the completion of Quests that reached their target progress.
        /// </summary>
        /// <param name="key">The progress key of the quests.</param>
        public virtual void AddProgress(string key)
        {
            foreach (var quest in m_quests)
            {
                if (quest.CanAddProgress(key))
                {
                    quest.progress++;
                    onProgressChanged?.Invoke(quest);

                    if (quest.progress == quest.data.targetProgress)
                        CompleteQuest(quest);
                }
            }
        }

        /// <summary>
        /// Triggers the completion of a given Quest from the active quests list.
        /// </summary>
        /// <param name="quest">The Quest to trigger completion.</param>
        public virtual void Trigger(Quest quest)
        {
            if (!TryGetQuest(quest, out var instance)) return;

            if (instance.CanCompleteByTrigger())
                CompleteQuest(instance);
        }
    }
}
