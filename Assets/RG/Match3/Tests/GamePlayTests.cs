using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Match3.Helpers;
using Match3.DataTypes;
using Match3.MonoBehaviors;

namespace Match3.Tests {
    public class GamePlayTests {

        [UnityTest]
        public IEnumerator CanGetAllMatchesInTileArray() {

            yield return new WaitUntil(() => TilePrefabs.Instance != null);

            var tileArray = new TileInfo[,] {
            { new TileInfo(TileShapeType.Tile0, new TileCoordsInArray(0, 0)), new TileInfo(TileShapeType.Tile1, new TileCoordsInArray(0, 1)), new TileInfo(TileShapeType.Tile2, new TileCoordsInArray(0, 2)), new TileInfo(TileShapeType.Tile0, new TileCoordsInArray(0, 3)) },
            { new TileInfo(TileShapeType.Tile3, new TileCoordsInArray(1, 0)), new TileInfo(TileShapeType.Tile2, new TileCoordsInArray(1, 1)), new TileInfo(TileShapeType.Tile2, new TileCoordsInArray(1, 2)), new TileInfo(TileShapeType.Tile2, new TileCoordsInArray(1, 3)) },
            { new TileInfo(TileShapeType.Tile1, new TileCoordsInArray(2, 0)), new TileInfo(TileShapeType.Tile0, new TileCoordsInArray(2, 1)), new TileInfo(TileShapeType.Tile0, new TileCoordsInArray(2, 2)), new TileInfo(TileShapeType.Tile1, new TileCoordsInArray(2, 3)) },
            { new TileInfo(TileShapeType.Tile0, new TileCoordsInArray(3, 0)), new TileInfo(TileShapeType.Tile0, new TileCoordsInArray(3, 1)), new TileInfo(TileShapeType.Tile0, new TileCoordsInArray(3, 2)), new TileInfo(TileShapeType.Tile0, new TileCoordsInArray(3, 3)) }
            };

            var matches = TileArrayHelper.GetAllMathesInTileArray(tileArray);

            Assert.AreEqual(matches.Count, 2);
            Assert.AreEqual(matches[0].Count, 3);
            Assert.AreEqual(matches[1].Count, 4);

            Assert.AreEqual(matches[0][0], tileArray[1, 1]);
            Assert.AreEqual(matches[0][1], tileArray[1, 2]);
            Assert.AreEqual(matches[0][2], tileArray[1, 3]);

            Assert.AreEqual(matches[1][0], tileArray[3, 0]);
            Assert.AreEqual(matches[1][1], tileArray[3, 1]);
            Assert.AreEqual(matches[1][2], tileArray[3, 2]);
            Assert.AreEqual(matches[1][3], tileArray[3, 3]);
        }

        [UnityTest]
        public IEnumerator CanCreateRandom4x4TileArrayWithZeroMatches() {

            yield return new WaitUntil(() => TilePrefabs.Instance != null);

            var tileArray = TileArrayHelper.CreateRandomTileArray(4, 4);

            var matches = TileArrayHelper.GetAllMathesInTileArray(tileArray);

            Assert.AreEqual(0, matches.Count);
        }

        [UnityTest]
        public IEnumerator CanCreateRandom20x20TileArrayWithZeroMatches() {

            yield return new WaitUntil(() => TilePrefabs.Instance != null);

            var tileArray = TileArrayHelper.CreateRandomTileArray(20, 20);

            var matches = TileArrayHelper.GetAllMathesInTileArray(tileArray);

            Assert.AreEqual(0, matches.Count);
        }

