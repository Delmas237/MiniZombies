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
        _playerContainer.HealthController.IsOver += Handle;
    }
    private void OnDestroy()
    {
        _playerContainer.HealthController.IsOver -= Handle;
    }

    private void Handle()
    {
        EventBus.Invoke(new GameOverEvent(_enemyWaveManager.CurrentWaveIndex));
    }
}
