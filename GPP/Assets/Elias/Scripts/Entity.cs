using UnityEngine;
using UnityEngine.UI;

public class Entity : Subject
{
    [SerializeField] private Slider _slider;
    [SerializeField] float _MaxHealth = 100;

    [Header("Audio")]
    [SerializeField] private AudioSource _enemyDeadSfx;

    private void Start()
    {
        _slider.maxValue = _MaxHealth;
        _slider.value = _MaxHealth;
    }
    public void DoDamage(int damage)
    {
        _slider.value -= damage;

        if (_slider.value <= 0)
        {
            if(GetComponent<Enemy>())
            {
                NotifyObservers("EnemyKilled");
                PlayerInventoryManager.Instance.AddMoney(1);
                RemoveAllObservers();
            }
            if(GetComponent<Core>())
            {
                NotifyObservers("CoreDestroyed");
                RemoveAllObservers();
            }

            EntityKilled();
        }
    }

    public void Heal(int healAmount)
    {
        _slider.value += healAmount;
        if (_slider.value > _MaxHealth)
        {
            _slider.value = _MaxHealth;
        }
    }

    public void AddMaxHealth(int amount)
    {
        _MaxHealth += amount;
        _slider.maxValue = _MaxHealth;

        Heal((int)amount);
    }

    public virtual void EntityKilled()
    {
        _enemyDeadSfx.transform.SetParent(null, true);

        //SND: Enemy Dead
        if (_enemyDeadSfx != null)
        {
            _enemyDeadSfx.Play(); 
        }

        Destroy(gameObject);
    }
}
