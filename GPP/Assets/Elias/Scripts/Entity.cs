using UnityEngine;
using UnityEngine.UI;

public class Entity : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    public void DoDamage(int damage)
    {
        _slider.value -= damage / 100f;

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
