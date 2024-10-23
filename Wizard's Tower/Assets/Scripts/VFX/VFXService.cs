using System.Collections.Generic;
using Factory;
using Player;
using Pool;
using UnityEngine;
using Zenject;

namespace VFX
{
    public class VFXService
    {
        private Dictionary<VFXType, ObjectPool<VFXObject>> _vfxPools = new Dictionary<VFXType, ObjectPool<VFXObject>>();
        private PlayerController _playerController;

        [Inject]
        public void Construct(DungeonFactory dungeonFactory, AssetProvider assetProvider)
        {
            var vfxPoolsConfig = assetProvider.VFXPoolsConfig;
            _playerController = dungeonFactory.Player;

            _vfxPools[VFXType.Muzzle] = vfxPoolsConfig.GetFireballPool();
            _vfxPools[VFXType.IceSpikes] = vfxPoolsConfig.GetIceSpikesPool();
            _vfxPools[VFXType.Shield] = vfxPoolsConfig.GetShieldPool();
            _vfxPools[VFXType.Hit] = vfxPoolsConfig.GetHitPool();
        }

        public void PlayVFX(VFXType type, Vector3 position, Quaternion rotation)
        {
            if (_vfxPools.ContainsKey(type))
            {
                var vfxObject = _vfxPools[type].Get();
                vfxObject.Play(position, rotation);
            }
        }

        public void PlayVFXAtPlayerPosition(VFXType type)
        {
            if (_vfxPools.ContainsKey(type))
            {
                var vfxObject = _vfxPools[type].Get();
                vfxObject.Play(_playerController.SkillPosition.position, Quaternion.identity);
            }
        }
    }
}