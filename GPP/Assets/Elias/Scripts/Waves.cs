using UnityEngine;
using UnityEngine.UIElements;

public class Waves : MonoBehaviour
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
        [SerializeField] public float interval;
    }

    [System.Serializable]
    public struct Wave
    {
        [SerializeField] public EnemyInfo[] enemiesToSpawn;
    }

    [SerializeField] private Wave[] _Waves;
    [SerializeField] GameObject _EnemiesParent;

    private int _WaveIndex = -1;
    private int _EnemyIndex = -1;
    private float _CurrentInterval = 0;
    private float _Time;
    private Button _SpawnButton;

    private void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        _SpawnButton = root.Q<Button>();
        _SpawnButton.clicked += SpawnNextWave;
    }

    void Update()
    {
        if (_WaveIndex == -1)
            return;

        if (_Waves[_WaveIndex].enemiesToSpawn.Length == _EnemyIndex)
            return;

        _Time += Time.deltaTime;
        if (_Time < _CurrentInterval)
            return;

        System.Random rnd = new System.Random();
        var enemyInfo = _Waves[_WaveIndex].enemiesToSpawn[_EnemyIndex];

        for(int i = 0; i < enemyInfo.amount; i++)
        {
            int a = rnd.Next(-25, 25);
            int b = rnd.Next(10, 25);
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
                    x = a;
                    z = b;
                    break;
                case SpawnPos.west:
                    x = b;
                    z = a;
                    break;
            }

            var enemy = Instantiate(enemyInfo.enemy);
            enemy.transform.position = new Vector3(x, 0, z);
            enemy.transform.SetParent(_EnemiesParent.transform, false);
        }

        _EnemyIndex++;
        _CurrentInterval = enemyInfo.interval;
    }

    void SpawnNextWave()
    {
        _WaveIndex++;

        if(_WaveIndex == _Waves.Length)
            _WaveIndex = 0;

        _EnemyIndex = 0;

        _SpawnButton.SetEnabled(false);
    }
}
