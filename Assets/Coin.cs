using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private AudioClip pickupSoundClip;
    [SerializeField] private int coinScore = 10;

    bool isCollected = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Coin OnTriggerEnter: " + collision.transform.name + " with Tag: " + collision.gameObject.tag);
        if (collision.tag == "Player" && !isCollected)
        {
            isCollected = true;
            AudioSource.PlayClipAtPoint(pickupSoundClip,transform.position);
            FindObjectOfType<GameSession>().IncreaseScore(coinScore);
            Destroy(gameObject);
        }
    }

}
