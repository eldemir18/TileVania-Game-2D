using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSFX;
    [SerializeField] int points = 100;

    bool isCollected = false;
    private float volume;

    void Start()
    {
        volume =  PlayerPrefs.GetFloat("EffectVolume",0.5f);
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Player") && !isCollected)
        {
            isCollected = true;
            FindObjectOfType<GameSession>().AddScore(points);
            AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position, volume);
            Destroy(gameObject);
        }    
    }
}
