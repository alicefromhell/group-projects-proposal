using UnityEngine;

public class BST_CoreHealthIncrease2 : SkillTreeInfiniteUpgrade
{
    [SerializeField] private Entity _coreEntity;

    
    public override void Start()
    {


        base.Start();

    }
    public override void OnClicked()
    {
        base.OnClicked();

        if (!IsUpgraded) { OnUpgraded(); }
    }

    public override bool OnUpgraded()
    {
        if (!base.OnUpgraded())
            return false;

        Debug.Log("Core Health Increased!");

        _coreEntity.AddMaxHealth(10);

        return true;
    }
}

