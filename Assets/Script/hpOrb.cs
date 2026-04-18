using System;
using System.Threading;
using UnityEngine;

public class hpOrb : MonoBehaviour
{
    GameObject player;
    [SerializeField] float moveSpeed = 5f;
    float timer = 3f;
    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (player == null || timer>0f) return;

        Vector3 vectorToPlayer = player.transform.position - transform.position;

        transform.Translate(vectorToPlayer * moveSpeed * Time.deltaTime, Space.World);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerLevel playerLevel = collision.gameObject.GetComponent<PlayerLevel>();
            playerLevel.healSignal();
            Destroy(gameObject);
        }
    }
}
