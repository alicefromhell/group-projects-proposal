using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Waves : MonoBehaviour, IObserver
{
    public enum SpawnPos
    {
        north,
        east,
        south,
        west
    }

    [System.Serializable]
    public struct EnemyInfo
    {
        [SerializeField] public GameObject enemy;
        [SerializeField] public SpawnPos pos;
        [SerializeField] public int amount;
    }

    [System.Serializable]
    public struct Wave
    {
        [SerializeField] public EnemyInfo[] enemiesToSpawn;
    }

    [SerializeField] private Wave[] _Waves;
    [SerializeField] GameObject _EnemiesParent;
    [SerializeField] private float _autoStartTimer = 8f;
    [SerializeField] private TMP_Text _timer;

    private int _WaveIndex = -1;
    private int _EnemyIndex = -1;
    private int _EnemiesAlive = 0;
    private Button _SpawnButton;

    private float _timerValue;

    private void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        _SpawnButton = root.Q<Button>();
        _SpawnButton.clicked += SpawnNextWave;

        _timerValue = 15;
    }

    void Update()
    {

        if (_timerValue > 0)
        {
            _timerValue -= Time.deltaTime;
            _timer.text = "Wave Starts in: " + Mathf.RoundToInt(_timerValue).ToString();
            if (_timerValue <= 0)
            {
                SpawnNextWave();
            }
        }

        if (_WaveIndex == -1)
            return;

        if (_Waves[_WaveIndex].enemiesToSpawn.Length == _EnemyIndex)
            return;

        System.Random rnd = new System.Random();
        var enemyInfo = _Waves[_WaveIndex].enemiesToSpawn[_EnemyIndex];

        for(int i = 0; i < enemyInfo.amount; i++)
        {
            int a = rnd.Next(30, 55);
            int b = rnd.Next(30, 55);
            int x = 0;
            int z = 0;

            switch(enemyInfo.pos)
            {
                case SpawnPos.north:
                    x = a;
                    z = -b;
                    break;
                case SpawnPos.east:
                    x = -b;
                    z = a;
                    break;
                case SpawnPos.south:
                    x = -a;
                    z = b;
                    break;
                case SpawnPos.west:
                    x = b;
                    z = a;
                    break;
            }

            var enemy = Instantiate(enemyInfo.enemy);
            enemy.transform.position = new Vector3(x, 0, z);
            enemy.GetComponent<Entity>().AddObserver(this);
            enemy.transform.SetParent(_EnemiesParent.transform, false);
            _EnemiesAlive++;
        }

        _EnemyIndex++;
    }

    void SpawnNextWave()
    {
        _WaveIndex++;

        if(_WaveIndex == _Waves.Length)
            _WaveIndex = 0;

        _EnemyIndex = 0;

        _SpawnButton.SetEnabled(false);
    }

    void EnemyKilled()
    {
        _EnemiesAlive--;

        if (_EnemiesAlive <= 0)
        {
            _SpawnButton.SetEnabled(true);
            _timerValue = _autoStartTimer;
        }
    }

    public void OnNotify(string action)
    {
        if(action == "EnemyKilled")
        {
            EnemyKilled();
        }
    }
}
