using UnityEngine;
using UnityEngine.UI;

public class Entity : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] float _MaxHealth = 100;

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
            EntityKilled();
        }
    }

    public virtual void EntityKilled()
    {
        Destroy(gameObject);
    }
}
