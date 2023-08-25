using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace PLAYERTWO.ARPGProject
{
    [AddComponentMenu("PLAYER TWO/ARPG Project/Game/Game Save")]
    public class GameSave : Singleton<GameSave>
    {
        public enum Mode { Binary, JSON, PlayerPrefs }

        [Header("Saving Settings")]
        [Tooltip("The mode in which you want to save the Game data.")]
        public Mode mode = Mode.Binary;

        [Tooltip("The name of the file used to store the Game data.")]
        public string fileName = "save";

        protected const string k_jsonExtension = "json";
        protected const string k_binaryExtension = "bin";

        protected Game m_game => Game.instance;
        protected CharacterInstance m_currentCharacter => m_game.currentCharacter;

        /// <summary>
        /// Saves the Game data in memory using the default saving settings.
        /// </summary>
        public virtual void Save()
        {
            UpdateSceneData();

            switch (mode)
            {
                default:
                case Mode.Binary:
                    SaveBinary();
                    break;
                case Mode.JSON:
                    SaveJSON();
                    break;
                case Mode.PlayerPrefs:
                    SavePlayerPrefs();
                    break;
            }
        }

        /// <summary>
        /// Loads the Game from the memory.
        /// </summary>
        /// <returns>Returns the serializer that represents the Game data.</returns>
        public virtual GameSerializer Load()
        {
            switch (mode)
            {
                default:
                case Mode.Binary:
                    return LoadBinary();
                case Mode.JSON:
                    return LoadJSON();
                case Mode.PlayerPrefs:
                    return LoadPlayerPrefs();
            }
        }

        protected virtual void SaveBinary()
        {
            var path = GetFilePath();
            var data = new GameSerializer(m_game);
            var formatter = new BinaryFormatter();
            var stream = new FileStream(path, FileMode.Create);
            formatter.Serialize(stream, data);
            stream.Close();
        }

        protected virtual GameSerializer LoadBinary()
        {
            var path = GetFilePath();

            if (File.Exists(path))
            {
                var formatter = new BinaryFormatter();
                var stream = new FileStream(path, FileMode.Open);

                try
                {
                    var data = formatter.Deserialize(stream);
                    stream.Close();
                    return data as GameSerializer;
                }
                catch (System.Exception)
                {
                    stream.Close();
                    File.Delete(path);
                }
            }

            return null;
        }

        protected virtual void SaveJSON()
        {
            var path = GetFilePath();
            var data = new GameSerializer(m_game);

            File.WriteAllText(path, data.ToJson());
        }

        protected virtual GameSerializer LoadJSON()
        {
            var path = GetFilePath();

            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                return GameSerializer.FromJson(json);
            }

            return null;
        }

        protected virtual void SavePlayerPrefs()
        {
            var data = new GameSerializer(m_game).ToJson();
            PlayerPrefs.SetString(fileName, data);
            PlayerPrefs.Save();
        }

        protected virtual GameSerializer LoadPlayerPrefs()
        {
            if (PlayerPrefs.HasKey(fileName))
            {
                var data = PlayerPrefs.GetString(fileName);
                return GameSerializer.FromJson(data);
            }

            return null;
        }

        protected virtual string GetFilePath()
        {
            var prefix = Application.isEditor ? "dev_" : "";
            var extension = mode == Mode.JSON ? k_jsonExtension : k_binaryExtension;
            return Application.persistentDataPath + $"/{prefix}{fileName}.{extension}";
        }

        protected virtual void UpdateSceneData()
        {
            if (Level.instance)
                m_currentCharacter.scenes.UpdateScene(Level.instance);
        }
    }
}
