using UnityEngine;

[CreateAssetMenu(fileName = "RewardData", menuName = "RewardData")]
public class RewardData : ScriptableObject
{
    [Header("Global")]
    [SerializeField] private float _globalWaveReward = 10f;
    [SerializeField] private float _completedAllWavesReward = 500f;
    [Header("Local")]
    [SerializeField] private float _localKillReward = 6f;

    public float GlobalWaveReward => _globalWaveReward;
    public float CompletedAllWavesReward => _completedAllWavesReward;
    public float LocalKillReward => _localKillReward;
}
