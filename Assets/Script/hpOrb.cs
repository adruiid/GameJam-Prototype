using System;
using System.Threading;
using UnityEngine;

public class hpOrb : MonoBehaviour
{
    GameObject player;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float healingAmount = 5f;
    float timer = 3f;
    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        Vector3 vectorToPlayer = player.transform.position - transform.position;
        if (vectorToPlayer.magnitude < 4f)
        {
            transform.Translate(vectorToPlayer * moveSpeed * Time.deltaTime, Space.World);
        }

        /*
        timer -= Time.deltaTime;

        if (player == null || timer>0f) return;

        Vector3 vectorToPlayer = player.transform.position - transform.position;

        transform.Translate(vectorToPlayer * moveSpeed * Time.deltaTime, Space.World);
        */
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerLevel playerLevel = collision.gameObject.GetComponent<PlayerLevel>();
            playerLevel.healSignal(healingAmount);
            Destroy(gameObject);
        }
    }
}
