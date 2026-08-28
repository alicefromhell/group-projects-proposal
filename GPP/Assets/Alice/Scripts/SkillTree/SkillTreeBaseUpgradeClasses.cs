using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum SkillTreeNodeStates
{
    Locked,
    Unlocked,
    FullyUpgraded
}

// Skill tree node base class.
// - Dependancy: node that must unlock this one.
// - UnlocksNextNode: when true, dependent nodes (that list this as Dependancy) become unlocked.
// - IsUpgraded: true when this node is considered "complete" (e.g. single upgrade done, or repeatable node maxed).
// - NodeState: drives UI (Locked/Unlocked/FullyUpgraded).
//
// Subclasses must set UnlocksNextNode = true at the appropriate time:
//   - Single-upgrade nodes: immediately after first upgrade.
//   - Repeatable nodes: either after first upgrade or after max upgrades, depending on design.
//   - Infinite nodes: typically after first upgrade.

public class SkillTreeBaseClass : MonoBehaviour
{
    // attributes
    public bool IsUpgraded = false;
    [SerializeField] public SkillTreeBaseClass Dependancy = null;
    [SerializeField] public SkillTreeBaseClass Lock = null;
    [SerializeField] public int Cost;
    public bool UnlocksNextNode = false;
    public SkillTreeNodeStates NodeState = SkillTreeNodeStates.Locked;

    public string _buttonName = "Upgrade";
    private TMP_Text _buttonText;

    // operations
    virtual public void Start()
    {
        _buttonText = GetComponentInChildren<TMP_Text>();

        UpdateButton();
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

    public virtual bool OnUpgraded()
    {
        if (PlayerInventoryManager.Instance.GetResourceAmount(ResourceType.Money) < Cost)
        {
            Debug.Log("Not enough money to upgrade!");
            return false;
        }

        PlayerInventoryManager.Instance.RemoveMoney(Cost);
        UpdateButton();
        return true;
    }

    private void StateManager()
    {
        Button button = GetComponent<Button>();

        switch (NodeState)
        {
            case SkillTreeNodeStates.Locked:
                if (Dependancy != null && Dependancy.UnlocksNextNode)
                {
                    NodeState = SkillTreeNodeStates.Unlocked;
                }

                //visual feedback for locked nodes can be added here, like changing the color or disabling the button

                if (button != null)
                {
                    button.interactable = false;
                }

                break;

            case SkillTreeNodeStates.Unlocked:
                if (IsUpgraded)
                {
                    NodeState = SkillTreeNodeStates.FullyUpgraded;
                }

                //visual feedback for unlocked nodes can be added here, like changing the color or enabling the button

                if (button != null)
                {
                    button.interactable = true;
                }

                break;

            case SkillTreeNodeStates.FullyUpgraded:

                //visual feedback for fully upgraded nodes can be added here, like changing the color or disabling the button

                if (button != null)
                {
                    button.interactable = false;
                }

                break;

        }
    }
    public void UpdateButton()
    {
        if (_buttonText != null)
        {
            _buttonText.text = _buttonName + " (" + Cost + ")";
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

    public override bool OnUpgraded()
    {
        if (!base.OnUpgraded())
            return false;

        IsUpgraded = true;
        UnlocksNextNode = true;

        return true;
    }
}

public class SkillTreeMultipleUpgrade : SkillTreeBaseClass
{
    [SerializeField] public int MaxAmountOfUpgrades;
    public int CurrentUpgradeAmount;
    [SerializeField] public float CostMultiplier;
    public override void Update()
    {
        base.Update();
    }
    public override void OnClicked()
    {
        base.OnClicked();
    }

    // Base implementation handles payment and incrementing upgrade count.
    // Derived classes decide when to set UnlocksNextNode = true:
    //   - Unlock on first upgrade: set UnlocksNextNode = true the first time OnUpgraded() succeeds.
    //   - Unlock on max: set UnlocksNextNode = true when CurrentUpgradeAmount >= MaxAmountOfUpgrades.
    public override bool OnUpgraded()
    {
        if (!base.OnUpgraded())
            return false;

        if (CurrentUpgradeAmount < MaxAmountOfUpgrades)
        {
            CurrentUpgradeAmount++;
            Cost = Mathf.FloorToInt(Cost * CostMultiplier);
            base.UpdateButton();
        }
        else 
        {
            IsUpgraded = true;
            Debug.Log("Max upgrades reached!");
        }

        return true;
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
    public override bool OnUpgraded()
    {
        if (!base.OnUpgraded())
            return false;

        //infinite upgrades, so the next upgrade is always available if this one is upgraded once
        UnlocksNextNode = true;

        //increase the cost for the next upgrade
        Cost = Mathf.FloorToInt(Cost * CostMultiplier);
        base.UpdateButton();

        return true;
    }
}