using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuilderScript : MonoBehaviour
{
    [SerializeField] private GameObject _builderUI;
    [SerializeField] private Camera _buildCamera;
    [SerializeField] private Camera _mainCamera;

    [Header("Building Spots")]
    [SerializeField] private List<GameObject> _buildingSpots;

    [Header("Building Prefabs")]
    [SerializeField] private GameObject _livingTurretPrefab;
    [SerializeField] private GameObject _deadTurretPrefab;
    [SerializeField] private GameObject _quantumTurretPrefab;
    private GameObject _currentTurretPrefab;

    [Header("Turret UI")]
    [SerializeField] private TMP_Text _livingTurretAmountText;
    [SerializeField] private Image _livingTurretImage;

    [SerializeField] private TMP_Text _deadTurretAmountText;
    [SerializeField] private Image _deadTurretImage;

    [SerializeField] private TMP_Text _quantumTurretAmountText;
    [SerializeField] private Image _quantumTurretImage;

    [Header("Audio")]
    [SerializeField] private AudioSource _turretBuildSfx;
    [SerializeField] private AudioSource _ui;

    public void BuildTurret(TurretType turretType, int spotIndex)
    {
        if (spotIndex < 0 || spotIndex >= _buildingSpots.Count)
        {
            Debug.LogError("Invalid building spot index.");
            return;
        }
        GameObject turretPrefab = null;
        switch (turretType)
        {
            case TurretType.LivingTurret:
                if (!(TurretInventoryManager.Instance.GetTurretAmount(TurretType.LivingTurret) >= 1))
                {
                    Debug.LogError("Not enough Living Turrets in inventory.");
                    return;
                }
                turretPrefab = _livingTurretPrefab;
                TurretInventoryManager.Instance.RemoveTurret(TurretType.LivingTurret, 1);
                break;
            case TurretType.DeadTurret:
                if (!(TurretInventoryManager.Instance.GetTurretAmount(TurretType.DeadTurret) >= 1))
                {
                    Debug.LogError("Not enough Dead Turrets in inventory.");
                    return;
                }
                turretPrefab = _deadTurretPrefab;
                TurretInventoryManager.Instance.RemoveTurret(TurretType.DeadTurret, 1);
                break;
            case TurretType.QuantumTurret:
                if (!(TurretInventoryManager.Instance.GetTurretAmount(TurretType.QuantumTurret) >= 1))
                {
                    Debug.LogError("Not enough Quantum Turrets in inventory.");
                    return;
                }
                turretPrefab = _quantumTurretPrefab;
                TurretInventoryManager.Instance.RemoveTurret(TurretType.QuantumTurret, 1);
                break;
            default:
                Debug.LogError("Invalid turret type.");
                return;
        }
        Instantiate(turretPrefab, _buildingSpots[spotIndex].transform.position, Quaternion.identity);

        //SND: Build Turret
        if (_turretBuildSfx != null)
        {
            _turretBuildSfx.Play();
        }
    }

    public void RemoveTurret(int spotIndex) //missing logic to add turret back to inventory when removed
    {
        if (spotIndex < 0 || spotIndex >= _buildingSpots.Count)
        {
            Debug.LogError("Invalid building spot index.");
            return;
        }
        Transform spotTransform = _buildingSpots[spotIndex].transform;
        if (spotTransform.childCount > 0)
        {
            Destroy(spotTransform.GetChild(0).gameObject);
        }
    }

    private void Update()
    {
        _livingTurretAmountText.text = $"Living Turrets: {TurretInventoryManager.Instance.GetTurretAmount(TurretType.LivingTurret)}";
        _deadTurretAmountText.text = $"Dead Turrets: {TurretInventoryManager.Instance.GetTurretAmount(TurretType.DeadTurret)}";
        _quantumTurretAmountText.text = $"Quantum Turrets: {TurretInventoryManager.Instance.GetTurretAmount(TurretType.QuantumTurret)}";

        //set the color of the selected turret image to green, set the color of the unselected turret images to white
        if (_currentTurretPrefab == _livingTurretPrefab)
        {
            _livingTurretImage.color = Color.green;
            _deadTurretImage.color = Color.white;
            _quantumTurretImage.color = Color.white;
        }
        else if (_currentTurretPrefab == _deadTurretPrefab)
        {
            _livingTurretImage.color = Color.white;
            _deadTurretImage.color = Color.green;
            _quantumTurretImage.color = Color.white;
        }
        else if (_currentTurretPrefab == _quantumTurretPrefab)
        {
            _livingTurretImage.color = Color.white;
            _deadTurretImage.color = Color.white;
            _quantumTurretImage.color = Color.green;
        }
        //set the color of any turret with 0 amount in inventory to red
        if (TurretInventoryManager.Instance.GetTurretAmount(TurretType.LivingTurret) <= 0)
        {
            _livingTurretImage.color = Color.red; 
        }
        else if (TurretInventoryManager.Instance.GetTurretAmount(TurretType.LivingTurret) > 0 && _currentTurretPrefab != _livingTurretPrefab)
        {
            _livingTurretImage.color = Color.white;
        }
        if (TurretInventoryManager.Instance.GetTurretAmount(TurretType.DeadTurret) <= 0)
        {
            _deadTurretImage.color = Color.red;
        }
        else if (TurretInventoryManager.Instance.GetTurretAmount(TurretType.DeadTurret) > 0 && _currentTurretPrefab != _deadTurretPrefab)
        {
            _deadTurretImage.color = Color.white;
        }
        if (TurretInventoryManager.Instance.GetTurretAmount(TurretType.QuantumTurret) <= 0)
        {
            _quantumTurretImage.color = Color.red;
        }
        else if (TurretInventoryManager.Instance.GetTurretAmount(TurretType.QuantumTurret) > 0 && _currentTurretPrefab != _quantumTurretPrefab)
        {
            _quantumTurretImage.color = Color.white;
        }
    }

    public void OpenBuilder()
    {
        if(_builderUI.activeSelf)
        {
            CloseBuilder();
            return;
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _builderUI.SetActive(true);
        _buildCamera.gameObject.SetActive(true);
        _mainCamera.gameObject.SetActive(false);
        //any other camera in scene should be disabled, this is a temporary solution until we have a camera manager

        //SND: Open Builder
        if (_ui != null)
        {
            _ui.Play();
        }
    }

    public void CloseBuilder()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _builderUI.SetActive(false);

        _mainCamera.gameObject.SetActive(true);
        _buildCamera.gameObject.SetActive(false);
    }

    #region Button Methods

    public void SelectLivingTurret()
    {
        _currentTurretPrefab = _livingTurretPrefab;
    }
    public void SelectDeadTurret()
    {
        _currentTurretPrefab = _deadTurretPrefab;
    }
    public void SelectQuantumTurret()
    {
        _currentTurretPrefab = _quantumTurretPrefab;
    }

    //ensure buttons spaces are set up to call this method with the correct spot index
    public void BuildSelectedTurret(int spotIndex)
    {
        if (_currentTurretPrefab == null)
        {
            Debug.LogError("No turret selected.");
            return;
        }
        TurretType selectedTurretType = TurretType.LivingTurret; // Default to LivingTurret
        if (_currentTurretPrefab == _livingTurretPrefab)
        {
            selectedTurretType = TurretType.LivingTurret;
        }
        else if (_currentTurretPrefab == _deadTurretPrefab)
        {
            selectedTurretType = TurretType.DeadTurret;
        }
        else if (_currentTurretPrefab == _quantumTurretPrefab)
        {
            selectedTurretType = TurretType.QuantumTurret;
        }

        int index = spotIndex - 1;
        if (index < 0 || index >= _buildingSpots.Count)
        {
            Debug.LogError("Invalid building spot index.");
            return;
        }

        // Check inventory before attempting to build
        if (TurretInventoryManager.Instance.GetTurretAmount(selectedTurretType) < 1)
        {
            Debug.LogError("Not enough turrets in inventory.");
            return;
        }

        BuildTurret(selectedTurretType, index);

        // Disable the UI button that triggered this call so the player can't build again on the same spot
        var clicked = EventSystem.current?.currentSelectedGameObject;
        if (clicked != null)
        {
            var btn = clicked.GetComponent<Button>();
            if (btn != null)
                btn.interactable = false;
        }
    }

    public void RemoveTurretFromSpot(int spotIndex)
    {
        RemoveTurret(spotIndex);
    }
    #endregion
}