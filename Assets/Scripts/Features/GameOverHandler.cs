using EventBusLib;
using PlayerLib;
using UnityEngine;
using WavesLib;

public class GameOverHandler : MonoBehaviour
{
    [SerializeField] private PlayerContainer _playerContainer;
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
