using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    Money
    // Add more resource types as needed
}

[System.Serializable]
public struct ResourceEntry
{
    public ResourceType ResourceType;
    public int Amount;
}

public class PlayerInventoryManager : MonoBehaviour
{
    #region Singleton
    public static PlayerInventoryManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    #endregion

    [SerializeField]
    public List<ResourceEntry> Resources = new List<ResourceEntry>();

    public int GetResourceAmount(ResourceType resourceType)
    {
        switch (resourceType)
        {
            case ResourceType.Money:
                return Resources.Find(r => r.ResourceType == ResourceType.Money).Amount;
            // Add more cases for other resource types as needed
            default:
                Debug.LogWarning("Resource type not recognized.");
                return 0;
        }
    }

    private void Start()
    {
        Resources.Add(new ResourceEntry { ResourceType = ResourceType.Money, Amount = 0 });
    }

    public void AddMoney(int amount)
    {
        int index = Resources.FindIndex(r => r.ResourceType == ResourceType.Money);
        if (index != -1)
        {
            var entry = Resources[index];
            entry.Amount += amount;
            Resources[index] = entry;
        }
    }

    public void RemoveMoney(int amount)
    {
        int index = Resources.FindIndex(r => r.ResourceType == ResourceType.Money);
        if (index != -1)
        {
            var entry = Resources[index];
            entry.Amount -= amount;
            Resources[index] = entry;
        }
    }
}
