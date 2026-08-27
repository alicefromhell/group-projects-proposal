using UnityEngine;

public class BST_LivingTurretdamageIncrease : SkillTreeMultipleUpgrade
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
        LivingTurret[] deadTurrets = FindObjectsOfType<LivingTurret>();
        foreach (LivingTurret deadTurret in deadTurrets)
        {
            deadTurret.UpgradeDamage(10);
        }

        return true;
    }
}
