using UnityEngine;

public class LootBoxBreak : MonoBehaviour
{
    [SerializeField]GameObject lootBoxMesh;
    private LootBox lootBoxManager;

   
    void Start()
    {
        lootBoxManager = GameObject.Find("LootboxManager").GetComponent<LootBox>();
    }


    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(lootBoxMesh);
            lootBoxManager.activateLootboxMenu();
        }
    }


}
