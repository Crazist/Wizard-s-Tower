namespace Characters
{
    public class StatsService
    {
        public CharacterStats PlayerStats => _playerStats;

        private readonly CharacterStats _playerStats;
        private readonly AssetProvider _assetProvider;

        public StatsService(AssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
            _playerStats = new CharacterStats(_assetProvider.CharacterConfigs.GetCharacterStats(CharacterType.Player));
        }

        public CharacterStats CreateCharacterStats(CharacterType type)
        {
            CharacterBaseStatsConfig baseStats = _assetProvider.CharacterConfigs.GetCharacterStats(type);
            return new CharacterStats(baseStats);
        }
    }
}