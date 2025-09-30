using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Steve;
    [Header("Sound Resouces")]

    public AudioSource backgroundMusic;
    public AudioClip enemyAdvanceSound;
    public GameObject enemyExplosionSoundPrefab;
    private AudioSource thisAudio;
    private void Awake()
    {
        Steve = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        thisAudio = GetComponent<AudioSource>();
    }

    public void MakeEnemyAdvanceSound()
    {
        thisAudio.PlayOneShot(enemyAdvanceSound);
    }

    public void MakeEnemyExplosionSound()
    {
        GameObject thisSound = Instantiate(enemyExplosionSoundPrefab);
        Destroy(thisSound, 1.5f);
    }
}
