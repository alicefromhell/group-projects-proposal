using UnityEngine;
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

    private VisualElement _root;
    private Label _moneyLabel;

    void OnEnable()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;

        var grid = _root.Q<VisualElement>("Inventory");

        _moneyLabel = _root.Q<Label>("Money");
        _moneyLabel.text = _money.ToString();

        for (int i = 0; i < _buildingSlotData.Length; i++)
        {
            var slot = _slotTemplate.Instantiate();

            var icon = slot.Q<Image>("Icon");
            icon.sprite = _buildingSlotData[i].Icon;

            var cost = slot.Q<Label>("Price");
            cost.text = _buildingSlotData[i].Cost.ToString();

            grid.Add(slot);
        }
    }
}
