using Assets.Code.Scripts.NetClient;
using Assets.Code.Scripts.NetClient.Attributes;
using Assets.Code.Scripts.NetClient.Emissary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Scripts
{
    public class AdventurerCollection : MonoBehaviour
    {

        private Dictionary<int, AdventurerController> Adventurers { get; set; } = 
            new Dictionary<int, AdventurerController>();

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
            delayTimer += Time.deltaTime;

            if (delayTimer >= delayDuration)
            {
                Debug.Log(Adventurers.Count);

                foreach(var adventurer in Adventurers)
                {
                    Debug.Log("Adventurer, with id = " + adventurer.Key + 
                        ", Transform = [" + adventurer.Value.getTransformInfo() + "]");
                }

                delayTimer = 0f;
            }
        }

        #region EventHandlers

        private void AddNewAdventurer(int AdventurerVId)
        {
            AdventurerController newAdventurerController = AdventurerController.CreateAdventurerController(
                    AdventurerTransformEmissary.instance.GetAdventurerTransformData(AdventurerVId),
                    AdventurerStateEmissary.instance.GetAdventurerAttributes(AdventurerVId));

            Adventurers.Add(AdventurerVId, newAdventurerController);
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
            //Adventurers[AdventurerVId].Item2.SetTransform(transform.Position, transform.Rotation );
            Adventurers[AdventurerVId].HandleRotation(transform.Rotation);
            Adventurers[AdventurerVId].HandleRunning(transform.Position);
        }

        #endregion

    }
}
