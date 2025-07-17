using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] int _damageAmount = 10;
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerHealth>())
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(_damageAmount);
        }
    }


    
}
