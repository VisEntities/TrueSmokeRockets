using UnityEngine;

namespace Oxide.Plugins
{
    [Info("True Smoke Rockets", "Dana", "1.0.0")]
    [Description("Making smoke rockets functional, just as they should be.")]
    public class TrueSmokeRockets : RustPlugin
    {
        #region Fields

        private const string SMOKE_ROCKET_PREFAB = "assets/prefabs/ammo/rocket/rocket_smoke.prefab";
        private const string SMOKE_PREFAB = "assets/prefabs/tools/smoke grenade/grenade.smoke.deployed.prefab";

        #endregion Fields

        #region Oxide Hooks

        private void OnEntityKill(BaseEntity entity)
        {
            if (!entity.IsValid())
                return;

            if (entity.PrefabName != SMOKE_ROCKET_PREFAB)
                return;

            BaseEntity smoke = GameManager.server.CreateEntity(SMOKE_PREFAB, entity.transform.position, Quaternion.identity);
            if (smoke)
                smoke.Spawn();
        }

        #endregion Oxide Hooks
    }
}