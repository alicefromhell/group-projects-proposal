using UnityEngine;

public class PST_HealthIncrease2 : SkillTreeInfiniteUpgrade
{
    [SerializeField] private int _healthIncreaseAmount = 10;

    public override void Start()
    {
        base.Start();

        Cost = 20;
        CostMultiplier = 1.075f;

    }

    public override void OnClicked()
    {
        base.OnClicked();

        if (Dependancy.IsUpgraded && !IsUpgraded)
        {
            OnUpgraded();
        }

    }

    public override void OnUpgraded()
    {
        base.OnUpgraded();

        Debug.Log("Health increased by 10!");
    }
}