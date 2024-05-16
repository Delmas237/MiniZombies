using UnityEngine;

namespace PlayerLib
{
    public class PlayerContainer : MonoBehaviour, IPlayer
    {
        [field: Header("Controllers")]
        [field: SerializeField] public CurrencyController CurrencyController { get; set; }
        [field: SerializeField] public HealthController HealthController { get; set; }
        [field: SerializeField] public PlayerWeaponsController WeaponsController { get; set; }
        [field: SerializeField] public PlayerMoveController MoveController { get; set; }
        [field: SerializeField] public PlayerAnimationController AnimationController { get; set; }

        private void Start()
        {
            HealthController.Initialize();
            AnimationController.Initialize(this, GetComponent<Animator>());
            WeaponsController.Initialize(this);
            MoveController.Initialize(this, transform);
        }

        private void Update()
        {
            MoveController.Move();
            AnimationController.MoveAnim();

            MoveController.Rotation();
            WeaponsController.UpdateShootLine();
        }
    }
}
