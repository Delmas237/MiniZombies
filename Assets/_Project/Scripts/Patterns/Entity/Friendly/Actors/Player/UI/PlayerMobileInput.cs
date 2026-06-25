using JoystickLib;
using UnityEngine;

namespace Entity.Friendly.Player
{
    public class PlayerMobileInput : MonoBehaviour
    {
        [SerializeField] private Joystick _moveJoystick;
        [SerializeField] private Joystick _attackJoystick;

        public Joystick MoveJoystick => _moveJoystick;
        public Joystick AttackJoystick => _attackJoystick;
    }
}
