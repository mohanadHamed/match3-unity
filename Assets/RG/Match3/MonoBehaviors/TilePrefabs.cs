using UnityEngine;

namespace Match3.MonoBehaviors {

    [ExecuteInEditMode]
    public class TilePrefabs : MonoBehaviour {
        public static TilePrefabs Instance;

        public GameObject[] allTilePrefabs;

        void Awake() {
            if (Instance == null) {
                Instance = this;
            }
            else {
                Destroy(gameObject);
            }
        }
    }
}
