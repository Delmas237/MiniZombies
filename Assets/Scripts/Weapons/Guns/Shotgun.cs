using UnityEngine;

namespace Weapons
{
    public class Shotgun : Gun
    {
        public override bool ShootRequest()
        {
            if (_canShoot)
            {
                float pitch = Random.Range(0.98f, 1.02f);
                
                Shoot(_shootDir.position + Vector3.left * 0.15f);
                SoundPitch(pitch);
                Shoot(_shootDir.position + Vector3.right * 0.15f);
                SoundPitch(pitch);

                _canShoot = false;
                _currentCooldown = Cooldown;
                return true;
            }

            return false;
        }
    }
}
