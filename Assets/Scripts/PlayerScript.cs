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

        Destroy(bulletObject, 5f);
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "EnemyBomb")
        {
            Destroy(collision.gameObject); 
            Destroy(this.gameObject, 0.3f);
        }
    }
}
