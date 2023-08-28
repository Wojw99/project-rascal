using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using System.Linq;
using System.Collections.Generic;

namespace PLAYERTWO.ARPGProject
{
    [RequireComponent(typeof(EventSystem), typeof(InputSystemUIInputModule))]
    [AddComponentMenu("PLAYER TWO/ARPG Project/Game/Game")]
    public class Game : Singleton<Game>
    {
        [Header("Game Settings")]
        [Tooltip("The base amount of experience necessary to level up")]
        public int baseExperience = 1973;
        [Tooltip("The amount of additional experience the Player will need to reach the next level")]
        public int experiencePerLevel = 179;
        [Tooltip("The base amount of experience earned from defeating an enemy")]
        public int baseEnemyDefeatExperience = 679;
        [Tooltip("The amount of point(s) added to the distribution points after raising a level")]
        public int levelUpPoints = 5;

        [Header("Combat Settings")]
        public float criticalMultiplier = 1.25f;
        public int maxAttackSpeed = 1000;

        [Header("Item Attributes Settings")]
        [Tooltip("The maximum chance of an item have additional attributes")]
        public float maxAttributeChance = 0.8f;
        [Tooltip("The minimum chance of an item have additional attributes")]
        public float minAttributeChance = 0.1f;
        [Tooltip("The additional price an item will have per attribute")]
        public int pricePerAttribute = 500;

        [Header("Collectibles Prefabs")]
        public CollectibleItem collectibleItemPrefab;
        public CollectibleMoney collectibleMoneyPrefab;

        [Space(15)]
        public UnityEvent<CharacterInstance> onCharacterAdded;
        public UnityEvent onCharacterDeleted;
        public UnityEvent onDataLoaded;

        protected CharacterInstance m_currentCharacter;
        protected GameStash m_stash;

        protected bool m_gameLoaded;

        public List<CharacterInstance> characters { get; protected set; } = new List<CharacterInstance>();

        /// <summary>
        /// Returns the current Character Instance of this Game session.
        /// </summary>
        public CharacterInstance currentCharacter
        {
            get
            {
                LoadGameData();

                if (m_currentCharacter == null)
                {
                    if (characters.Count == 0)
                        CreateCharacter("PLAYERTWO", 0);

                    m_currentCharacter = characters[0];
                }

                return m_currentCharacter;
            }

            set { m_currentCharacter = value; }
        }

        public QuestsManager quests => m_currentCharacter.quests.manager;

        public GameStash stash
        {
            get
            {
                if (!m_stash)
                    m_stash = GetComponent<GameStash>();

                return m_stash;
            }
        }

        /// <summary>
        /// Starts a new Game session with a given Character Instance.
        /// </summary>
        /// <param name="character">The Character Instance you want to start a new session with.</param>
        public virtual void StartGame(CharacterInstance character)
        {
            if (character == null) return;

            currentCharacter = character;

            if (currentCharacter.initialScene != null)
                GameScenes.instance.LoadScene(currentCharacter.initialScene);
        }

        /// <summary>
        /// Closes the game application.
        /// </summary>
        public virtual void ExitGame()
        {
#if UNITY_EDITOR
            Fader.instance.FadeOut(() =>
                UnityEditor.EditorApplication.isPlaying = false);
#elif UNITY_STANDALONE
            Fader.instance.FadeOut(() => Application.Quit());
#endif
        }

        /// <summary>
        /// Creates a new Character Instance with a given name.
        /// </summary>
        /// <param name="name">The name of the Character Instance to create.</param>
        /// <param name="classId">The index of the character data.</param>
        public virtual void CreateCharacter(string name, int classId)
        {
            var characterType = GameDatabase.instance.FindElementById<Character>(classId);
            var character = new CharacterInstance(characterType, name);
            characters.Add(character);
            currentCharacter = character;
            onCharacterAdded?.Invoke(character);
        }

        /// <summary>
        /// Deletes a Character Instance from the list of available characters.
        /// </summary>
        /// <param name="character">The Character Instance you want to delete.</param>
        public virtual void DeleteCharacter(CharacterInstance character)
        {
            if (!characters.Contains(character)) return;

            characters.Remove(character);
            onCharacterDeleted?.Invoke();
        }

        /// <summary>
        /// Loads the Game data from the memory.
        /// </summary>
        public virtual void LoadGameData()
        {
            if (m_gameLoaded) return;

            var data = GameSave.instance.Load();

            m_gameLoaded = true;

            if (data == null) return;

            characters = data.characters.Select(c =>
                CharacterInstance.CreateFromSerializer(c)).ToList();

            stash.LoadData(data.stashes);
            onDataLoaded?.Invoke();
        }

        /// <summary>
        /// Reloads the Game data from the memory.
        /// </summary>
        public virtual void ReloadGameData()
        {
            m_gameLoaded = false;
            m_currentCharacter = null;
            LoadGameData();
        }

        protected override void Initialize()
        {
            LoadGameData();
            DontDestroyOnLoad(gameObject);
        }

        void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
                GameSave.instance.Save();
        }
    }
}
