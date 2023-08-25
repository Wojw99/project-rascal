using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace PLAYERTWO.ARPGProject
{
    [AddComponentMenu("PLAYER TWO/ARPG Project/GUI/GUI Quest HUD")]
    public class GUIQuestHUD : MonoBehaviour
    {
        [Header("Texts References")]
        [Tooltip("A reference to the Text component that represents the status of the Quest.")]
        public Text status;

        [Tooltip("A reference to the Text component that represents the title of the Quest.")]
        public Text title;

        [Tooltip("A reference to the Text component that represents the objective of the Quest.")]
        public Text objective;

        [Header("Display Settings")]
        [Tooltip("The duration in seconds the HUD takes to fade in.")]
        public float showDuration = 0.25f;

        [Tooltip("The duration in seconds the HUD takes to fade out.")]
        public float hideDuration = 1f;

        [Tooltip("The duration in seconds before the HUD starts to fade out.")]
        public float hideDelay = 3f;

        [Header("Status Messages")]
        [Tooltip("The status text to use when the Quest was accepted.")]
        public string newQuestStatus = "New Quest";

        [Tooltip("The status text to use when the Quest was completed.")]
        public string questCompletedStatus = "Quest Completed";

        [Header("Audio Settings")]
        [Tooltip("The Audio Clip that plays when the Quest was accepted.")]
        public AudioClip questAccepted;

        [Tooltip("The Audio Clip that plays when the Quest was completed.")]
        public AudioClip questCompleted;

        protected CanvasGroup m_group;

        protected GameAudio m_audio => GameAudio.instance;

        protected virtual void InitializeCanvasGroup()
        {
            if (!TryGetComponent(out m_group))
                m_group = gameObject.AddComponent<CanvasGroup>();

            m_group.alpha = 0;
        }

        protected virtual void InitializeCallbacks()
        {
            LevelQuests.instance.onQuestAdded.AddListener(OnQuestAdded);
            LevelQuests.instance.onQuestCompleted.AddListener(onQuestCompleted);
        }

        protected virtual void OnQuestAdded(QuestInstance quest)
        {
            m_audio.PlayUiEffect(questAccepted);
            UpdateTexts(quest, newQuestStatus);
            StopAllCoroutines();
            StartCoroutine(ShowRoutine());
        }

        protected virtual void onQuestCompleted(QuestInstance quest)
        {
            m_audio.PlayUiEffect(questCompleted);
            UpdateTexts(quest, questCompletedStatus);
            StopAllCoroutines();
            StartCoroutine(ShowRoutine());
        }

        protected virtual void UpdateTexts(QuestInstance quest, string status)
        {
            this.status.text = status;
            title.text = quest.data.title;
            objective.text = quest.data.objective;
        }

        protected IEnumerator ShowRoutine()
        {
            for (float timer = 0; timer < showDuration;)
            {
                timer += Time.deltaTime;
                m_group.alpha = Mathf.Lerp(0, 1, timer / showDuration);
                yield return null;
            }

            yield return new WaitForSeconds(hideDelay);

            for (float timer = 0; timer < hideDuration;)
            {
                timer += Time.deltaTime;
                m_group.alpha = Mathf.Lerp(1, 0, timer / hideDuration);
                yield return null;
            }

            m_group.alpha = 0;
        }

        protected virtual void Start()
        {
            InitializeCanvasGroup();
            InitializeCallbacks();
        }
    }
}
