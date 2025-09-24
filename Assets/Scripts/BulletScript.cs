using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.linearVelocity = (Vector3.up * speed);
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "EnemyBomb")
        {
            // Bullet hits bomb - both destroy each other
            Destroy(collision.gameObject); 
            Destroy(this.gameObject); 
        }
    }
}
