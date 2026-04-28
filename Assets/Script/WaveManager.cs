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

    //private UniversalStatMultiplier statMultiplier;
    void Start()
    {
        enemySpawner= GetComponent<EnemySpawner>();
        //statMultiplier = GameObject.Find("Game Manager").GetComponent<UniversalStatMultiplier>();
        manageWave();
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
    }

    void manageWave()
    {
        if (waveNumber == 1)
        {
            enemySpawner.changeSpawnMaxLimit(30);
        }
        if (waveNumber == 2)
        {
        //     statMultiplier.setUniversalMultiplier(statMultiplier.getDamageMultiplier()*1.2f);

            enemySpawner.changeSpawnMaxLimit(60);
            enemySpawner.changeSpawnRate(2f);
        }
        else if (waveNumber == 3)
        {
        //    statMultiplier.setUniversalMultiplier(statMultiplier.getDamageMultiplier()*1.2f);
            enemySpawner.changeSpawnIdxLimit(1);
            enemySpawner.changeSpawnMaxLimit(90);
            enemySpawner.changeSpawnRate(3f);
        }
        else if (waveNumber == 4)
        {

            enemySpawner.changeSpawnMaxLimit(120);
            enemySpawner.changeSpawnRate(4f);
        }
        else if (waveNumber == 5)
        {
        //    statMultiplier.setUniversalMultiplier(statMultiplier.getDamageMultiplier() * 1.2f);
            enemySpawner.changeSpawnIdxLimit(2);
            enemySpawner.changeSpawnMaxLimit(150);
            enemySpawner.changeSpawnRate(5f);
        }
        else if (waveNumber == 6)
        {
            enemySpawner.changeSpawnMaxLimit(180);
            enemySpawner.changeSpawnRate(6f);
        }
        else if (waveNumber == 7)
        {
        //    statMultiplier.setUniversalMultiplier(statMultiplier.getDamageMultiplier() * 1.4f);
            enemySpawner.changeSpawnIdxLimit(3);
            enemySpawner.changeSpawnRate(7f);
            enemySpawner.changeSpawnMaxLimit(210);
        }
        else if (waveNumber == 8)
        {
            enemySpawner.changeSpawnRate(8f);
            enemySpawner.changeSpawnMaxLimit(240);
        }
        else if (waveNumber == 9)
        {
        //    statMultiplier.setUniversalMultiplier(statMultiplier.getDamageMultiplier() * 1.5f);
            enemySpawner.changeSpawnMaxLimit(270);
            enemySpawner.changeSpawnRate(9f);
        }
        else if (waveNumber == 10)
        {
        //    statMultiplier.setUniversalMultiplier(statMultiplier.getDamageMultiplier() * 1.5f);
            enemySpawner.SpawnBoss();
            enemySpawner.changeSpawnMaxLimit(300);
            enemySpawner.changeSpawnRate(10f);

        }

    }
 
    IEnumerator WaveNumberIncrease()
    {
        waveTextAnimator.SetTrigger("increase");
        yield return new WaitForSeconds(0.7f);
        waveNumber++;
        manageWave();
    }
    public int getWaveNumber()
    {
        return waveNumber;
    }
}
