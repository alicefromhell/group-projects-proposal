using UnityEngine;

public class BST_BuyDeadTurret: SkillTreeBaseUpgrade
{
    public override void Start()
    {


        base.Start();


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

        Debug.Log("Dead Turret purchased!");

        TurretInventoryManager.Instance.AddTurret(TurretType.DeadTurret,1);

        return true;
    }
}

