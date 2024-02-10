using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace scenes
{
    /// <summary>
    /// Database of scenery objects.
    /// See also LevelSceneryManager which is the MonoBehaviour to use.
    /// </summary>
    [CreateAssetMenu(fileName = nameof(LevelSceneryDB), menuName = "scenery/" + nameof(LevelSceneryDB), order = 0)]
    public class LevelSceneryDB : ScriptableObject, IEnumerable<SceneryObject>
    {
        private class LevelSceneryDBIterator : IEnumerator<SceneryObject>
        {
            private readonly LevelSceneryDB _db;

            public LevelSceneryDBIterator(LevelSceneryDB db) =>
                _db = db;

            public bool MoveNext()
            {
                if (_db.possibleAssets.Count == 0)
                    return false;

                if (null != Current && Current.requiredFollowUp.Count > 0)
                    Current = Current.requiredFollowUp[Random.Range(0, Current.requiredFollowUp.Count)];
                else
                    Current = _db.possibleAssets[Random.Range(0, _db.possibleAssets.Count)];

                return true;
            }

            public void Reset()
            {
                throw new System.NotImplementedException();
            }

            object IEnumerator.Current => Current;

            public SceneryObject Current { get; private set; }

            public void Dispose()
            {
            }
        }

        [SerializeField] private List<SceneryObject> possibleAssets;

        public IEnumerator<SceneryObject> GetEnumerator() =>
            new LevelSceneryDBIterator(this);

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();
    }
}