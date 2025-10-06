using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Steve;
    [Header("Sound Resouces")]

    public AudioSource backgroundMusic;
    public AudioClip enemyAdvanceSound;
    public AudioClip playerExplosion;
    public AudioClip happySound; 
    public GameObject enemyExplosionSoundPrefab;
    

    public AudioClip bossMusic;
    private AudioSource thisAudio;
    private void Awake()
    {
        if (Steve)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Steve = this;
            DontDestroyOnLoad(this.gameObject);
        }
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
        GameObject thisSound = Instantiate(enemyExplosionSoundPrefab, transform);
        Destroy(thisSound, 1.5f);
    }

    public void MakePlayerExplosionSound()
    {
        thisAudio.PlayOneShot(playerExplosion);
    }
    
    public void MakeHappySound()
    {

        thisAudio.PlayOneShot(happySound);
        
    }

    public void StartTheMusic()
    {
        backgroundMusic.Play();
    }

    public void StopTheMusic()
    {
        backgroundMusic.Stop();
    }
    
    public void StartBossMusic()
    {
        if (bossMusic != null)
        {
            backgroundMusic.clip = bossMusic;
            backgroundMusic.Play();
        }
    }

    public void PlayerExplosionSequence()
    {
        StopTheMusic();

        AudioSource[] audioSources = GetComponentsInChildren<AudioSource>();
        foreach (AudioSource childSource in audioSources)
        {
            childSource.Stop();
        }

        thisAudio.PlayOneShot(playerExplosion);
    }
}
