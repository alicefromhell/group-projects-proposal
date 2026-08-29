using UnityEngine;
using UnityEngine.InputSystem;

public class Playergun : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private GameObject _muzzle;

    [Header("Audio")]
    [SerializeField] private AudioSource _playerWeaponSfx;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Shoot");

            GameObject bulletObj = Instantiate(_bulletPrefab, _muzzle.transform.position,_muzzle.transform.rotation);
            Vector3 direction = _muzzle.transform.position - transform.position;
            bulletObj.GetComponent<PlayerBullet>().SetDirection(direction);
            Debug.Log("Bullet instantiated");

            //SND: Player Weapon
            if (_playerWeaponSfx != null)
            {
                _playerWeaponSfx.Play();
            }
        }
    }
}
