using UnityEngine;
using System.Collections;

namespace PLAYERTWO.ARPGProject
{
    [AddComponentMenu("PLAYER TWO/ARPG Project/Item/Item Loot")]
    public class ItemLoot : MonoBehaviour
    {
        [Tooltip("The Item Loot Stats settings for the loots.")]
        public ItemLootStats stats;

        protected Entity m_entity;

        protected const float k_groundOffset = 0.1f;
        protected const float k_lootRayOffset = 0.5f;
        protected const float k_lootLoopDelay = 0.1f;

        protected Game m_game => Game.instance;
        protected CollectibleItem m_itemPrefab => m_game.collectibleItemPrefab;
        protected CollectibleMoney m_moneyPrefab => m_game.collectibleMoneyPrefab;

        /// <summary>
        /// Starts the looting routine.
        /// </summary>
        public virtual void Loot()
        {
            if (Random.Range(0, 1f) > stats.lootChance) return;

            StopAllCoroutines();
            StartCoroutine(LootRoutine());
        }

        protected virtual void InstantiateItem(Vector3 position)
        {
            var index = Random.Range(0, stats.items.Length);
            var item = new ItemInstance(stats.items[index],
                stats.generateAttributes, stats.minAttributes, stats.maxAttributes);
            var collectible = Instantiate(m_itemPrefab, position, Quaternion.identity);
            collectible.SetItem(item);
        }

        protected virtual void InstantiateMoney(Vector3 position)
        {
            var money = Instantiate(m_moneyPrefab, position, Quaternion.identity);
            money.amount = Random.Range(stats.minMoneyAmount, stats.maxMoneyAmount);
        }

        protected virtual Vector3 GetLootOrigin()
        {
            var random = Random.insideUnitCircle;
            var radius = Random.Range(stats.randomPositionMinRadius, stats.randomPositionMaxRadius);
            var randomOffset = new Vector3(random.x, 0, random.y) * radius;
            var position = transform.position + Vector3.up * k_lootRayOffset;

            if (stats.randomPosition)
                position += randomOffset;

            return position;
        }

        protected IEnumerator LootRoutine()
        {
            for (int i = 0; i <= stats.loopCount; i++)
            {
                yield return new WaitForSeconds(k_lootLoopDelay);

                var origin = GetLootOrigin();

                if (Physics.Raycast(origin, Vector3.down, out var hit, 2f))
                {
                    var position = hit.point + Vector3.up * k_groundOffset;

                    if (Random.Range(0, 1f) > stats.moneyChance)
                    {
                        InstantiateItem(position);
                        continue;
                    }

                    InstantiateMoney(position);
                }
            }
        }

        protected virtual void Start()
        {
            if (TryGetComponent(out m_entity))
            {
                m_entity.onDie.AddListener(Loot);
            }
        }
    }
}
