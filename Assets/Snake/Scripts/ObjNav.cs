using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
public class ObjNav : MonoBehaviour
{
    [SerializeField] public Slider distanceSlider;  
    [SerializeField] private float maxDistance = 100f; 

    private Transform player;  
    private GameObject _closestSoul; 
    private float closestDistance;  
    private List<GameObject> spawnedPrefabs = new List<GameObject>(); 

    private void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform; 
    }

    private void Update()
    {
        if (player == null) return; 

        FindClosestPrefab();
        DisplayClosestPrefab(); 
    }

    void FindClosestPrefab()
    {
        closestDistance = Mathf.Infinity;
        _closestSoul = null;

        foreach (GameObject prefab in spawnedPrefabs)
        {
            if (prefab == null || !prefab.activeInHierarchy) continue;

            float distanceToPrefab = Vector3.Distance(player.position, prefab.transform.position);

            if (distanceToPrefab < closestDistance)
            {
                closestDistance = distanceToPrefab;
                _closestSoul = prefab;
            }
        }
    }

    void DisplayClosestPrefab()
    {
        if (_closestSoul != null)
        {
            distanceSlider.value = Mathf.Clamp(closestDistance, 0, maxDistance);
        }
        else
        {
            distanceSlider.value = 0;
        }
    }

    public void RegisterPrefabSpawn(GameObject prefab)
    {
        if (!spawnedPrefabs.Contains(prefab))
        {
            spawnedPrefabs.Add(prefab);
        }
    }

    public void UnregisterPrefab(GameObject prefab)
    {
        if (spawnedPrefabs.Contains(prefab))
        {
            spawnedPrefabs.Remove(prefab);
        }
    }
}
