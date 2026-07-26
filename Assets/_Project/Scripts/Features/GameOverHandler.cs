using Entity;
using EventBusLib;
using UnityEngine;
using Waves;

public class GameOverHandler : MonoBehaviour
{
    [SerializeField] private EntityBase _player;
    [SerializeField] private EnemyWaveManager _enemyWaveManager;

    private void Start()
    {
        if (_player == null)
            return;

        _player.HealthModule.IsOver += Handle;
    }

    private void Handle()
    {
        EventBus.Invoke(new GameOverEvent(_enemyWaveManager.CurrentWaveIndex));
    }

    private void OnDestroy()
    {
        if (_player == null)
            return;

        _player.HealthModule.IsOver -= Handle;
    }
}
