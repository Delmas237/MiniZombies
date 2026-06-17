using UnityEngine;

public class TurretEntity : MonoBehaviour, IFriendly
{
    [Header("Modules")]
    [SerializeField] protected EntityHealthModule _healthModule;
    [SerializeField] protected WeaponModule _weaponsModule;
    [SerializeField] protected TurretAttackModule _attackModule;
    [SerializeField] protected TurretRotationModule _rotationModule;
    [SerializeField] protected TurretAnimationModule _animationModule;
    [SerializeField] protected EntityAudioModule _audioModule;

    public Transform Transform => transform;
    public IEntityHealthModule HealthModule => _healthModule;
    public IWeaponModule WeaponModule => _weaponsModule;
    public TurretAttackModule AttackModule => _attackModule;
    public TurretRotationModule RotationModule => _rotationModule;
    public TurretAnimationModule AnimationModule => _animationModule;

    private void Start()
    {
        _healthModule.Initialize();
        _audioModule.Initialize(HealthModule);
        _weaponsModule.Initialize();
        _attackModule.Initialize(WeaponModule);
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
