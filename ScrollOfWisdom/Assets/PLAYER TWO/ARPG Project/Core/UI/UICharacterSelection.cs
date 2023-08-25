using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

namespace PLAYERTWO.ARPGProject
{
    [AddComponentMenu("PLAYER TWO/ARPG Project/UI/UI Character Selection")]
    public class UICharacterSelection : MonoBehaviour
    {
        [Header("Character References")]
        [Tooltip("A reference to the UI Character Window to display the Character's data.")]
        public UICharacterWindow characterWindow;

        [Tooltip("The prefab to use to represent each character on the list.")]
        public UICharacterButton characterSlot;

        [Tooltip("A reference to the UI Character Form component.")]
        public UICharacterForm characterForm;

        [Tooltip("A reference to the Game Object that contains the character's list.")]
        public GameObject charactersWindow;

        [Tooltip("A reference to the Game Object that contains the actions to manage characters.")]
        public GameObject characterActions;

        [Tooltip("A reference to the transform used as container for placing all characters.")]
        public Transform charactersContainer;

        [Header("Button References")]
        [Tooltip("A reference to the Button component that opens the character creation window.")]
        public Button newCharacterButton;

        [Tooltip("A reference to the Button component that starts the Game.")]
        public Button startGameButton;

        [Tooltip("A reference to the Button component that deletes the current selected character.")]
        public Button deleteCharacterButton;

        [Header("Audio References")]
        [Tooltip("The Audio Clip that plays when selecting a character.")]
        public AudioClip selectCharacterAudio;

        [Tooltip("The Audio Clip that plays when deleting a character.")]
        public AudioClip deleteCharacterAudio;

        [Space(15)]
        public UnityEvent<CharacterInstance> onCharacterSelected;

        protected CanvasGroup m_charactersWindowGroup;
        protected CanvasGroup m_characterActionsGroup;
        protected CharacterInstance m_currentCharacter;

        protected bool m_creatingCharacter;

        protected UICharacterButton m_currentUiCharacter;
        protected List<UICharacterButton> m_characters = new List<UICharacterButton>();

        protected GameAudio m_audio => GameAudio.instance;

        protected virtual void InitializeGroups()
        {
            if (!charactersWindow.TryGetComponent(out m_charactersWindowGroup))
                m_charactersWindowGroup = charactersWindow.AddComponent<CanvasGroup>();

            if (!characterActions.TryGetComponent(out m_characterActionsGroup))
                m_characterActionsGroup = characterActions.AddComponent<CanvasGroup>();
        }

        protected virtual void InitializeCallbacks()
        {
            newCharacterButton.onClick.AddListener(ToggleCharacterCreation);
            characterForm.onSubmit.AddListener(ToggleCharacterCreation);
            characterForm.onCancel.AddListener(ToggleCharacterCreation);
            startGameButton.onClick.AddListener(StartGame);

            deleteCharacterButton.onClick.AddListener(() =>
            {
                m_audio.PlayUiEffect(deleteCharacterAudio);
                DeleteCharacter();
            });

            Game.instance.onCharacterAdded.AddListener(AddCharacter);
            Game.instance.onCharacterDeleted.AddListener(RefreshList);
            Game.instance.onDataLoaded.AddListener(RefreshList);
        }

        /// <summary>
        /// Toggles the character creation form.
        /// </summary>
        public virtual void ToggleCharacterCreation()
        {
            m_creatingCharacter = !m_creatingCharacter;
            m_charactersWindowGroup.alpha = m_characterActionsGroup.alpha = m_creatingCharacter ? 0.5f : 1;
            m_charactersWindowGroup.blocksRaycasts = m_characterActionsGroup.blocksRaycasts = !m_creatingCharacter;
            characterForm.gameObject.SetActive(m_creatingCharacter);
        }

        /// <summary>
        /// Starts the Game with the current character.
        /// </summary>
        public virtual void StartGame()
        {
            if (m_currentCharacter == null) return;

            characterActions.SetActive(false);
            Game.instance.StartGame(m_currentCharacter);
        }

        /// <summary>
        /// Adds a given Character Instance to the Game.
        /// </summary>
        /// <param name="character">The Character Instance you want to add.</param>
        public virtual void AddCharacter(CharacterInstance character)
        {
            if (character == null) return;

            SelectCharacter(character);
            RefreshList();
        }

        /// <summary>
        /// Deletes the current selected character.
        /// </summary>
        public virtual void DeleteCharacter()
        {
            if (m_currentCharacter == null) return;

            Game.instance.DeleteCharacter(m_currentCharacter);
            characterWindow.gameObject.SetActive(false);
            characterActions.gameObject.SetActive(false);
            m_currentCharacter = null;
        }

        /// <summary>
        /// Selects a given Character Instance.
        /// </summary>
        /// <param name="character">The Character Instance you want to select.</param>
        public virtual void SelectCharacter(CharacterInstance character)
        {
            if (character == m_currentCharacter) return;

            m_currentCharacter = character;
            characterWindow.gameObject.SetActive(true);
            characterActions.gameObject.SetActive(true);
            characterWindow.UpdateTexts(m_currentCharacter);
            onCharacterSelected.Invoke(m_currentCharacter);
        }

        /// <summary>
        /// Refreshes the list of characters.
        /// </summary>
        public virtual void RefreshList()
        {
            var characters = Game.instance.characters;

            foreach (var character in m_characters)
            {
                character.gameObject.SetActive(false);
            }

            for (int i = 0; i < characters.Count; i++)
            {
                if (m_characters.Count < i + 1)
                {
                    var index = i;

                    m_characters.Add(Instantiate(characterSlot, charactersContainer));
                    m_characters[i].onSelect.AddListener((character) =>
                    {

                        if (m_currentUiCharacter != m_characters[index])
                            m_currentUiCharacter?.SetInteractable(true);

                        m_currentUiCharacter = m_characters[index];
                        m_currentUiCharacter.SetInteractable(false);
                        m_audio.PlayUiEffect(selectCharacterAudio);
                        SelectCharacter(character);
                    });
                }

                m_characters[i].SetCharacter(characters[i]);
                m_characters[i].gameObject.SetActive(true);
                m_characters[i].SetInteractable(true);

                if (m_currentCharacter == m_characters[i].character)
                {
                    m_currentUiCharacter = m_characters[i];
                    m_currentUiCharacter.SetInteractable(false);
                }
            }
        }

        protected virtual void Start()
        {
            Game.instance.ReloadGameData();
            InitializeGroups();
            InitializeCallbacks();
            RefreshList();
        }
    }
}
