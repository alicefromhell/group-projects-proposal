using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] private float launchSpeed = 10f;
    [SerializeField] private float launchDuration = 0.3f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log("Player entered the box trigger.");

        GameManager.Instance.ToggleState();

        var cc = other.GetComponent<CharacterController>();
        if (cc == null) return;

        // Determine launch direction: up + backward relative to box or player
        Vector3 launchDir = (Vector3.up * 5f + -other.transform.forward*3).normalized;

        // Start a simple coroutine to apply the launch over time
        other.GetComponent<PlayerLauncher>()?.StopLaunch();
        var launcher = other.GetComponent<PlayerLauncher>();
        if (launcher == null)
            launcher = other.gameObject.AddComponent<PlayerLauncher>();

        launcher.StartLaunch(cc, launchDir * launchSpeed, launchDuration);
    }
}

// Small helper component on the player to apply the launch over time
public class PlayerLauncher : MonoBehaviour
{
    private CharacterController _cc;
    private float _remainingTime;
    private Vector3 _launchVelocity;
    private bool _isLaunching;

    public void StartLaunch(CharacterController cc, Vector3 launchVelocity, float duration)
    {
        _cc = cc;
        _launchVelocity = launchVelocity;
        _remainingTime = duration;
        _isLaunching = true;
    }

    public void StopLaunch()
    {
        _isLaunching = false;
    }

    private void Update()
    {
        if (!_isLaunching) return;

        if (_remainingTime > 0)
        {
            _remainingTime -= Time.deltaTime;
            _cc.Move(_launchVelocity * Time.deltaTime);
        }
        else
        {
            _isLaunching = false;
        }
    }
}