using UnityEngine;

namespace Paint.General
{
    public class AssetsLibrary : MonoBehaviour
    {
        public PrefabsLibrary Library_Prefabs;

        [System.Serializable]
        public class PrefabsLibrary
        {
            public Characters.Character PlayerCharacterPrefab;
            public Characters.Character ManekenCharacterPrefab;
            public Characters.Character TurrentCharacterPrefab;
            public Characters.Character UnitCharacterPrefab;

            public Projectiles.Projectile ProjectilePrefab;
            public Shields.Shield ShieldPrefab;

            public Character.Health.UI.UIHealthBarController UIHealthBarPrefab;
            public Character.Health.UI.UIHealthBarController_Unit UIHealthBarUnitPrefab;
            public Character.Health.UI.UIHealthBarSegment UIHealthBarSegmentPrefab;
            public Character.Health.UI.UIAttackDefenceBar UIAttackDefenceBarPrefab;
        }
    }
}
