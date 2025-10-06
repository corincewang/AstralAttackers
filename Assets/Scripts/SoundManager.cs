using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Steve;
    [Header("Sound Resouces")]

    public AudioClip backgroundMusic;
    private AudioSource backgroundMusicSource;
    public AudioClip enemyAdvanceSound;
    public AudioClip playerExplosion;
    public AudioClip happySound; 
    public GameObject enemyExplosionSoundPrefab;
    

    public AudioClip bossMusic;
    private AudioSource thisAudio;
    private void Awake()
    {
        if (Steve && Steve != this)
        {
            Debug.Log("Destroying duplicate SoundManager");
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
        backgroundMusicSource = GetComponent<AudioSource>();
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
        if (backgroundMusic != null && backgroundMusicSource != null)
        {
            backgroundMusicSource.clip = backgroundMusic;
            backgroundMusicSource.Play();
        }
    }

    public void StopTheMusic()
    {
        if (backgroundMusicSource != null)
        {
            backgroundMusicSource.Stop();
        }
    }
    
    public void StartBossMusic()
    {
        if (bossMusic != null && backgroundMusicSource != null)
        {
            backgroundMusicSource.Stop();
            backgroundMusicSource.clip = bossMusic;
            backgroundMusicSource.Play();
        }
    }

    public void PlayerExplosionSequence()
    {
        thisAudio.PlayOneShot(playerExplosion);
    }
}
