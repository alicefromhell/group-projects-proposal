using UnityEngine;

public class PST_HealthIncrease1 : SkillTreeBaseUpgrade
{
    [SerializeField] private int _healthIncreaseAmount = 10;

    public override void Start()
    {
        base.Start();

        Cost = 10;
    }

    public override void OnClicked()
    {
        base.OnClicked();

        //no dependancy, is the first node in a branch
        if (!IsUpgraded) { OnUpgraded(); }
    }

    public override void OnUpgraded()
    {
        base.OnUpgraded();

        Debug.Log("Health increased by 10!");
        // Increase health by 10

    }
}



