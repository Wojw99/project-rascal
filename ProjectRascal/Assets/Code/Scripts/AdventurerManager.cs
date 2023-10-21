using Assets.Code.Scripts.NetClient;
using Assets.Code.Scripts.NetClient.Attributes;
using Assets.Code.Scripts.NetClient.Emissary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Concurrent;
using JetBrains.Annotations;
using TMPro;

namespace Assets.Code.Scripts
{
    public class AdventurerManager : MonoBehaviour
    {
        public GameObject AdventurerPrefab;

        private Dictionary<int, AdventurerController> Adventurers =
            new Dictionary<int, AdventurerController>();

        private ConcurrentQueue<(AdventurerAttributesData, TransformData)> AdventurersLoadData 
            = new ConcurrentQueue<(AdventurerAttributesData, TransformData)>();

        [SerializeField]int AdventurersCount { get { return Adventurers.Count; } }
        [SerializeField] int AdventurersLoadDataCount { get { return AdventurersLoadData.Count; } }

        private void Start()
        {
            AdventurerLoadEmissary.Instance.OnNewAdventurerLoad += AddNewAdventurer;

            AdventurerStateEmissary.Instance.OnAdventurerNameUpdate += ChangeAdventurerName;
            AdventurerStateEmissary.Instance.OnAdventurerCurrentHealthUpdate += ChangeAdventurerCurrentHealth;
            AdventurerStateEmissary.Instance.OnAdventurerMaxHealthUpdate += ChangeAdventurerMaxHealth;
            AdventurerStateEmissary.Instance.OnAdventurerCurrentManaUpdate += ChangeAdventurerCurrentMana;
            AdventurerStateEmissary.Instance.OnAdventurerMaxManaUpdate += ChangeAdventurerMaxMana;

            AdventurerTransformEmissary.Instance.OnAdventurerTransformChanged += ChangeAdventurerTransform;
            AdventurerStateEmissary.Instance.OnAdventurerStateUpdate += ChangeAdventurerState;
        }

        private void Update()
        {
            if(AdventurersLoadData.Count > 0)
            {
                if (AdventurersLoadData.TryDequeue(out var loadData))
                {
                    GameObject adventurerObject = Instantiate(AdventurerPrefab, loadData.Item2.Position, Quaternion.identity);
                    AdventurerController controller = adventurerObject.GetComponent<AdventurerController>();
                    controller.InitializeData(loadData.Item1, loadData.Item2);
                    //Adventurers.Add(loadData.Item1.CharacterVId, controller);
                    Adventurers[loadData.Item1.CharacterVId] = controller;
                    
                    string str = string.Empty;
                    foreach(var adventurer in Adventurers)
                    {
                        str = str + adventurer.Key.ToString() + " ";
                    }
                }
            }
            /*delayTimer += Time.deltaTime;

            if (delayTimer >= delayDuration)
            {
                Debug.Log(Adventurers.Count);

                foreach(var adventurer in Adventurers)
                {
                    //Debug.Log("Adventurer, with id = " + adventurer.Key + ", Transform = [" + adventurer.Value.getTransformInfo() + "]");
                    Debug.Log("Adventurer, with id = " + adventurer.Key + ", Transform = [" + adventurer.Value.transform + "]");
                }

                delayTimer = 0f;
            }*/
        }

        #region EventHandlers

        private void AddNewAdventurer(int AdventurerVId)
        {
            AdventurersLoadData.Enqueue((AdventurerStateEmissary.Instance.GetAdventurerAttributes(AdventurerVId),
                AdventurerTransformEmissary.Instance.GetAdventurerTransformData(AdventurerVId)));  
        }
        private void ChangeAdventurerName(int AdventurerVId)
        {
            Adventurers[AdventurerVId].adventurerCharacter.SetName(AdventurerStateEmissary.Instance.GetAdventurerAttributes(AdventurerVId).Name);
        }

        private void ChangeAdventurerCurrentHealth(int AdventurerVId)
        {
            Adventurers[AdventurerVId].adventurerCharacter.SetCurrentHealth((AdventurerStateEmissary.Instance.GetAdventurerAttributes(AdventurerVId).CurrentHealth));
        }

        private void ChangeAdventurerCurrentMana(int AdventurerVId)
        {
            Adventurers[AdventurerVId].adventurerCharacter.SetCurrentMana((AdventurerStateEmissary.Instance.GetAdventurerAttributes(AdventurerVId).CurrentMana));
        }

        private void ChangeAdventurerMaxHealth(int AdventurerVId)
        {
            Adventurers[AdventurerVId].adventurerCharacter.SetMaxHealth((AdventurerStateEmissary.Instance.GetAdventurerAttributes(AdventurerVId).MaxHealth));
        }

        private void ChangeAdventurerMaxMana(int AdventurerVId)
        {
            Adventurers[AdventurerVId].adventurerCharacter.SetMaxMana((AdventurerStateEmissary.Instance.GetAdventurerAttributes(AdventurerVId).MaxMana));
        }

        private void ChangeAdventurerTransform(int AdventurerVId)
        {
            TransformData transform = AdventurerTransformEmissary.Instance.GetAdventurerTransformData(AdventurerVId);
            //Adventurers[AdventurerVId].SetTransform(transform.Position, transform.Rotation );
            Adventurers[AdventurerVId].SetTargetTransform(transform.Position, transform.Rotation);
            Adventurers[AdventurerVId].SetAdventurerState(transform.adventurerState);
        }

        private void ChangeAdventurerState(int AdventurerVId)
        {
            Adventurers[AdventurerVId].SetAdventurerState(AdventurerStateEmissary.Instance.GetAdventurerAttributes(AdventurerVId).State);
        }

        #endregion

    }
}
