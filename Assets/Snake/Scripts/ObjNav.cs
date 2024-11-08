using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
public class ObjNav : MonoBehaviour
{
    [SerializeField] public Slider distanceSlider;  
    [SerializeField] private float maxDistance = 100f; 
    [SerializeField] private GameObject arrowPrefab;
    
    private Transform player;  
    private GameObject _closestSoul; 
    private float closestDistance;  
    private List<GameObject> spawnedPrefabs = new List<GameObject>(); 
    private GameObject arrowInstance;
    private void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform; 
        arrowInstance = Instantiate(arrowPrefab, player.position + Vector3.up * 2, Quaternion.identity);
        arrowInstance.transform.SetParent(player); 
        arrowInstance.SetActive(false); 
    }

    private void Update()
    {
        if (player == null) return; 

        FindClosestPrefab();
        DisplayClosestPrefab(); 
        UpdateArrowGuide();
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
    void UpdateArrowGuide()
    {
        if (_closestSoul != null && closestDistance <= maxDistance)
        {
            arrowInstance.SetActive(true);
            Vector3 directionToTarget = (_closestSoul.transform.position - player.position).normalized;

            arrowInstance.transform.rotation = Quaternion.LookRotation(directionToTarget) * Quaternion.Euler(0, 180, 0);;
        }
        else
        {
            arrowInstance.SetActive(false);
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
