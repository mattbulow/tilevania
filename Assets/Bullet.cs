using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D myRigidbody;
    [SerializeField] private float bulletSpeed = 5f;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        myRigidbody.velocity = new Vector2(bulletSpeed*Mathf.Sign(transform.localScale.x),0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Bullet collided with: " + collision.transform.name + " on Layer: " + LayerMask.LayerToName(collision.gameObject.layer));
        switch (LayerMask.LayerToName(collision.gameObject.layer))
        {
            case "Ground":
                Destroy(gameObject);
                break;
            case "Enemy":
                Destroy(gameObject);
                FindObjectOfType<GameSession>().IncreaseScore(50);
                Destroy(collision.gameObject);
                break;
        }
        LayerMask.NameToLayer("string");
    }


}
