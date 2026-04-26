using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    [SerializeField]private int waveNumber = 1;
    private EnemySpawner enemySpawner;

    [SerializeField] Text waveText;
    [SerializeField] Animator waveTextAnimator;


    float timer;

    private UniversalStatMultiplier statMultiplier;
    void Start()
    {
        enemySpawner= GetComponent<EnemySpawner>();
        statMultiplier = GameObject.Find("Game Manager").GetComponent<UniversalStatMultiplier>();
    }

    
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 30f)
        {
            StartCoroutine(WaveNumberIncrease());
            timer = 0f;
        }

        waveText.text = "" + waveNumber;

        manageWave();
    }

    void manageWave()
    {
        if (waveNumber == 1)
        {
            enemySpawner.changeSpawnMaxLimit(30);
        }
        if (waveNumber == 2)
        {
            statMultiplier.setUniversalMultiplier(statMultiplier.getDamageMultiplier()*1.2f);
            enemySpawner.changeSpawnIdxLimit(1);
            enemySpawner.changeSpawnMaxLimit(80);
            enemySpawner.changeSpawnRate(1f);
        }
        else if (waveNumber == 3)
        {
            statMultiplier.setUniversalMultiplier(statMultiplier.getDamageMultiplier()*1.2f);
        }
        else if (waveNumber == 4)
        {
            enemySpawner.changeSpawnIdxLimit(2);
            enemySpawner.changeSpawnMaxLimit(120);
            enemySpawner.changeSpawnRate(2f);
        }
        else if (waveNumber == 5)
        {
            statMultiplier.setUniversalMultiplier(statMultiplier.getDamageMultiplier() * 1.2f);
        }
        else if (waveNumber == 6)
        {
            enemySpawner.changeSpawnIdxLimit(3);
            enemySpawner.changeSpawnMaxLimit(200);
            
        }
        else if (waveNumber == 7)
        {
            statMultiplier.setUniversalMultiplier(statMultiplier.getDamageMultiplier() * 1.4f);
            enemySpawner.changeSpawnRate(3f);
            enemySpawner.changeSpawnMaxLimit(300);
        }
        else if (waveNumber == 8)
        {

        }
        else if (waveNumber == 9)
        {
            statMultiplier.setUniversalMultiplier(statMultiplier.getDamageMultiplier() * 1.5f);
            enemySpawner.changeSpawnMaxLimit(100);
            enemySpawner.changeSpawnRate(4f);
        }
        else if (waveNumber == 10)
        {

        }

    }
 
    IEnumerator WaveNumberIncrease()
    {
        waveTextAnimator.SetTrigger("increase");
        yield return new WaitForSeconds(0.7f);
        waveNumber++;
    }
}
