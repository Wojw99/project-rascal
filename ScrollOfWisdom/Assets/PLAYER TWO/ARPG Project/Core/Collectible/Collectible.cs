using UnityEngine;

namespace PLAYERTWO.ARPGProject
{
    [RequireComponent(typeof(Collider))]
    public abstract class Collectible : Interactive
    {
        [Header("GUI Name Settings")]
        [Tooltip("The GUI Collectible prefab that represents this Collectible name on the GUI.")]
        public GUICollectibleName guiName;

        [Tooltip("The color of the text on the GUI Collectible.")]
        public Color nameColor = Color.white;

        protected virtual void InitializeCanvas()
        {
            var gui = Instantiate(guiName);
            gui.SetCollectible(this, nameColor);
        }

        protected override void InitializeTag() => tag = GameTags.Collectible;

        /// <summary>
        /// Collects this Collectible.
        /// </summary>
        /// <param name="other">The object that is collecting this Collectible.</param>
        public virtual void Collect(object other) => Destroy(gameObject);

        protected override void OnInteract(object other)
        {
            if (other is Entity && TryCollect((other as Entity).inventory.instance))
                Collect(other);
        }

        /// <summary>
        /// Returns the name of the Item on the Collectible.
        /// </summary>
        public abstract string GetName();

        protected abstract bool TryCollect(Inventory inventory);

        protected override void Start()
        {
            base.Start();
            InitializeCanvas();
        }
    }
}
