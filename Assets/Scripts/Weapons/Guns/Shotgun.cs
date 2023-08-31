using UnityEngine;

namespace Weapons
{
    public class Shotgun : Gun
    {
        public override bool ShootRequest()
        {
            if (canShoot)
            {
                float pitch = Random.Range(0.98f, 1.02f);
                
                Shoot(shootDir.position + Vector3.left * 0.15f);
                SoundPitch(pitch);
                Shoot(shootDir.position + Vector3.right * 0.15f);
                SoundPitch(pitch);

                canShoot = false;
                currentCooldown = Cooldown;
                return true;
            }

            return false;
        }
    }
}
