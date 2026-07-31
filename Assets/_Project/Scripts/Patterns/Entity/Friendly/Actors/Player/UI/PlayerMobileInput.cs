using JoystickLib;
using UnityEngine;

namespace EntityLib.Friendly.Player
{
    public class PlayerMobileInput : MonoBehaviour
    {
        [SerializeField] private Joystick _moveJoystick;
        [SerializeField] private Joystick _attackJoystick;

        public Joystick MoveJoystick => _moveJoystick;
        public Joystick AttackJoystick => _attackJoystick;
    }
}
