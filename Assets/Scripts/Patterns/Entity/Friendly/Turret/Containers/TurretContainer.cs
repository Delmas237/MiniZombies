using UnityEngine;

public class TurretContainer : MonoBehaviour, IFriendly
{
    [Header("Modules")]
    [SerializeField] protected HealthModule _healthModule;
    [SerializeField] protected WeaponsModule _weaponsModule;
    [SerializeField] protected TurretRotationModule _rotationModule;
    [SerializeField] protected TurretAnimationModule _animationModule;
    [SerializeField] protected EntityAudioModule _audioModule;

    public Transform Transform => transform;
    public IHealthModule HealthModule => _healthModule;
    public IWeaponsModule WeaponsModule => _weaponsModule;
    public TurretRotationModule RotationModule => _rotationModule;
    public TurretAnimationModule AnimationModule => _animationModule;

    private void Awake()
    {
        _healthModule.Initialize();
        _audioModule.Initialize(HealthModule);
        _weaponsModule.Initialize();
        _rotationModule.Initialize(WeaponsModule);
        _animationModule.Initialize(HealthModule, RotationModule);

        _healthModule.IsOver += OnHealhIsOver;
    }
    private void OnHealhIsOver()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        _rotationModule.Rotate();
        _animationModule.UpdateState();
    }

    private void OnDestroy()
    {
        _healthModule.IsOver -= OnHealhIsOver;
        _rotationModule.OnDestroy();
        _animationModule.OnDestroy();
    }
}
