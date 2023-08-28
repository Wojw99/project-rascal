using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;

namespace PLAYERTWO.ARPGProject
{
    [AddComponentMenu("PLAYER TWO/ARPG Project/UI/UI Character Form")]
    public class UICharacterForm : MonoBehaviour
    {
        [Tooltip("A reference to the Input Field component used to input the character name.")]
        public InputField characterName;

        [Tooltip("A reference to the Dropdown component used to select the character's class.")]
        public Dropdown characterClass;

        [Tooltip("A reference to the Button component to create characters.")]
        public Button createButton;
        public UnityEvent onSubmit;
        public UnityEvent onCancel;

        [Header("Audio Settings")]
        [Tooltip("The Audio Clip that plays when showing the form.")]
        public AudioClip showFormClip;

        [Tooltip("The Audio Clip that plays when cancelling the form.")]
        public AudioClip cancelFormClip;

        [Tooltip("TYhe Audio Clip that plays when submitting the form.")]
        public AudioClip submitFormClip;

        protected GameAudio m_audio => GameAudio.instance;

        /// <summary>
        /// Cancels the form.
        /// </summary>
        public virtual void Cancel()
        {
            m_audio.PlayUiEffect(cancelFormClip);
            onCancel.Invoke();
        }

        protected virtual void HandleSubmit()
        {
            if (characterName.text.Length <= 0) return;

            Game.instance.CreateCharacter(characterName.text, characterClass.value);
            m_audio.PlayUiEffect(submitFormClip);
            onSubmit.Invoke();
        }

        protected virtual void Start()
        {
            createButton.onClick.AddListener(HandleSubmit);
            characterName.onValueChanged.AddListener((value) =>
                createButton.interactable = value.Length > 0);
        }

        protected virtual void OnEnable()
        {
            var classes = GameDatabase.instance.characters.Select(c => c.name);

            characterClass.ClearOptions();
            characterClass.AddOptions(classes.ToList<string>());
            characterName.text = "";
            createButton.interactable = false;
            m_audio.PlayUiEffect(showFormClip);
        }
    }
}
