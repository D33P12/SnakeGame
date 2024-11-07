using System;
using UnityEngine;

public class Food : MonoBehaviour
{
    public event Action OnFoodDestroyed;

    private void OnDestroy()
    {
        OnFoodDestroyed?.Invoke(); 
    }
}
