using Newtonsoft.Json;
using UnityEngine;

namespace Oxide.Plugins
{
    [Info("True Smoke Rockets", "VisEntities", "1.1.0")]
    [Description("Functional smoke rockets that produce a smoke cloud upon explosion.")]
    public class TrueSmokeRockets : RustPlugin
    {
        #region Fields

        private static Configuration _config;

        private const string SMOKE_ROCKET_PREFAB = "assets/prefabs/ammo/rocket/rocket_smoke.prefab";
        private const string SMOKE_GRENADE_PREFAB = "assets/prefabs/tools/smoke grenade/grenade.smoke.deployed.prefab";

        #endregion Fields

        #region Configuration

        private class Configuration
        {
            [JsonProperty("Version")]
            public string Version { get; set; }

            [JsonProperty("Smoke Duration Seconds")]
            public float SmokeDurationSeconds { get; set; }
        }

        protected override void LoadConfig()
        {
            base.LoadConfig();
            _config = Config.ReadObject<Configuration>();

            if (string.Compare(_config.Version, Version.ToString()) < 0)
                UpdateConfig();

            SaveConfig();
        }

        protected override void LoadDefaultConfig()
        {
            _config = GetDefaultConfig();
        }

        protected override void SaveConfig()
        {
            Config.WriteObject(_config, true);
        }

        private void UpdateConfig()
        {
            PrintWarning("Config changes detected! Updating...");

            Configuration defaultConfig = GetDefaultConfig();

            if (string.Compare(_config.Version, "1.0.0") < 0)
                _config = defaultConfig;

            PrintWarning("Config update complete! Updated from version " + _config.Version + " to " + Version.ToString());
            _config.Version = Version.ToString();
        }

        private Configuration GetDefaultConfig()
        {
            return new Configuration
            {
                Version = Version.ToString(),
                SmokeDurationSeconds = 30f,
            };
        }

        #endregion Configuration

        #region Oxide Hooks

        private void Unload()
        {
            _config = null;
        }

        private void OnEntityKill(TimedExplosive explosive)
        {
            if (explosive == null)
                return;
            
            if (explosive.PrefabName != SMOKE_ROCKET_PREFAB)
                return;

            SmokeGrenade smokeGrenade = GameManager.server.CreateEntity(SMOKE_GRENADE_PREFAB, explosive.transform.position, Quaternion.identity) as SmokeGrenade;
            if (smokeGrenade != null)
            {
                smokeGrenade.smokeDuration = _config.SmokeDurationSeconds;
                smokeGrenade.Spawn();
            }
        }

        #endregion Oxide Hooks
    }
}
