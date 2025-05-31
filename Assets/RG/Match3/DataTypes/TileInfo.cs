using UnityEngine;
using UnityEngine.Assertions;
using Match3.MonoBehaviors;

namespace Match3.DataTypes {
    public class TileInfo {
        public TileShapeType TileShapeType { get; private set; }

        public TileCoordsInArray TileCoords { get; set; }

        public GameObject TileInstanceObject { get; }

        public TileInfo(TileShapeType shapeType, TileCoordsInArray tileCoords, Transform parent = null) {

            TileShapeType = shapeType;
            TileCoords = tileCoords;

            var allPrefabs = TilePrefabs.Instance.allTilePrefabs;
            GameObject selectedPrefab = null;

            switch (shapeType) {
                case TileShapeType.Tile0:
                    Assert.IsTrue(allPrefabs != null && allPrefabs.Length > 0);
                    selectedPrefab = allPrefabs[0];
                    break;

                case TileShapeType.Tile1:
                    Assert.IsTrue(allPrefabs != null && allPrefabs.Length > 1);
                    selectedPrefab = allPrefabs[1];
                    break;

                case TileShapeType.Tile2:
                    Assert.IsTrue(allPrefabs != null && allPrefabs.Length > 2);
                    selectedPrefab = allPrefabs[2];
                    break;

                case TileShapeType.Tile3:
                    Assert.IsTrue(allPrefabs != null && allPrefabs.Length > 3);
                    selectedPrefab = allPrefabs[3];
                    break;
            }

            if (selectedPrefab != null) {
                TileInstanceObject = Object.Instantiate(selectedPrefab, parent);
            }
        }


        public void Delete() {

            TileShapeType = TileShapeType.None;
            TileInstanceObject.SetActive(false);
        }
    }
}
