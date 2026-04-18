using UnityEngine;

public class expOrb : MonoBehaviour
{
    GameObject player;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float expAmount=10f;
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vectorToPlayer = player.transform.position - transform.position;
        if (vectorToPlayer.magnitude < 4f)
        {
            transform.Translate(vectorToPlayer * moveSpeed * Time.deltaTime, Space.World);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerLevel playerLevel = collision.gameObject.GetComponent<PlayerLevel>();
            playerLevel.updateExp(expAmount);
            Destroy(gameObject);
        }
    }
}
