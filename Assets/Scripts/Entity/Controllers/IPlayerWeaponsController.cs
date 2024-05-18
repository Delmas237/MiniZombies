using JoystickLib;
using System;
using System.Collections.Generic;
using Weapons;

namespace PlayerLib
{
    public interface IPlayerWeaponsController
    {
        public int Bullets { get; }
        public IReadOnlyList<Gun> Guns { get; }
        public Gun CurrentGun { get; }
        public event Action<Gun> GunChanged;

        public Joystick AttackJoystick { get; }
        
        public void ChangeGun(GunType gunType);

        public void AddBullets(int amount);
        public void SpendBullets(int amount);
    }
}
