using UnityEngine;

namespace Weapons
{
    public class Shotgun : Gun
    {
        public override bool ShootRequest()
        {
            if (_canShoot)
            {
                float pitchRange = 0.02f;
                
                BulletTrail bullet = Shoot(_shootDir.position + Vector3.left * 0.15f);
                bullet.AudioSource.pitch += Random.Range(-pitchRange, pitchRange);

                BulletTrail bullet1 = Shoot(_shootDir.position + Vector3.right * 0.15f);
                bullet1.AudioSource.volume = 0;

                _canShoot = false;
                _currentCooldown = Cooldown;
                return true;
            }

            return false;
        }
    }
}
