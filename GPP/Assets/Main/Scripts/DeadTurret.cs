using System;
using UnityEngine;

public class DeadTurret : MonoBehaviour
{
    
    [SerializeField] private float _fireRate = 1f;
    [SerializeField] private int _damage = 10;
    [SerializeField] private GameObject _BulletSpawnPos;
    [SerializeField] private GameObject _Shooter;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _detectionRange = 15f;

    [Header("Audio")]
    [SerializeField] private AudioSource _turretShotSfx; 

    private GameObject _enemyTarget;
    private float _fireCooldown = 0f;

    void Update()
    {
        _fireCooldown += Time.deltaTime;

        SearchForEnemies();

        if (_enemyTarget != null)
        {
            if(Vector3.Distance(transform.position, _enemyTarget.transform.position) > _detectionRange)
            {
                _enemyTarget = null;
                return;
            }
            Vector3 enemyPos = _enemyTarget.transform.position;
            Vector3 rotateEnemie = new Vector3(enemyPos.x, transform.position.y, enemyPos.z);

            _Shooter.transform.LookAt(rotateEnemie);

            if(_fireCooldown >= _fireRate)
            {
                GameObject bulletObj = Instantiate(_bulletPrefab, _BulletSpawnPos.transform.position, _Shooter.transform.rotation);

                Vector3 direction = _enemyTarget.transform.position - _BulletSpawnPos.transform.position;
                bulletObj.GetComponent<Bullet>().SetDirection(direction);
                bulletObj.GetComponent<Bullet>().SetDamage(_damage);

                _fireCooldown = 0f;

                //SND: Turret Shot
                if (_turretShotSfx != null)
                {
                    _turretShotSfx.Play();
                }
            }
        } 
    }

    void SearchForEnemies()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Ghost");

        if (enemies.Length == 0)
            return;

        GameObject closestEnemy = enemies[0];

        foreach (GameObject enemy in enemies)
        {
            float oldDist = Vector3.Distance(transform.position, closestEnemy.transform.position);
            float newDist = Vector3.Distance(transform.position, enemy.transform.position);

            if (newDist < oldDist)
            {
                closestEnemy = enemy;
            }
        }

        _enemyTarget = closestEnemy;
    }

    public void AddDamage(int damage)
    {
        _damage += damage;
    }
}
