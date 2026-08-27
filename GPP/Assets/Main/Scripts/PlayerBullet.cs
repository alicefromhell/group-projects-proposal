using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private int _damage = 50;
    [SerializeField] private float _travelDistance = 25f;

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

        if (_movementTime > _travelDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie") && GameManager.Instance.CurrentState==SchrodingerState.Alive)
        {
            other.GetComponent<Entity>().DoDamage(_damage);
        }

        if (other.CompareTag("Ghost") && GameManager.Instance.CurrentState==SchrodingerState.Dead)
        {
            other.GetComponent<Entity>().DoDamage(_damage);
        }


        Destroy(gameObject);
    }
}