        [UnityTest]
        public IEnumerator AreTileProperlyMovedDownAfterDeletion() {

            yield return new WaitUntil(() => TilePrefabs.Instance != null);

            var tileArray = new TileInfo[,] {
            { new TileInfo(TileShapeType.Tile0, new TileCoordsInArray(0, 0)), new TileInfo(TileShapeType.Tile1, new TileCoordsInArray(0, 1)), new TileInfo(TileShapeType.Tile2, new TileCoordsInArray(0, 2)), new TileInfo(TileShapeType.Tile3, new TileCoordsInArray(0, 3)) },
            { new TileInfo(TileShapeType.Tile3, new TileCoordsInArray(1, 0)), new TileInfo(TileShapeType.Tile2, new TileCoordsInArray(1, 1)), new TileInfo(TileShapeType.Tile2, new TileCoordsInArray(1, 2)), new TileInfo(TileShapeType.Tile1, new TileCoordsInArray(1, 3)) },
            { new TileInfo(TileShapeType.Tile1, new TileCoordsInArray(2, 0)), new TileInfo(TileShapeType.Tile0, new TileCoordsInArray(2, 1)), new TileInfo(TileShapeType.Tile0, new TileCoordsInArray(2, 2)), new TileInfo(TileShapeType.Tile1, new TileCoordsInArray(2, 3)) },
            { new TileInfo(TileShapeType.Tile2, new TileCoordsInArray(3, 0)), new TileInfo(TileShapeType.Tile3, new TileCoordsInArray(3, 1)), new TileInfo(TileShapeType.Tile2, new TileCoordsInArray(3, 2)), new TileInfo(TileShapeType.Tile3, new TileCoordsInArray(3, 3)) }
            };

            TileArrayHelper.DeleteTile(ref tileArray, new TileCoordsInArray(3, 1));

            Assert.AreEqual(TileShapeType.None, tileArray[0, 1].TileShapeType);
            Assert.AreEqual(TileShapeType.Tile1, tileArray[1, 1].TileShapeType);
            Assert.AreEqual(TileShapeType.Tile2, tileArray[2, 1].TileShapeType);
            Assert.AreEqual(TileShapeType.Tile0, tileArray[3, 1].TileShapeType);

        }

        [UnityTest]
        public IEnumerator CanDeleteMatchesInTileArray() {

            yield return new WaitUntil(() => TilePrefabs.Instance != null);


            var tileArray = new TileInfo[,] {
            { new TileInfo(TileShapeType.Tile0, new TileCoordsInArray(0, 0)), new TileInfo(TileShapeType.Tile1, new TileCoordsInArray(0, 1)), new TileInfo(TileShapeType.Tile2, new TileCoordsInArray(0, 2)), new TileInfo(TileShapeType.Tile0, new TileCoordsInArray(0, 3)) },
            { new TileInfo(TileShapeType.Tile3, new TileCoordsInArray(1, 0)), new TileInfo(TileShapeType.Tile2, new TileCoordsInArray(1, 1)), new TileInfo(TileShapeType.Tile2, new TileCoordsInArray(1, 2)), new TileInfo(TileShapeType.Tile2, new TileCoordsInArray(1, 3)) },
            { new TileInfo(TileShapeType.Tile1, new TileCoordsInArray(2, 0)), new TileInfo(TileShapeType.Tile0, new TileCoordsInArray(2, 1)), new TileInfo(TileShapeType.Tile0, new TileCoordsInArray(2, 2)), new TileInfo(TileShapeType.Tile1, new TileCoordsInArray(2, 3)) },
            { new TileInfo(TileShapeType.Tile0, new TileCoordsInArray(3, 0)), new TileInfo(TileShapeType.Tile0, new TileCoordsInArray(3, 1)), new TileInfo(TileShapeType.Tile0, new TileCoordsInArray(3, 2)), new TileInfo(TileShapeType.Tile0, new TileCoordsInArray(3, 3)) }
            };

            TileArrayHelper.DeleteAllMatches(ref tileArray);

            // 1st row
            Assert.AreEqual(TileShapeType.None, tileArray[0, 0].TileShapeType);
            Assert.AreEqual(TileShapeType.None, tileArray[0, 1].TileShapeType);
            Assert.AreEqual(TileShapeType.None, tileArray[0, 2].TileShapeType);
            Assert.AreEqual(TileShapeType.None, tileArray[0, 3].TileShapeType);

            // 2nd row
            Assert.AreEqual(TileShapeType.Tile0, tileArray[1, 0].TileShapeType);
            Assert.AreEqual(TileShapeType.None, tileArray[1, 1].TileShapeType);
            Assert.AreEqual(TileShapeType.None, tileArray[1, 2].TileShapeType);
            Assert.AreEqual(TileShapeType.None, tileArray[1, 3].TileShapeType);

            // 3rd row
            Assert.AreEqual(TileShapeType.Tile3, tileArray[2, 0].TileShapeType);
            Assert.AreEqual(TileShapeType.Tile1, tileArray[2, 1].TileShapeType);
            Assert.AreEqual(TileShapeType.Tile2, tileArray[2, 2].TileShapeType);
            Assert.AreEqual(TileShapeType.Tile0, tileArray[2, 3].TileShapeType);

            // 4th row
            Assert.AreEqual(TileShapeType.Tile1, tileArray[3, 0].TileShapeType);
            Assert.AreEqual(TileShapeType.Tile0, tileArray[3, 1].TileShapeType);
            Assert.AreEqual(TileShapeType.Tile0, tileArray[3, 2].TileShapeType);
            Assert.AreEqual(TileShapeType.Tile1, tileArray[3, 3].TileShapeType);


        }
    }
}