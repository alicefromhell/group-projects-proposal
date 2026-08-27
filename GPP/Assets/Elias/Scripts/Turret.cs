using System;
using UnityEngine;

public class Turret : MonoBehaviour
{
    
    [SerializeField] private float _fireRate = 1f;
    [SerializeField] private int _damage = 10;
    [SerializeField] private GameObject _BulletSpawnPos;
    [SerializeField] private GameObject _Shooter;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _detectionRange = 55f;

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
            }
        } 
    }

    void SearchForEnemies()
    {
        var zombies = GameObject.FindGameObjectsWithTag("Zombie");
        var ghosts = GameObject.FindGameObjectsWithTag("Ghost");

        int totalEnemies = zombies.Length + ghosts.Length;
        if (totalEnemies == 0)
            return;

        GameObject[] enemies = new GameObject[totalEnemies];
        zombies.CopyTo(enemies, 0);
        ghosts.CopyTo(enemies, zombies.Length);

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

    public void UpgradeDamage(int damage)
    {
        _damage += damage;
    }
}
