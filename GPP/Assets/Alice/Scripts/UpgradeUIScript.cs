using UnityEngine;
using UnityEngine.InputSystem;
//place this on your base
public class UpgradeUIScript : MonoBehaviour
{
    [SerializeField] private GameObject _upgradeUI;
    [SerializeField] private GameObject _promt;

    [SerializeField] private float _interactionRange = 20f;
    [SerializeField] private Transform _playerTransform;


    private void Start()
    {
        _upgradeUI.SetActive(false);
        _promt.SetActive(false);
    }

    private void Update()
    {
        bool inRange = Vector3.Distance(transform.position, _playerTransform.position) < _interactionRange;

        if (inRange && !_upgradeUI.activeSelf)
            _promt.SetActive(true);
        else
            _promt.SetActive(false);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("Interact action performed: " + context.performed);

        if (!context.performed)
            return;

        // Early-out if not in range
        if (Vector3.Distance(transform.position, _playerTransform.position) > _interactionRange)
            return;

        // Toggle UI
        bool newActive = !_upgradeUI.activeSelf;
        _upgradeUI.SetActive(newActive);

        // Prompt visible only when in range AND UI is closed
        _promt.SetActive(!newActive);
    }

}
