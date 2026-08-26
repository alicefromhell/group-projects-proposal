using UnityEngine;

public class MaterialSwapper : MonoBehaviour
{
    [SerializeField] private Material _activeMaterial;
    [SerializeField] private Material _inactiveMaterial;

    private Renderer _renderer;


    [SerializeField] private SchrodingerState _activeState;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        if(GameManager.Instance != null)
        {
            if (GameManager.Instance.CurrentState == _activeState)
            {
                _renderer.material = _activeMaterial;
            }
            else
            {
                _renderer.material = _inactiveMaterial;
            }
        }
    }
}
