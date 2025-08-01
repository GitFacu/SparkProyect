using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugTower : MonoBehaviour, IDamage
{
    public static event Action OnDead;
    [SerializeField] private float _radius = 10f;
    [SerializeField] private int _maxHealth = 100; 
    [SerializeField] private float _speedRotate = 30f;
    [SerializeField] private float _shootDelay = 1f;
    [SerializeField] private GameObject _projectile;
    private Player _player;
    private float _distance = 0;
    [SerializeField] private Transform _shootPoint;
    private float _timer = 0;  
    private int _currentHealth = 0;
    [SerializeField] private Transform _head;

    [SerializeField] private BossHealthUI _healthUI;

    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>();
        _currentHealth = _maxHealth;

        _healthUI.ChangeHealth(_currentHealth);
        _healthUI.ShowPanel(false);
    }

    // Update is called once per frame
    void Update()
    {
        _distance = Vector3.Distance(transform.position, _player.transform.position);

        if (_distance <= _radius)
        {
            _timer += Time.deltaTime;
            if (_timer > _shootDelay)
            {
                _timer = 0;
                Shoot();
            }
        }

        _head.LookAt(_player.transform);
    }

    private void Shoot()
    {
        GameObject proj = Instantiate(_projectile, _shootPoint.position, _shootPoint.rotation);
        //proj.GetComponent<Projectile>().SetTarget(_player.transform);
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Boss damage");
        _currentHealth -= damage;
        _healthUI.ChangeHealth(_currentHealth);
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Die();
        }

    }

    public void Die()
    {
        _healthUI.ShowPanel(false);
        OnDead?.Invoke();
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _radius);

    }
}
