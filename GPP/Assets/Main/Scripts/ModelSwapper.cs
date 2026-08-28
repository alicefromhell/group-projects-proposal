using UnityEngine;

public class ModelSwapper : MonoBehaviour
{
    [SerializeField] private GameObject _activeModel;
    [SerializeField] private GameObject _inactiveModel;

    [SerializeField] private SchrodingerState _activeState;

    private void Update()
    {
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.CurrentState == _activeState)
            {
                _activeModel.SetActive(true);
                _inactiveModel.SetActive(false);
            }
            else
            {
                _activeModel.SetActive(false);
                _inactiveModel.SetActive(true);
            }
        }
    }
}
