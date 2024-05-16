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
            AnimationController.Initialize(HealthController, WeaponsController, MoveController, GetComponent<Animator>());
            WeaponsController.Initialize(HealthController, MoveController);
            MoveController.Initialize(HealthController, WeaponsController, transform, GetComponent<Rigidbody>());
        }

        private void Update()
        {
            MoveController.Move();
            AnimationController.MoveAnim();

            MoveController.Rotation();
            WeaponsController.UpdateShootLine();
        }

        private void OnDestroy()
        {
            MoveController.OnDestroy();
        }
    }
}
