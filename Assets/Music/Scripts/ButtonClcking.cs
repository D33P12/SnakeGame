using UnityEngine;

public class ButtonClcking : MonoBehaviour
{
    public AudioSource soundPLayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playThisSoundEffect()
    {
        soundPLayer.Play();
    }
}
