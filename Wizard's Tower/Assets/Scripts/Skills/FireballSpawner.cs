using Factory;
using Pool;
using UnityEngine;
using VFX;
using Player;

namespace Skills
{
    public class FireballSpawner
    {
        private ObjectPool<Fireball> _fireballPool;
        private DungeonFactory _dungeonFactory;
        private VFXService _vfxService;
       
        public FireballSpawner(PoolsConfig poolsConfig, DungeonFactory dungeonFactory, VFXService vfxService)
        {
            _vfxService = vfxService;
            _dungeonFactory = dungeonFactory;
            _fireballPool = poolsConfig.GetFireballPool();
        }

        public void Shoot()
        {
            _dungeonFactory.Player.CastSpell(_dungeonFactory.Player.PlayerConfig.CastAnimationDuration);

            _dungeonFactory.Player.StartCoroutine(CastCoroutine(_dungeonFactory.Player.PlayerConfig.CastAnimationDuration));
        }

        private System.Collections.IEnumerator CastCoroutine(float castDuration)
        {
            yield return new WaitForSeconds(castDuration);

            var fireball = _fireballPool.Get();
            fireball.OnHit += HandleFireballHit;
            var forward = _dungeonFactory.Player.transform.forward;
            var position = _dungeonFactory.Player.SkillPosition.position;

            _vfxService.PlayVFX(VFXType.Muzzle, position, Quaternion.LookRotation(forward));
            fireball.Activate(position, forward);
        }

        private void HandleFireballHit(Vector3 hitPosition) => 
            _vfxService.PlayVFX(VFXType.Hit, hitPosition, Quaternion.identity);
    }
}