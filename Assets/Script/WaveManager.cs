using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    private int waveNumber = 1;
    private EnemySpawner enemySpawner;

    [SerializeField] Text waveText;
    [SerializeField] Animator waveTextAnimator;

    float timer;
    void Start()
    {
        enemySpawner= GetComponent<EnemySpawner>();
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
        if (waveNumber == 2)
        {
            enemySpawner.changeSpawnRate(1f);
        }
        else if (waveNumber == 3)
        {
            enemySpawner.changeSpawnIdxLimit(1);
        }
        else if (waveNumber == 4)
        {
            enemySpawner.changeSpawnRate(2f);
        }
        else if (waveNumber == 5)
        {
            enemySpawner.changeSpawnIdxLimit(2);
        }
        else if (waveNumber == 6)
        {
            enemySpawner.changeSpawnRate(3f);
        }
        else if (waveNumber == 7)
        {
            enemySpawner.changeSpawnIdxLimit(3);
        }
        else if (waveNumber == 8)
        {

        }
        else if (waveNumber == 9)
        {
            enemySpawner.changeSpawnRate(4f);
        }
        else if (waveNumber == 10)
        {

        }

    }

    [ContextMenu("Kaj kor")]
    private void testFunction()
    {
        StartCoroutine(WaveNumberIncrease());
    }

    
    IEnumerator WaveNumberIncrease()
    {
        waveTextAnimator.SetTrigger("increase");
        yield return new WaitForSeconds(0.7f);
        waveNumber++;
    }
}
