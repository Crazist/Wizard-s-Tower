using DungenGenerator;
using Factory;
using Skills;
using UnityEngine;
using VFX;

namespace Zenject
{
    [CreateAssetMenu(fileName = "GameInstaller", menuName = "Installers/GameInstaller")]
    public class GameInstaller : ScriptableObjectInstaller<GameInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<BspService>().AsSingle();
            Container.Bind<RandomWalkService>().AsSingle();
            Container.Bind<DungeonFactory>().AsSingle();
            Container.Bind<UIFactory>().AsSingle();
            Container.Bind<RoomService>().AsSingle();
            Container.Bind<WallService>().AsSingle();
            Container.Bind<CorridorService>().AsSingle();
            Container.Bind<ItemSpawner>().AsSingle();
            Container.Bind<AssetProvider>().AsSingle();
            Container.Bind<VFXService>().AsSingle();
           
            Container.BindInterfacesAndSelfTo<SkillService>().AsSingle();
        }
    }
}