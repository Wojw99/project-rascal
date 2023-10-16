using Assets.Code.Scripts;
using UnityEngine;
using System;

public class EntitySpawner : MonoBehaviour
{
    public GameObject EntityPrefab;

    [SerializeField] private float delayDuration = 8f;
    [SerializeField] private float delayTimer;
    [SerializeField] private bool enabled = false;

    //public int NumberOfAdventurers = 0; // Liczba przykładowych przygód
    private void Start()
    {
        //SpawnAdventurers();
    }

    private void Update()
    {
        try
        {
            if(enabled)
            {
                delayTimer += Time.deltaTime;

                if(delayTimer >= delayDuration)
                {
                    Debug.Log("Spawnuje...");

                    //SpawnSingle();
                    SpawnGroup(15);
                    delayTimer = 0f;
                }

            }

        }
        catch(Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    public void Init(Vector3 pos, Vector3 rot, float delay)
    {
        transform.position = pos;
        transform.rotation = Quaternion.Euler(rot);
        delayDuration = delay;
    }

    private void SpawnGroup(int groupSize)
    {
        for(int i = 0; i < groupSize; i++)
        {
            SpawnSingle();
        }
    }

    private void SpawnSingle()
    {
        Vector3 spawnPosition = transform.position + UnityEngine.Random.insideUnitSphere * 4f;
        spawnPosition.y = 0;
        GameObject entityObject = Instantiate(EntityPrefab, spawnPosition, Quaternion.identity);

        // Pobierz komponent AdventurerController z instancji
        //EnemyController adventurer = enemyObject.GetComponent<EnemyController>();
    }
}