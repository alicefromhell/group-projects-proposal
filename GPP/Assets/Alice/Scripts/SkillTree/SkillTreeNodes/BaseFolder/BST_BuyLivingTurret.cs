using UnityEngine;

public class BST_BuyLivingTurret: SkillTreeBaseUpgrade
{
    public override void Start()
    {
        _buttonName = "Buy Living Turret";


        base.Start();

        NodeState = SkillTreeNodeStates.Unlocked;

    }

    public override void OnClicked()
    {
        base.OnClicked();

        //no dependancy, is the first node in a branch
        if (!IsUpgraded) { OnUpgraded(); }
    }

    public override bool OnUpgraded()
    {
        if (!base.OnUpgraded())
            return false;

        Debug.Log("Living Turret purchased!");

        TurretInventoryManager.Instance.AddTurret(TurretType.LivingTurret,1);

        return true;
    }
}

