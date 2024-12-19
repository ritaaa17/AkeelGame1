using UnityEngine;
using System.Collections;

public class AudioOnCollision : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component missing from this game object");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
            // Hide sprite renderer
            GetComponent<SpriteRenderer>().enabled = false;
            StartCoroutine(DestroyAfterAudio());
        }
    }

    private IEnumerator DestroyAfterAudio()
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        Destroy(gameObject);
    }
}