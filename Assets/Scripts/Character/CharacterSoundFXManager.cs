using UnityEngine;

public class CharacterSoundFXManager : MonoBehaviour
{
    private AudioSource audioSource;

    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayRollSound()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(WorldSoundFXManager.Instance.rollClip);
    }
}
