using Enemy;
using Skills;
using UnityEngine;
using VFX;

public class AssetProvider
{
    private const string SkillConfigPath = "Configs/SkillConfig";
    private const string PoolsConfigPath = "Configs/PoolsConfig";
    private const string VFXPoolsConfigPath = "Configs/VFXPoolsConfig";
    private const string EnemySpawnConfigPath = "Configs/EnemySpawnConfig";
    public SkillConfig SkillConfig { get; set; } 
    public PoolsConfig PoolsConfig { get; set; }
    public VFXPoolsConfig VFXPoolsConfig { get; set; }
    public EnemySpawnConfig EnemySpawnConfig { get; set; }
    public AssetProvider()
    {
        SkillConfig = Resources.Load<SkillConfig>(SkillConfigPath);
        PoolsConfig = Resources.Load<PoolsConfig>(PoolsConfigPath);
        VFXPoolsConfig = Resources.Load<VFXPoolsConfig>(VFXPoolsConfigPath);
        EnemySpawnConfig = Resources.Load<EnemySpawnConfig>(EnemySpawnConfigPath);
    }
}