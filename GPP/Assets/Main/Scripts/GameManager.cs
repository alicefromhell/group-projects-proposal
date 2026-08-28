using UnityEngine;

public enum SchrodingerState
{
    Dead,
    Alive,
    Superposition
}

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance { get; private set; }
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

    public SchrodingerState CurrentState;

    [Header("Volumes")]
    [SerializeField] private GameObject _deadVolume;
    [SerializeField] private GameObject _aliveVolume;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        CurrentState = SchrodingerState.Alive; // Initial state

        if (_deadVolume != null) _deadVolume.SetActive(CurrentState == SchrodingerState.Dead);
        if (_aliveVolume != null) _aliveVolume.SetActive(CurrentState == SchrodingerState.Alive);
    }

    public void ToggleState()
    {
        CurrentState = CurrentState == SchrodingerState.Alive ? SchrodingerState.Dead : SchrodingerState.Alive;
        Debug.Log($"Current state: {CurrentState}");

        // Update volumes based on the current state
        if (_deadVolume != null) _deadVolume.SetActive(CurrentState == SchrodingerState.Dead);
        if (_aliveVolume != null) _aliveVolume.SetActive(CurrentState == SchrodingerState.Alive);
    }
}
