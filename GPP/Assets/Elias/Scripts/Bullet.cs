using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private int _damage = 50;

    private float _movementTime = 0f;

    private Vector3 _BulletDirection;

    public void SetDirection(Vector3 direction)
    {
        _BulletDirection = direction;
    }

    private void Update()
    {
        _movementTime += Time.deltaTime;

        transform.position += _BulletDirection * _speed * Time.deltaTime;

        if(_movementTime > 5f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Entity>().DoDamage(_damage);
        }

        Destroy(gameObject);
    }
}
