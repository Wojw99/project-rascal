using UnityEngine;

namespace PLAYERTWO.ARPGProject
{
    public class GameTags : MonoBehaviour
    {
        public static string Untagged = "Untagged";
        public static string Player = "Entity/Player";
        public static string Enemy = "Entity/Enemy";
        public static string Interactive = "Interactive";
        public static string Destructible = "Destructible";
        public static string Collectible = "Collectible";

        /// <summary>
        /// Returns true if the tag of a given Game Object matches the targets tags.
        /// </summary>
        public static bool IsTarget(GameObject gameObject) =>
            gameObject && (gameObject.CompareTag(Enemy) || gameObject.CompareTag(Destructible));

        /// <summary>
        /// Returns true if the tag of a given Game Object matches the interactives tags.
        /// </summary>
        public static bool IsInteractive(GameObject gameObject) =>
            gameObject && (gameObject.CompareTag(Interactive) || gameObject.CompareTag(Collectible));
    }
}
