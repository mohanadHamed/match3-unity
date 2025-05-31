using Match3.InputHandlers;
using UnityEngine;
using Match3.Helpers;
using System.Collections;
using System.Collections.Generic;
using Match3.DataTypes;

namespace Match3.MonoBehaviors {
    public class GameManager : MonoBehaviour {
        public const int MinimumNumberOfTilesToMatch = 3;

        public const float CameraZPositionFactor = 1.8f;

        public const float CameraZPositionShift = 2f;

        public static GameManager Instance;

        [SerializeField]
        [Range(4, 20)]
        private int numberOfRows = 10;

        [SerializeField]
        [Range(4, 20)]
        private int numberOfColumns = 10;

        [SerializeField]
        private Camera mainCamera;

        [SerializeField]
        private GameObject background;

        [SerializeField]
        private Tiles3dPositionsComponent tilesComponent;

        private const float TimeDelayBetweenDeletingTilesInSeconds = 0.2f;

        private TileInfo[,] tileArray;

        private InputHandler inputHandler;

        private bool inputAllowed = true;

        void Awake() {
            if (Instance == null) {
                Instance = this;
            }
            else {
                Destroy(gameObject);
            }
        }

        void Start() {
            tileArray = tilesComponent.CreateAllTiles(numberOfRows, numberOfColumns, mainCamera, background);

            StartCoroutine(tilesComponent.SetTile3dPositions(tileArray, TileArrayHelper.GetAllColumns(tileArray)));

            inputHandler = InputHandler.CreateInstance();
        }

        void Update() {
            Vector3 clickPos;

            if (inputAllowed && inputHandler.GetUserClickPosiiton(out clickPos)) {
                inputAllowed = false;
                StartCoroutine(HandleClickRaycast(clickPos));
            }
        }

        private IEnumerator HandleClickRaycast(Vector3 clickPos) {

            TileInfo clickedTile;

            if (TileInputHelper.GetClickedTile(tileArray, clickPos, out clickedTile)) {
                TileArrayHelper.DeleteTile(ref tileArray, clickedTile.TileCoords);
                yield return new WaitForSeconds(TimeDelayBetweenDeletingTilesInSeconds);

                var columns = new List<int> { clickedTile.TileCoords.col };
                yield return tilesComponent.SetTile3dPositions(tileArray, columns, true);

                yield return DeleteNewMatches();
            }

            inputAllowed = true;
        }

        private IEnumerator DeleteNewMatches() {
            List<List<TileInfo>> matches;
            while ((matches = TileArrayHelper.GetAllMathesInTileArray(tileArray)).Count > 0) {
                yield return new WaitForSeconds(TimeDelayBetweenDeletingTilesInSeconds);
                int i, j;
                int numMatches, numTilesInMatch;
                var affectedColumns = new List<int> ();

                for (i = 0, numMatches = matches.Count; i < numMatches; i++) {
                    var match = matches[i];
                    for (j = 0, numTilesInMatch = match.Count; j < numTilesInMatch; j++) {
                        var tile = match[j];
                        var coords = tile.TileCoords;
                        var col = coords.col;

                        TileArrayHelper.DeleteTile(ref tileArray, coords);

                        if (affectedColumns.Contains(col) == false) {
                            affectedColumns.Add(col);
                        }
                    }
                }

                yield return tilesComponent.SetTile3dPositions(tileArray, affectedColumns, true);
                yield return new WaitForSeconds(TimeDelayBetweenDeletingTilesInSeconds);
            }
        }
    }
}