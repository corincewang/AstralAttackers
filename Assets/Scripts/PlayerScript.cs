using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float speed;
    public float offset;
    public GameObject bulletprefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Gary.state == GameState.Playing)
        {
            movePlayer();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                FireBullet();
            }
        }


    }

    private void movePlayer()
    {
        Vector3 currentPosition = transform.position;

        // offset by speed * input * deltatime
        currentPosition.x = currentPosition.x + (Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime);

        currentPosition.x = Mathf.Clamp(currentPosition.x, -offset, offset);

        transform.position = currentPosition;
    }

    private void FireBullet()
    {
        GameObject bulletObject = Instantiate(bulletprefab, (transform.position + Vector3.up), Quaternion.identity);

        Destroy(bulletObject, 5f);
    }

    void OnCollisionEnter(Collision collision)
    {
        if ((collision.transform.tag == "EnemyBomb") && GameManager.Gary.state == GameState.Playing)
        {
            GameManager.Gary.PlayerWasDestroyed();
            SoundManager.Steve.MakePlayerExplosionSound();
            Destroy(collision.gameObject);
            Destroy(this.gameObject, 0.3f);
        }
    }

}
