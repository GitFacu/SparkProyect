using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _timeToDelete = 1.5f;
    [SerializeField] private float _speed = 100f;
    private Rigidbody _rb;
    private Transform _target;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        Destroy(gameObject, _timeToDelete);

        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 dir = (transform.forward).normalized;
        _rb.MovePosition(transform.position + dir * _speed * Time.fixedDeltaTime);

        
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IDamage damage))
        {
            damage.TakeDamage(_damage);
            Destroy(gameObject);
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }
}
