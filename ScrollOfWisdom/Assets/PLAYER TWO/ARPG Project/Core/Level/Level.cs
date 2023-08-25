using UnityEngine;
using UnityEngine.SceneManagement;

namespace PLAYERTWO.ARPGProject
{
    [AddComponentMenu("PLAYER TWO/ARPG Project/Level/Level")]
    public class Level : Singleton<Level>
    {
        [Tooltip("The transform that represents the initial position and rotation of the Character.")]
        public Transform playerOrigin;

        [Header("Tracking Lists")]
        [Tooltip("The list of all entities in which the Level tracks.")]
        public Entity[] entities;

        [Tooltip("The list of all interactives objects in which the Level tracks.")]
        public Interactive[] interactives;

        [Tooltip("The list of all Game Objects in which the Level tracks.")]
        public GameObject[] gameObjects;

        /// <summary>
        /// Returns the Entity that represents the current player.
        /// </summary>
        public Entity player { get; protected set; }

        public LevelQuests quests => LevelQuests.instance;
        public LevelWaypoints waypoints => LevelWaypoints.instance;
        public CharacterInstance currentCharacter => Game.instance.currentCharacter;
        public Scene currentScene => SceneManager.GetActiveScene();

        protected virtual void InitializePlayer()
        {
            if (Physics.Raycast(playerOrigin.position, Vector3.down, out var hit))
            {
                var position = hit.point + Vector3.up;
                var rotation = playerOrigin.rotation;

                player = currentCharacter.Instantiate();

                if (currentCharacter.initialScene.CompareTo(currentScene.name) == 0 &&
                    (currentCharacter.initialPosition != Vector3.zero ||
                    currentCharacter.initialRotation.eulerAngles != Vector3.zero))
                    player.Teleport(currentCharacter.initialPosition, currentCharacter.initialRotation);
                else
                    player.Teleport(playerOrigin.position, playerOrigin.rotation);
            }
        }

        protected virtual void RestoreState()
        {
            if (!currentCharacter.scenes.TryGetScene(currentScene.name, out var scene)) return;

            for (int i = 0; i < scene.entities.Length; i++)
            {
                if (i >= entities.Length) break;

                var position = scene.entities[i].position;
                var rotation = Quaternion.Euler(scene.entities[i].rotation);

                if (scene.entities[i].health == 0)
                    entities[i].gameObject.SetActive(false);
                else
                {
                    entities[i].stats.Initialize();
                    entities[i].stats.health = scene.entities[i].health;
                    entities[i].Teleport(position, rotation);
                }
            }

            for (int i = 0; i < scene.waypoints.Length; i++)
            {
                if (i >= waypoints.waypoints.Count) break;

                if (waypoints.waypoints[i].title
                    .CompareTo(scene.waypoints[i].title) != 0)
                    continue;

                waypoints.waypoints[i].active = scene.waypoints[i].active;
            }

            RestoreQuestItems(scene);
            RestoreGameObjects(scene);
        }

        protected virtual void RestoreQuestItems(CharacterScenes.Scene scene)
        {
            if (scene.interactives == null) return;

            for (int i = 0; i < scene.interactives.Length; i++)
            {
                if (interactives == null || i >= interactives.Length) break;

                interactives[i].interactive = scene.interactives[i].interactive;
            }
        }

        protected virtual void RestoreGameObjects(CharacterScenes.Scene scene)
        {
            if (scene.gameObjects == null) return;

            for (int i = 0; i < scene.gameObjects.Length; i++)
            {
                if (gameObject == null || i >= gameObjects.Length) break;

                gameObjects[i].transform.position = scene.gameObjects[i].position;
                gameObjects[i].transform.rotation = Quaternion.Euler(scene.gameObjects[i].rotation);
                gameObjects[i].SetActive(scene.gameObjects[i].active);
            }
        }

        protected virtual void EvaluateQuestScene() =>
            Game.instance.quests.ReachedScene(currentScene.name);

        protected override void Initialize()
        {
            InitializePlayer();
            RestoreState();
        }

        protected virtual void Start() => EvaluateQuestScene();
    }
}
