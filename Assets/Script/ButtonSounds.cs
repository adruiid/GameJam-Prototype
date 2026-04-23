using UnityEngine;

public class ButtonSounds : MonoBehaviour
{
    [SerializeField] AudioClip hoverClip;
    [SerializeField] AudioClip clickClip;

    [SerializeField] AudioSource source;
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnHover()
    {
        source.PlayOneShot(hoverClip);
    }

    public void OnClick()
    {
        source.PlayOneShot(clickClip);
    }
}
