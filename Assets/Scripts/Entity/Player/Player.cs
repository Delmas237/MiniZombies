using UnityEngine;

namespace PlayerLib
{
    public class Player : MonoBehaviour
    {
        [field: Header("Controllers")]
        [field: SerializeField] public PlayerCurrencyController CurrencyController { get; set; }
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
            if (Time.timeScale > 0)
            {
                MoveController.Move();
                AnimationController.MoveAnim();

                MoveController.Rotation();
            }
        }
    }
}
