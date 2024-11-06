using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjNav : MonoBehaviour
{
    [SerializeField] public List<GameObject> prefabs;
    private Transform player;
    [SerializeField] public Slider distanceSlider;
    private GameObject _closestSoul;
    private float closestDistance;
    [SerializeField] private float maxDistance = 100f;

    void Update()
    {
        player = player == null ? GameObject.FindWithTag("Player").GetComponent<Transform>() : player;
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;
        }

        FindClosestPrefab();
        DisplayClosestPrefab();
    }

    void FindClosestPrefab()
    {
        closestDistance = Mathf.Infinity;
        _closestSoul = null;

        foreach (GameObject prefab in prefabs)
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
    }
}
