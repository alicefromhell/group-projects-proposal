using UnityEngine;

public enum SkillTreeNodeStates
{
    Locked,
    Unlocked,
    FullyUpgraded
}

public class SkillTreeBaseClass : MonoBehaviour
{
    // attributes
    public bool IsUpgraded = false;
    [SerializeField] public SkillTreeBaseClass Dependancy = null;
    [SerializeField] public SkillTreeBaseClass Lock = null;
    [SerializeField] public int Cost;
    public bool UnlocksNextNode = false;
    public SkillTreeNodeStates NodeState = SkillTreeNodeStates.Locked;

    // operations
    virtual public void Start()
    {
    }

    virtual public void Update()
    {
        StateManager();
    }

    virtual public void OnClicked()
    {
        if (Dependancy != null && !Dependancy.UnlocksNextNode)
        {
            Debug.Log("You need to upgrade the previous node first!");
            return;
        }
    }

    virtual public void OnUpgraded()
    {
        //no player manager yet
        /*
        if (PlayerManager.Instance.Money <= Cost)
        {
            Debug.Log("Not enough money to upgrade!");
            return;
        }
        else
        {
            PlayerManager.Instance.Money -= Cost;
        }
        */

    }

    private void StateManager()
    {
        switch (NodeState)
        {
            case SkillTreeNodeStates.Locked:
                if (Dependancy != null && Dependancy.UnlocksNextNode)
                {
                    NodeState = SkillTreeNodeStates.Unlocked;
                }

                //visual feedback for locked nodes can be added here, like changing the color or disabling the button

                break;

            case SkillTreeNodeStates.Unlocked:
                if (IsUpgraded)
                {
                    NodeState = SkillTreeNodeStates.FullyUpgraded;
                }

                //visual feedback for unlocked nodes can be added here, like changing the color or enabling the button

                break;

            case SkillTreeNodeStates.FullyUpgraded:

                //visual feedback for fully upgraded nodes can be added here, like changing the color or disabling the button
                
                break;

        }
    }
}

public class SkillTreeBaseUpgrade : SkillTreeBaseClass
{
    public override void Update()
    {
        base.Update();
    }
    public override void OnClicked()
    {
        base.OnClicked();
    }

    public override void OnUpgraded()
    {
        base.OnUpgraded();

        //single upgrade, so the next upgrade is always available if this one is upgraded
        IsUpgraded = true;
        UnlocksNextNode = true;
    }
}

public class SkillTreeMultipleUpgrade : SkillTreeBaseClass
{
    [SerializeField] public int MaxAmountOfUpgrades;
    private int _currentUpgradeAmount;
    [SerializeField] public float CostMultiplier;
    public override void Update()
    {
        base.Update();
    }
    public override void OnClicked()
    {
        base.OnClicked();
    }
    public override void OnUpgraded()
    {
        base.OnUpgraded();

        //multiple upgrades have 2 conditions,
        //some upgrades need to be maxed out before the next node is unlocked,
        //some upgrades unlock the next node after the first upgrade.
        //Which is why this has to be set on a case by case basis and not in the base class

        if (_currentUpgradeAmount < MaxAmountOfUpgrades)
        {
            _currentUpgradeAmount++;
            Cost = Mathf.FloorToInt(Cost * CostMultiplier);
        }
        else
        {
            IsUpgraded = true;
            Debug.Log("Max upgrades reached!");
        }
    }
}

public class SkillTreeInfiniteUpgrade : SkillTreeBaseClass
{
    private int _currentUpgradeAmount;
    [SerializeField] public float CostMultiplier;
    public override void Update()
    {
        base.Update();
    }
    public override void OnClicked()
    {
        base.OnClicked();
    }
    public override void OnUpgraded()
    {
        base.OnUpgraded();

        //infinite upgrades, so the next upgrade is always available if this one is upgraded once
        UnlocksNextNode = true;

        //increase the cost for the next upgrade
        Cost = Mathf.FloorToInt(Cost * CostMultiplier);

    }
}