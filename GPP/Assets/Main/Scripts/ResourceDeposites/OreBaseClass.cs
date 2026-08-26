using UnityEngine;
using UnityEngine.UI;

public class OreBaseClass : MonoBehaviour
{
    [SerializeField] private ResourceType _resourceType;
    [SerializeField] private SchrodingerState _schrodingerState;
    [SerializeField] private Vector2 _resourceAmount;

    [SerializeField] private int detectionRange = 5;

    [SerializeField] private float _reschargeRate;
    [SerializeField] private int _maxCharges;
    [SerializeField] private bool _isDepleted = false;

    [SerializeField] private Slider _rechargeMeter;
    [SerializeField] private ParticleSystem _sparkles;

    private int _charges;
    private float _currentChargeRate;

    private void Start()
    {
        _charges = _maxCharges;
        _currentChargeRate = _reschargeRate;

        // Initial visual state: only sparkles visible when not depleted
        SetVisualsBasedOnLogic();
    }

    private void Update()
    {
        // Recharge logic
        if (_currentChargeRate < _reschargeRate && !_isDepleted)
        {
            _rechargeMeter.value = _currentChargeRate / _reschargeRate;
            _currentChargeRate += Time.deltaTime;
        }
        else
        {
            if (!_isDepleted)
                _currentChargeRate = _reschargeRate; // clamp
        }

        bool isSuperposition = _schrodingerState == SchrodingerState.Superposition;
        bool stateMatches = isSuperposition || GameManager.Instance.CurrentState == _schrodingerState;

        gameObject.GetComponent<Collider>().enabled = stateMatches;
        gameObject.GetComponent<MeshRenderer>().enabled = stateMatches;
        if (TryGetComponent<BoxCollider>(out var box))
            box.enabled = stateMatches; // second collider for the ore, is the trigger

        if (!stateMatches)
        {
            _rechargeMeter.gameObject.SetActive(false);
            _sparkles.gameObject.SetActive(false);
        }
        else
        {
            SetVisualsBasedOnLogic();
        }
    }

    public void Depletion()
    {
        _sparkles.gameObject.SetActive(false);
        _rechargeMeter.gameObject.SetActive(true);
        _charges--;
        _currentChargeRate = 0f;

        if (_charges <= 0)
            _isDepleted = true;
    }

    // Sets slider and particle system to the correct active/inactive state based on current depletion and recharge status.
    private void SetVisualsBasedOnLogic()
    {
        if (_isDepleted)
        {
            _rechargeMeter.gameObject.SetActive(false);
            _sparkles.gameObject.SetActive(false);
        }
        else if (_currentChargeRate < _reschargeRate)
        {
            _rechargeMeter.gameObject.SetActive(true);
            _sparkles.gameObject.SetActive(false);
        }
        else
        {
            _rechargeMeter.gameObject.SetActive(false);
            _sparkles.gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_isDepleted)
        {
            PlayerInventoryManager.Instance.AddResource(
                _resourceType,
                Random.Range((int)_resourceAmount.x, (int)_resourceAmount.y)
            );
            Depletion();
        }
    }
}