using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace scenes
{
    [Serializable]
    public struct EnemyPotential
    {
        public float spawnChance;
        public GameObject spawner;
    }
    
    /**
     * Single segment of scenery.
     * See [LevelSceneryDB].
     */
    
    [CreateAssetMenu(fileName = nameof(SceneryObject), menuName = "scenery/" + nameof(SceneryObject), order = 0)]
    public class SceneryObject : ScriptableObject
    {
        [Range(-1,1)]
        public float affinity = 0;

        public GameObject leftBorder, rightBorder, background;

        public List<EnemyPotential> enemySpawnPotential;

        // You can set assets here, when this screen HAS to be followed by a specific scenery.
        [SerializeField]
        [Description("When this background has specific assets, which are required to follow this one.")]
        public List<SceneryObject> requiredFollowUp;
    }
}