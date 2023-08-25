using UnityEngine;
using UnityEngine.SceneManagement;

namespace PLAYERTWO.ARPGProject
{
    public class CharacterInstance
    {
        public Character data;

        public string name;
        public string initialScene;

        public Vector3 initialPosition;
        public Quaternion initialRotation;

        public CharacterStats stats;
        public CharacterEquipments equipments;
        public CharacterInventory inventory;
        public CharacterSkills skills;
        public CharacterQuests quests;
        public CharacterScenes scenes;

        protected Entity m_entity;

        public Vector3 currentPosition => m_entity ? m_entity.position : initialPosition;
        public Quaternion currentRotation => m_entity ? m_entity.transform.rotation : initialRotation;

        /// <summary>
        /// Returns the current scene.
        /// </summary>
        public string currentScene => Level.instance ? SceneManager.GetActiveScene().name : initialScene;

        public CharacterInstance() { }

        public CharacterInstance(Character data, string name)
        {
            this.data = data;
            this.name = name;
            initialScene = data.initialScene;
            stats = new CharacterStats(data);
            equipments = new CharacterEquipments(data);
            inventory = new CharacterInventory(data);
            skills = new CharacterSkills(data);
            quests = new CharacterQuests();
            scenes = new CharacterScenes();
        }

        /// <summary>
        /// Instantiates a new Entity from this Character Instance data.
        /// </summary>
        public virtual Entity Instantiate()
        {
            if (m_entity == null)
            {
                m_entity = GameObject.Instantiate(data.entity);
                stats.InitializeStats(m_entity.stats);
                equipments.InitializeEquipments(m_entity.items);
                inventory.InitializeInventory(m_entity.inventory);
                skills.InitializeSkills(m_entity.skills);
                quests.InitializeQuests();
                scenes.InitializeScenes();
            }

            return m_entity;
        }

        public static CharacterInstance CreateFromSerializer(CharacterSerializer serializer)
        {
            var data = GameDatabase.instance.FindElementById<Character>(serializer.characterId);

            return new CharacterInstance()
            {
                data = data,
                name = serializer.name,
                initialScene = serializer.scene,
                initialPosition = serializer.position.ToUnity(),
                initialRotation = Quaternion.Euler(serializer.rotation.ToUnity()),
                stats = CharacterStats.CreateFromSerializer(serializer.stats),
                equipments = CharacterEquipments.CreateFromSerializer(serializer.equipments),
                inventory = CharacterInventory.CreateFromSerializer(serializer.inventory),
                skills = CharacterSkills.CreateFromSerializer(serializer.skills),
                quests = CharacterQuests.CreateFromSerializer(serializer.quests),
                scenes = CharacterScenes.CreateFromSerializer(serializer.scenes)
            };
        }
    }
}
