using UnityEngine;

public class ShieldScript : MonoBehaviour
{
    void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.tag == "EnemyBomb")
        {
            Destroy(collision.gameObject); 
            Destroy(this.gameObject); 
        }
    }
}