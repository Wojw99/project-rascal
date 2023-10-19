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

        //private Dictionary<int, GameObject> Adventurers  = 
        // new Dictionary<int, GameObject>();

        private Dictionary<int, AdventurerController> Adventurers =
            new Dictionary<int, AdventurerController>();

        private ConcurrentQueue<(AdventurerAttributesData, TransformData)> AdventurersLoadData 
            = new ConcurrentQueue<(AdventurerAttributesData, TransformData)>();

        [SerializeField]int AdventurersCount { get { return Adventurers.Count; } }
        [SerializeField] int AdventurersLoadDataCount { get { return AdventurersLoadData.Count; } }

        private void Start()
        {
            AdventurerLoadEmissary.instance.OnNewAdventurerLoad += AddNewAdventurer;

            AdventurerStateEmissary.instance.OnAdventurerNameUpdate += ChangeAdventurerName;
            AdventurerStateEmissary.instance.OnAdventurerCurrentHealthUpdate += ChangeAdventurerCurrentHealth;
            AdventurerStateEmissary.instance.OnAdventurerMaxHealthUpdate += ChangeAdventurerMaxHealth;
            AdventurerStateEmissary.instance.OnAdventurerCurrentManaUpdate += ChangeAdventurerCurrentMana;
            AdventurerStateEmissary.instance.OnAdventurerMaxManaUpdate += ChangeAdventurerMaxMana;

            AdventurerTransformEmissary.instance.OnAdventurerTransformChanged += ChangeAdventurerTransform;
            AdventurerStateEmissary.instance.OnAdventurerStateUpdate += ChangeAdventurerState;
        }

        private float delayTimer = 0f;
        private float delayDuration = 1f; 

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
                else
                {

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
            AdventurersLoadData.Enqueue((AdventurerStateEmissary.instance.GetAdventurerAttributes(AdventurerVId),
                AdventurerTransformEmissary.instance.GetAdventurerTransformData(AdventurerVId)));  
        }
        private void ChangeAdventurerName(int AdventurerVId)
        {
            Adventurers[AdventurerVId].adventurerCharacter.SetName(AdventurerStateEmissary.instance.GetAdventurerAttributes(AdventurerVId).Name);
        }

        private void ChangeAdventurerCurrentHealth(int AdventurerVId)
        {
            Adventurers[AdventurerVId].adventurerCharacter.SetCurrentHealth((AdventurerStateEmissary.instance.GetAdventurerAttributes(AdventurerVId).CurrentHealth));
        }

        private void ChangeAdventurerCurrentMana(int AdventurerVId)
        {
            Adventurers[AdventurerVId].adventurerCharacter.SetCurrentMana((AdventurerStateEmissary.instance.GetAdventurerAttributes(AdventurerVId).CurrentMana));
        }

        private void ChangeAdventurerMaxHealth(int AdventurerVId)
        {
            Adventurers[AdventurerVId].adventurerCharacter.SetMaxHealth((AdventurerStateEmissary.instance.GetAdventurerAttributes(AdventurerVId).MaxHealth));
        }

        private void ChangeAdventurerMaxMana(int AdventurerVId)
        {
            Adventurers[AdventurerVId].adventurerCharacter.SetMaxMana((AdventurerStateEmissary.instance.GetAdventurerAttributes(AdventurerVId).MaxMana));
        }

        private void ChangeAdventurerTransform(int AdventurerVId)
        {
            TransformData transform = AdventurerTransformEmissary.instance.GetAdventurerTransformData(AdventurerVId);
            //Adventurers[AdventurerVId].SetTransform(transform.Position, transform.Rotation );
            Adventurers[AdventurerVId].SetTargetTransform(transform.Position, transform.Rotation);
            Adventurers[AdventurerVId].SetAdventurerState(transform.adventurerState);
        }

        private void ChangeAdventurerState(int AdventurerVId)
        {
            Adventurers[AdventurerVId].SetAdventurerState(AdventurerStateEmissary.instance.GetAdventurerAttributes(AdventurerVId).State);
        }

        #endregion

    }
}
