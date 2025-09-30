using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed;
    public float lifeTime; 
    
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.linearVelocity = (Vector3.up * speed);
        
        Destroy(this.gameObject, lifeTime);
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "EnemyBomb")
        {
            Destroy(collision.gameObject); 
            Destroy(this.gameObject); 
        }
    
    }
}
