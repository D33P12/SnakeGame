using UnityEngine;
using System.Collections.Generic;
public class FoodSpawner : MonoBehaviour
{
    public List<Transform> spawnPoints; 
    public GameObject foodPrefab;
    public int maxFoodCount = 1; 
    public float minDistanceBetweenFood = 2f;

    private List<GameObject> activeFood = new List<GameObject>(); 
    private ObjNav objNav; 
    private void Start()
    {
        objNav = FindObjectOfType<ObjNav>(); 

        for (int i = 0; i < maxFoodCount; i++)
        {
            SpawnFood();
        }
    }

    public void SpawnFood()
    {
        if (activeFood.Count >= maxFoodCount) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

        if (IsPositionClear(spawnPoint.position))
        {
            GameObject newFood = Instantiate(foodPrefab, spawnPoint.position, Quaternion.identity);
            activeFood.Add(newFood);

            objNav?.RegisterPrefabSpawn(newFood);

            Food foodScript = newFood.GetComponent<Food>();
            if (foodScript != null)
            {
                foodScript.OnFoodDestroyed += () => HandleFoodDestroyed(newFood);
            }
        }
    }

    private bool IsPositionClear(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, minDistanceBetweenFood);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Food"))
            {
                return false;
            }
        }
        return true;
    }

    private void HandleFoodDestroyed(GameObject food)
    {
        activeFood.Remove(food);

        objNav?.UnregisterPrefab(food);

        SpawnFood();
    }
}
