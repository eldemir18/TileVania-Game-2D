using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    [SerializeField] AudioClip hearthPickupSFX;
    
    GameSession gameSession;

    bool isCollected = false;
    private float volume;
    
    void Start()
    {
        volume =  PlayerPrefs.GetFloat("EffectVolume",0.5f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        gameSession = FindObjectOfType<GameSession>();
        if(other.CompareTag("Player") && !isCollected && !gameSession.IsHeartsFull())
        {
            isCollected = true;
            gameSession.UpdateHearts(true);
            AudioSource.PlayClipAtPoint(hearthPickupSFX, Camera.main.transform.position, volume);
            // CHange this as other.gameobject
            Destroy(gameObject);
        }
    }
}
