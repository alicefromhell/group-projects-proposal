using UnityEngine;

public class BST_DeadTurretdamageIncrease : SkillTreeMultipleUpgrade
{


    private void Awake()
    {
    }

    public override void OnClicked()
    {
        base.OnClicked();

        if(!IsUpgraded)
            OnUpgraded();
    }

    public override bool OnUpgraded()
    {
        if (!base.OnUpgraded())
            return false;

        UnlocksNextNode = true;


        //find all dead turrets in the scene and increase their damage
        DeadTurret[] deadTurrets = FindObjectsOfType<DeadTurret>();
        foreach (DeadTurret deadTurret in deadTurrets)
        {
            deadTurret.AddDamage(10);
        }

        return true;
    }
}
