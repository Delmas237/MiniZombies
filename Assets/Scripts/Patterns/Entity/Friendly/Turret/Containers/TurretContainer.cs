using UnityEngine;

public class TurretContainer : MonoBehaviour, IFriendly
{
    [Header("Modules")]
    [SerializeField] protected HealthModule _healthModule;
    [SerializeField] protected WeaponsModule _weaponsModule;
    [SerializeField] protected TurretAttackModule _attackModule;
    [SerializeField] protected TurretRotationModule _rotationModule;
    [SerializeField] protected TurretAnimationModule _animationModule;
    [SerializeField] protected EntityAudioModule _audioModule;

    public Transform Transform => transform;
    public IHealthModule HealthModule => _healthModule;
    public IWeaponsModule WeaponsModule => _weaponsModule;
    public TurretAttackModule AttackModule => _attackModule;
    public TurretRotationModule RotationModule => _rotationModule;
    public TurretAnimationModule AnimationModule => _animationModule;

    private void Awake()
    {
        _healthModule.Initialize();
        _audioModule.Initialize(HealthModule);
        _weaponsModule.Initialize();
        _attackModule.Initialize(WeaponsModule);
        _rotationModule.Initialize(AttackModule);
        _animationModule.Initialize(HealthModule, AttackModule);

        _healthModule.IsOver += OnHealhIsOver;
    }
    private void OnHealhIsOver()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        _attackModule.Attack();
        _rotationModule.Rotate();
        _animationModule.UpdateState();
    }

    private void OnDestroy()
    {
        _healthModule.IsOver -= OnHealhIsOver;
        _attackModule.OnDestroy();
        _animationModule.OnDestroy();
    }
}
