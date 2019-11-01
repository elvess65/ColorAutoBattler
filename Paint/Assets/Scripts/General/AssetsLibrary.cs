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
            public Projectiles.Projectile ProjectilePrefab;

            public Character.Health.UI.UIHealthBarController UIHealthBarPrefab;
            public Character.Health.UI.UIHealthBarSegment UIHealthBarSegmentPrefab;
        }
    }
}
