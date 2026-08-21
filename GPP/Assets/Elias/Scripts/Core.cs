using System;
using UnityEngine;

public class Core : MonoBehaviour, IObserver
{
    [SerializeField] GameObject _EnemiesParent;
    [SerializeField] GameObject _WaveSystem;

    void Start()
    {
        GetComponent<Entity>().AddObserver(this);
    }

    public void OnNotify(string action)
    {
        if(action == "CoreDestroyed")
        {
            Debug.Log("Game Ended");
            Destroy(_EnemiesParent);
            Destroy(_WaveSystem);
        }
    }
}
