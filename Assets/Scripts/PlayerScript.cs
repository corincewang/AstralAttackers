using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float speed;
    public float offset;
    public GameObject bulletprefab;
    public AudioClip playerFireSound, playerExplosionSound;
    private AudioSource audioComponent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioComponent = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentPosition = transform.position;

        // offset by speed * input * deltatime
        currentPosition.x = currentPosition.x + (Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime);

        currentPosition.x = Mathf.Clamp(currentPosition.x, -offset, offset);

        transform.position = currentPosition;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireBullet();
        }
    }

    private void FireBullet()
    {
        GameObject bulletObject = Instantiate(bulletprefab, (transform.position + Vector3.up), Quaternion.identity);
        
        // Play fire sound
        if (audioComponent != null && playerFireSound != null)
        {
            audioComponent.PlayOneShot(playerFireSound);
        }

        Destroy(bulletObject, 5f);
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "EnemyBomb")
        {
            audioComponent.PlayOneShot(playerExplosionSound); 
            Destroy(collision.gameObject); 
            Destroy(this.gameObject, 0.3f);
        }
    }
}
