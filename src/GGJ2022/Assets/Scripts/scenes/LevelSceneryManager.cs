using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace scenes
{
    // Use this to scroll the world.
    public class LevelSceneryManager : MonoBehaviour
    {
        public GameStateHandler gameState;

        [SerializeField] private BoxCollider2D worldBounds;

        [SerializeField] private LevelSceneryDB db;

        [SerializeField] // How fast things will move.
        private float movementSpeed = 1;

        private IEnumerator<SceneryObject> _sceneryIterator;

        // How much height do we have left, until we spawn the next scenery element.
        private float _currentHeight;

        [SerializeField] private float minSpawnDelay = 1, maxSpawnDelay = 5;
        
        private float _nextMonsterSpawnAt;

        private bool _spawnEnemies = true;
        
        public void EnableEnemySpawning(bool enable)
        {
            if (enable)
                _nextMonsterSpawnAt = Time.time + 5;

            _spawnEnemies = enable;
        }

        private void Awake()
        {
            _sceneryIterator = db.GetEnumerator();
            EnableEnemySpawning(true);
        }

        private void Update()
        {
            var move = -movementSpeed * Time.deltaTime;

            foreach (Transform child in transform)
            {
                child.position += new Vector3(0, move, 0);
                if (child.position.y <= -worldBounds.size.y * 4.1)
                    Destroy(child.gameObject);
            }

            _currentHeight += move;

            while (_currentHeight <= worldBounds.size.y * 2) // Spawn the next background the tiniest bit early
                SpawnNextScenery();

            if (_nextMonsterSpawnAt <= Time.time && _spawnEnemies)
            {
                _nextMonsterSpawnAt = Time.time + Random.Range(minSpawnDelay, maxSpawnDelay);

                if (null != _sceneryIterator.Current)
                    SpawnEnemies(_sceneryIterator.Current.enemySpawnPotential);
            }
        }

        private void SpawnNextScenery()
        {
            var hadOne = _sceneryIterator.MoveNext();
            Debug.Assert(hadOne);
            var so = _sceneryIterator.Current;
            Debug.Assert(null != so);

            var parent = new GameObject("Scrolling Background: " + so.name);
            parent.transform.parent = transform;

            var bl = Instantiate(so.leftBorder, parent.transform);
            bl.transform.position = new Vector3(-worldBounds.size.x / 2, 0, 0);
            var br = Instantiate(so.rightBorder, parent.transform);
            br.transform.position = new Vector3(worldBounds.size.x / 2, 0, 0);
            Instantiate(so.background, parent.transform);

            var size = worldBounds.size;
            parent.transform.position = new Vector3(0, -size.y / 2 + _currentHeight, 0);

            _currentHeight += size.y;
        }

        private void SpawnEnemies(List<EnemyPotential> enemies)
        {
            if (enemies.Count == 0)
                return;

            float selection = Random.Range(0, enemies.Select(rp => rp.spawnChance).Sum());
            float sum = 0;
            foreach (var sc in enemies)
            {
                sum += sc.spawnChance;

                if (sum >= selection)
                {
                    if (null != sc.spawner)
                    {
                        var spawner = Instantiate(sc.spawner);
                        SearchForEnemyScripts(spawner);

                        spawner.transform.position = new Vector3(0, -worldBounds.size.y / 2 + _currentHeight, 0);
                        while (spawner.transform.childCount != 0) // Can not use for each when modefing
                        {
                            spawner.transform.GetChild(0).parent = transform;
                        }

                        Destroy(spawner);
                    }

                    return;
                }
            }
        }

        private void SearchForEnemyScripts(GameObject go)
        {
            foreach (Transform child in go.transform)
            {
                SearchForEnemyScripts(child.gameObject);
            }

            var enemy = go.GetComponent<Enemy>();
            if (null != enemy)
            {
                enemy.onKill.AddListener(gameState.IncrementScore);
                enemy.onCrash.AddListener(gameState.DamagePlayer);
            }
        }
    }
}