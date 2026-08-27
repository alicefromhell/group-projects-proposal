using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    private int _damage;
    [SerializeField] private float _travelDistance = 25f;

    private float _movementTime = 0f;

    private Vector3 _BulletDirection;

    public void SetDirection(Vector3 direction)
    {
        _BulletDirection = direction;
    }

    public void SetDamage(int damage)
    {
        _damage = damage;
    }

    private void Update()
    {
        _movementTime += Time.deltaTime;

        transform.position += _BulletDirection * _speed * Time.deltaTime;

        if(_movementTime > _travelDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie") || other.CompareTag("Ghost"))
        {
            other.GetComponent<Entity>().DoDamage(_damage);
        }

        Destroy(gameObject);
    }
}
