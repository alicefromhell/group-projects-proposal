using UnityEngine;
using System.Collections.Generic;
using TMPro;
public enum TurretType
{
    LivingTurret,
    DeadTurret,
    QuantumTurret
    // Add more turret types as needed
}

public struct TurretEntry
{
    public TurretType TurretType;
    public int Amount;
}

public class TurretInventoryManager : MonoBehaviour
{
    #region Singleton
    public static TurretInventoryManager Instance { get; private set; }
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

    private List<TurretEntry> _turretInventory = new List<TurretEntry>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _turretInventory.Add(new TurretEntry { TurretType = TurretType.LivingTurret, Amount = 1 });
        _turretInventory.Add(new TurretEntry { TurretType = TurretType.DeadTurret, Amount = 1 });
        _turretInventory.Add(new TurretEntry { TurretType = TurretType.QuantumTurret, Amount = 0 });
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddTurret(TurretType turretType, int amount)
    {
        int index = _turretInventory.FindIndex(t => t.TurretType == turretType);
        if (index != -1)
        {
            TurretEntry entry = _turretInventory[index];
            entry.Amount += amount;
            _turretInventory[index] = entry;
        }
        else
        {
            _turretInventory.Add(new TurretEntry { TurretType = turretType, Amount = amount });
        }
    }

    public void RemoveTurret(TurretType turretType, int amount)
    {
        int index = _turretInventory.FindIndex(t => t.TurretType == turretType);
        if (index != -1)
        {
            TurretEntry entry = _turretInventory[index];
            entry.Amount -= amount;
            if (entry.Amount < 0) entry.Amount = 0; // Prevent negative amounts
            _turretInventory[index] = entry;
        }
    }

    public int GetTurretAmount(TurretType turretType)
    {
        TurretEntry entry = _turretInventory.Find(t => t.TurretType == turretType);
        return entry.Amount;
    }

}
