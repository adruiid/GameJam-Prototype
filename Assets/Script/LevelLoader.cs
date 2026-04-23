using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] Animator transition;

    [SerializeField] float TransitionTime = 1f;

    [SerializeField] GameObject menuCanvas;

    [SerializeField] AudioSource audioSource;

    void Update()
    {

    }

    public void LoadLastLevel()
    {
        Time.timeScale = 1f;
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex - 1));
    }

    public void LoadNextLevel()
    {
        StartCoroutine(FadeOutCoroutine(1f));
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelIdx)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1f);

        menuCanvas.SetActive(false);

        yield return new WaitForSeconds(TransitionTime);

        SceneManager.LoadScene(levelIdx);

    }

    IEnumerator FadeOutCoroutine(float duration)
    {
        float startVolume = audioSource.volume;

        float time = 0f;
        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, time / duration);
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();
    }
}
