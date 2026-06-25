using Entity.Friendly.Player;
using EventBusLib;
using UnityEngine;
using Waves;

public class GameOverHandler : MonoBehaviour
{
    [SerializeField] private PlayerEntity _playerContainer;
    [SerializeField] private EnemyWaveManager _enemyWaveManager;

    private void Start()
    {
        _playerContainer.HealthModule.IsOver += Handle;
    }

    private void Handle()
    {
        EventBus.Invoke(new GameOverEvent(_enemyWaveManager.CurrentWaveIndex));
    }

    private void OnDestroy()
    {
        _playerContainer.HealthModule.IsOver -= Handle;
    }
}
