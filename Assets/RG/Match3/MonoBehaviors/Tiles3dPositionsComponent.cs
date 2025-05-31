using UnityEngine;
using Match3.Helpers;
using System.Collections;
using Match3.DataTypes;
using System.Collections.Generic;

namespace Match3.MonoBehaviors {
    public class Tiles3dPositionsComponent : MonoBehaviour {
        [SerializeField]
        private uint tileSpacingX = 1;

        [SerializeField]
        private uint tileSpacingY = 1;

        [SerializeField]
        private Vector3 gridShift = new Vector3(0.5f, 0.5f, 0);

        [SerializeField]
        [Range(1, 50)]
        private int posiitonAnimateSpeed = 25;

        private Vector3 gridTopLeft;

        public TileInfo[,] CreateAllTiles(int numberOfRows, int numberOfColumns, Camera mainCamera, GameObject background) {
            var tileArray = TileArrayHelper.CreateRandomTileArray(numberOfRows, numberOfColumns, transform);

            var bgTransform = background.transform;
            var cameraTransform = mainCamera.transform;

            var bgScale = bgTransform.localScale;
            var cameraPosition = cameraTransform.position;

            cameraPosition.z = -1 * (GameManager.CameraZPositionShift + GameManager.CameraZPositionFactor * Mathf.Max(numberOfColumns, numberOfRows));

            bgTransform.localScale = new Vector3(numberOfColumns, numberOfRows, bgScale.z);
            cameraTransform.position = cameraPosition;

            gridTopLeft = gridShift + new Vector3(-numberOfColumns * 0.5f * tileSpacingX, numberOfRows * 0.5f * tileSpacingY, 0);

            return tileArray;
        }

        public IEnumerator SetTile3dPositions(TileInfo[,] tileArray, List<int> affectedColumns, bool animate = false) {
            const float extraAnimationTime = 0.5f;
            var timeout = extraAnimationTime + 1f / posiitonAnimateSpeed;
            var startTime = Time.time;

            if (animate) {
                while ((Time.time - startTime) < timeout) {
                    yield return null;
                    SetOrLerpTile3dPositions(tileArray, affectedColumns, true);
                }
            }

            SetOrLerpTile3dPositions(tileArray, affectedColumns, false);
        }

        private void SetOrLerpTile3dPositions(TileInfo[,] tileArray, List<int> affectedColumns, bool lerp) {
            var numRows = tileArray.GetLength(0);
            var numAffectedColumns = affectedColumns.Count;

            for (int affectedColIndex = 0; affectedColIndex < numAffectedColumns; affectedColIndex++) {
                for (int row = 0; row < numRows; row++) {

                    var col = affectedColumns[affectedColIndex];
                    var tile = tileArray[row, col];
                    var finalPosition = GetTileFinalPsition(row, col);
                    if (tile.TileShapeType == TileShapeType.None
                        || Vector3.Distance(tile.TileInstanceObject.transform.position, finalPosition) <= Mathf.Epsilon) {
                        continue;
                    }

                    if (lerp) {
                        var currentPos = tile.TileInstanceObject.transform.position;
                        tile.TileInstanceObject.transform.position = Vector3.Lerp(currentPos, finalPosition, posiitonAnimateSpeed * Time.deltaTime);
                    }
                    else {
                        tile.TileInstanceObject.transform.position = finalPosition;
                    }
                }
            }
        }


        private Vector3 GetTileFinalPsition(int row, int col) {
            return gridTopLeft + new Vector3(col * tileSpacingX, -row * tileSpacingY, 0);
        }
    }
}