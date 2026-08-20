using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Inventory : MonoBehaviour
{
    [System.Serializable]
    struct BuildingSlotData
    {
        [SerializeField] private int cost;
        [SerializeField] private Sprite icon;
        [SerializeField] private GameObject buildingPrefab;

        public int Cost => cost;
        public Sprite Icon => icon;
        public GameObject BuildingPrefab => buildingPrefab;
    }

    [SerializeField] int _money = 200;
    [SerializeField] private VisualTreeAsset _slotTemplate;
    [SerializeField] private BuildingSlotData[] _buildingSlotData;

    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private Camera _cam;


    private VisualElement _root;
    private Label _moneyLabel;
    private bool _HoveringOverInv = false;

    private int _totalMoney;
    private int _costBuilding = -1;
    private GameObject _currentBuilding;


    void OnEnable()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;

        var grid = _root.Q<VisualElement>("Inventory");

        grid.RegisterCallback<PointerEnterEvent>(evt => _HoveringOverInv = true);
        grid.RegisterCallback<PointerLeaveEvent>(evt => _HoveringOverInv = false);

        _moneyLabel = _root.Q<Label>("Money");
        _moneyLabel.text = _money.ToString();

        for (int i = 0; i < _buildingSlotData.Length; i++)
        {
            var slot = _slotTemplate.Instantiate();

            var icon = slot.Q<Image>("Icon");
            icon.sprite = _buildingSlotData[i].Icon;

            var cost = slot.Q<Label>("Price");
            cost.text = _buildingSlotData[i].Cost.ToString();

            int index = i;
            slot.RegisterCallback<PointerDownEvent>(evt => SpawnBuilding(index));

            grid.Add(slot);
        }
    }

    private void SpawnBuilding(int index)
    {
        var cost = _buildingSlotData[index].Cost;

        if (cost > _money)
            return;

        _costBuilding = cost;
        var prefab = _buildingSlotData[index].BuildingPrefab;
        
        _currentBuilding = Instantiate(prefab);
    }

    private void Update()
    {
        if(_currentBuilding == null) return;

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            if(_currentBuilding.activeSelf)
            {
                _currentBuilding = null;
                _money -= _costBuilding;
                _moneyLabel.text = _money.ToString();
            }
            else
            {
                Destroy(_currentBuilding);
                _currentBuilding = null;
            } 
        }
        else
        {
            Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
            Ray ray = _cam.ScreenPointToRay(mouseScreenPos);

            if (Physics.Raycast(ray, out RaycastHit hit, 500f, _groundMask))
            {
                _currentBuilding.SetActive(true);
                _currentBuilding.transform.position = hit.point;
            }
            else
            {
                _currentBuilding.SetActive(false);
            }
        }
    }
}
