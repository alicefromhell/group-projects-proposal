using UnityEngine;

public class SkillTreeBaseClass : MonoBehaviour
{
    // attributes
    public bool IsUpgraded = false;
    [SerializeField] public SkillTreeBaseClass Dependancy = null;
    [SerializeField] public SkillTreeBaseClass Lock = null;
    [SerializeField] public int Cost;
    public bool UnlocksNextNode = false;

    // operations
    virtual public void Start()
    {
    }

    virtual public void Update()
    {

    }

    virtual public void OnClicked()
    {
        if(Dependancy != null && !Dependancy.UnlocksNextNode)
        {
            Debug.Log("You need to upgrade the previous node first!");
            return;
        }
    }

    virtual public void OnUpgraded()
    {
    }
}

public class SkillTreeBaseUpgrade : SkillTreeBaseClass
{
    public override void OnUpgraded()
    {
        IsUpgraded = true;
        UnlocksNextNode = true;
    }
}

public class SkillTreeMultipleUpgrade : SkillTreeBaseClass
{
    [SerializeField] public int MaxAmountOfUpgrades;
    private int _currentUpgradeAmount;
    [SerializeField] public float CostMultiplier;
}

public class SkillTreeInfiniteUpgrade : SkillTreeBaseClass
{
    private int _currentUpgradeAmount;
    [SerializeField] public float CostMultiplier;
}