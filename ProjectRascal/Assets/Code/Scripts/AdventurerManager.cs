using Assets.Code.Scripts.NetClient;
using Assets.Code.Scripts.NetClient.Attributes;
using Assets.Code.Scripts.NetClient.Emissary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Search;
using UnityEngine;
using System.Collections.Concurrent;

namespace Assets.Code.Scripts
{
    public class AdventurerManager : MonoBehaviour
    {
        public GameObject AdventurerPrefab;

        //private Dictionary<int, GameObject> Adventurers  = 
        // new Dictionary<int, GameObject>();

        private Dictionary<int, AdventurerController> Adventurers =
            new Dictionary<int, AdventurerController>();

        private ConcurrentQueue<(AdventurerAttributesData, TransformData)> AdventurerLoadData 
            = new ConcurrentQueue<(AdventurerAttributesData, TransformData)>();

        private void Start()
        {
            AdventurerLoadEmissary.instance.OnNewAdventurerLoad += AddNewAdventurer;
            Debug.Log("AdventurerCollection Awake");

            AdventurerStateEmissary.instance.OnAdventurerNameUpdate += ChangeAdventurerName;
            AdventurerStateEmissary.instance.OnAdventurerCurrentHealthUpdate += ChangeAdventurerCurrentHealth;
            AdventurerStateEmissary.instance.OnAdventurerMaxHealthUpdate += ChangeAdventurerMaxHealth;
            AdventurerStateEmissary.instance.OnAdventurerCurrentManaUpdate += ChangeAdventurerCurrentMana;
            AdventurerStateEmissary.instance.OnAdventurerMaxManaUpdate += ChangeAdventurerMaxMana;

            AdventurerTransformEmissary.instance.OnAdventurerTransformChanged += ChangeAdventurerTransform;
        }

        private float delayTimer = 0f;
        private float delayDuration = 1f; 

        private void Update()
        {
            if(AdventurerLoadData.Count > 0)
            {
                if (AdventurerLoadData.TryDequeue(out var loadData))
                {
                    GameObject adventurerObject = Instantiate(AdventurerPrefab, loadData.Item2.Position, Quaternion.identity);
                    AdventurerController controller = adventurerObject.GetComponent<AdventurerController>();
                    controller.InitializeData(loadData.Item1, loadData.Item2);
                    //Adventurers.Add(loadData.Item1.CharacterVId, controller);
                    Adventurers[loadData.Item1.CharacterVId] = controller;
                    Debug.Log("Adventurers dictionary count = " + Adventurers.Count);
                    
                    string str = string.Empty;
                    foreach(var adventurer in Adventurers)
                    {
                        str = str + adventurer.Key.ToString() + " ";
                    }

                    Debug.Log("Adventurers dictionary: " + str);
                    Debug.Log("New Adventurer join! with VId = " + loadData.Item1.CharacterVId + "Position = " + controller.transform.position);
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
            AdventurerLoadData.Enqueue((AdventurerStateEmissary.instance.GetAdventurerAttributes(AdventurerVId),
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
            Debug.Log("Adventurers count = " + Adventurers.Count);
            Debug.Log("Jakis gracz sie porusza :OO");
            TransformData transform = AdventurerTransformEmissary.instance.GetAdventurerTransformData(AdventurerVId);
            //Adventurers[AdventurerVId].SetTransform(transform.Position, transform.Rotation );
            Adventurers[AdventurerVId].SetTransform(transform.Position, transform.Rotation);
        }

        #endregion

    }
}
