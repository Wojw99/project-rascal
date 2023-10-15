using Assets.Code.Scripts;
using UnityEngine;

public class AdventurerSpawner : MonoBehaviour
{
    public GameObject AdventurerPrefab; // Przypisz prefab w Unity Editor
    public int NumberOfAdventurers = 0; // Liczba przykładowych przygód

    private void Start()
    {
        SpawnAdventurers();
    }

    private void SpawnAdventurers()
    {
        for (int i = 0; i < NumberOfAdventurers; i++)
        {
            // Instancjonuj obiekt i umieszczaj go w losowej lokalizacji wokół spawnera
            Vector3 spawnPosition = transform.position + Random.insideUnitSphere * 4f;
            spawnPosition.y = 0;
            GameObject adventurerObject = Instantiate(AdventurerPrefab, spawnPosition, Quaternion.identity);
            
            // Pobierz komponent AdventurerController z instancji
            AdventurerController adventurer = adventurerObject.GetComponent<AdventurerController>();

            Debug.Log("Spawn adventurer at " + spawnPosition);
            // Podejmij dowolne dodatkowe działania na nowym przygodowcu
            //adventurer.InitializeData(/* Przekazuj dane inicjalizacyjne, jeśli potrzebne */);

            // Możesz również dodać nowego przygodowca do jakiejś listy lub kolekcji
        }
    }
}