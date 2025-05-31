using System;
using System.Collections.Generic;
using UnityEngine;
using Match3.DataTypes;
using Match3.MonoBehaviors;

namespace Match3.Helpers {

    #region public methods
    public static class TileArrayHelper {

        public static List<List<TileInfo>> GetAllMathesInTileArray(TileInfo[,] tileArray) {
            var result = new List<List<TileInfo>>();

            var numRows = tileArray.GetLength(0);
            var numColumns = tileArray.GetLength(1);

            for (int row = 0; row < numRows; row++) {

                var tempMatch = new List<TileInfo>();

                for (int col = 0; col < numColumns; col++) {

                    var currentTile = tileArray[row, col];

                    if (tempMatch.Count == 0) {
                        AddTileToTempMatch(tempMatch, currentTile);
                        continue;
                    }

                    CheckMatches(result, numColumns, tempMatch, col, currentTile);
                }
            }

            return result;
        }

        public static TileInfo[,] CreateRandomTileArray(int rows, int columns, Transform parent = null) {

            TileInfo[,] tileArray = new TileInfo[rows, columns];

            var allShapeTypesToPick = GetAllShapeTypes(true);

            for (int row = 0; row < rows; row++) {

                var prevMatches = new List<TileInfo>();
                var availableShapesToPick = new List<TileShapeType>(allShapeTypesToPick);

                for (int col = 0; col < columns; col++) {
                    availableShapesToPick = CreateRandomTile(tileArray, allShapeTypesToPick, row, prevMatches, availableShapesToPick, col, parent);
                }
            }

            return tileArray;
        }

        public static void ReArrangeTileArrayAccordingToAssignedCoordinates(ref TileInfo[,] tileArray, int column) {
            var rows = tileArray.GetLength(0);
            var newArray = new TileInfo[rows];
            int row;

            for (row = 0; row < rows; row++) {

                var tileInfo1 = tileArray[row, column];
                var newRow = tileInfo1.TileCoords.row;
                
                newArray[newRow] = tileInfo1;
            }

            for (row = 0; row < rows; row++) {

                tileArray[row, column] = newArray[row];
            }
        }

        public static void DeleteTile(ref TileInfo[,] tileArray, TileCoordsInArray tileCoords) {
            var rows = tileArray.GetLength(0);
            var cols = tileArray.GetLength(1);

            if (tileCoords.row >= rows || tileCoords.row < 0) {
                return;
            }

            if (tileCoords.col >= cols || tileCoords.col < 0) {
                return;
            }

            var deletedTile = tileArray[tileCoords.row, tileCoords.col];
            deletedTile.Delete();

            MoveDownAffectedTiles(ref tileArray, deletedTile);
        }

        public static void DeleteAllMatches(ref TileInfo[,] tileArray) {

            var matches = GetAllMathesInTileArray(tileArray);
            int i, j, numMatches, numTilesInMatch;

            for (i = 0, numMatches = matches.Count; i < numMatches; i++) {
                var match = matches[i];
                for (j = 0, numTilesInMatch = match.Count; j < numTilesInMatch; j++) {
                    DeleteTile(ref tileArray, match[j].TileCoords);
                }
            }
        }

        public static void MoveDownAffectedTiles(ref TileInfo[,] tileArray, TileInfo deletedTile) {
            var deletedTileRow = deletedTile.TileCoords.row;
            var deletedTileCol = deletedTile.TileCoords.col;

            var lastCoords = deletedTile.TileCoords;

            var row = deletedTileRow - 1;

            TileInfo tile;

            while (row >= 0 && (tile = tileArray[row, deletedTileCol]).TileShapeType != TileShapeType.None) {
                var tempCoords = tile.TileCoords;
                tile.TileCoords = lastCoords;
                lastCoords = tempCoords;

                row--;
            }

            deletedTile.TileCoords = lastCoords;

            ReArrangeTileArrayAccordingToAssignedCoordinates(ref tileArray, deletedTileCol);
        }


        public static List<int> GetAllColumns(TileInfo[,] tileArray) {

            var result = new List<int>();
            var numColumns = tileArray.GetLength(1);

            for (int col = 0; col < numColumns; col++) {
                result.Add(col);
            }

            return result;
        }

        #endregion




        #region private methods
        private static void CheckMatches(List<List<TileInfo>> result, int numColumns, List<TileInfo> tempMatch, int col, TileInfo currentTile) {
            var prevTile = tempMatch[tempMatch.Count - 1];

            if (currentTile.TileShapeType != prevTile.TileShapeType) {
                if (tempMatch.Count >= GameManager.MinimumNumberOfTilesToMatch) {
                    result.Add(new List<TileInfo>(tempMatch));
                }
                tempMatch.Clear();
                AddTileToTempMatch(tempMatch, currentTile);
            }
            else {
                AddTileToTempMatch(tempMatch, currentTile);

                if (col == numColumns - 1) {
                    if (tempMatch.Count >= GameManager.MinimumNumberOfTilesToMatch) {
                        result.Add(new List<TileInfo>(tempMatch));
                    }
                }
            }
        }

        private static List<TileShapeType> CreateRandomTile(TileInfo[,] tileArray, List<TileShapeType> allShapeTypesToPick, int row, List<TileInfo> prevMatches, List<TileShapeType> availableShapesToPick, int col, Transform parent) {

            if (prevMatches.Count > 0) {
                var lastShape = prevMatches[prevMatches.Count - 1].TileShapeType;

                if (prevMatches.Count >= (GameManager.MinimumNumberOfTilesToMatch - 1) && availableShapesToPick.Contains(lastShape)) {
                    availableShapesToPick.Remove(lastShape);
                }
            }

            var randIndex = UnityEngine.Random.Range(0, availableShapesToPick.Count);
            var currentShape = availableShapesToPick[randIndex];
            var currentTile = new TileInfo(currentShape, new TileCoordsInArray(row, col), parent);

            tileArray[row, col] = currentTile;

            if (prevMatches.Count > 0 && prevMatches[prevMatches.Count - 1].TileShapeType != currentShape) {
                prevMatches.Clear();
                availableShapesToPick = new List<TileShapeType>(allShapeTypesToPick);
            }

            if (prevMatches.Count == 0 || prevMatches[prevMatches.Count - 1].TileShapeType == currentShape) {
                prevMatches.Add(currentTile);
            }

            return availableShapesToPick;
        }

        private static List<TileShapeType> GetAllShapeTypes(bool excludeNoneShapeType) {

            var result = new List<TileShapeType>();

            foreach (TileShapeType shapeType in Enum.GetValues(typeof(TileShapeType))) {
                if (excludeNoneShapeType && shapeType == TileShapeType.None) {
                    continue;
                }

                result.Add(shapeType);
            }

            return result;
        }

        private static void AddTileToTempMatch(List<TileInfo> tempMatch, TileInfo currentTile) {
            if (currentTile.TileShapeType != TileShapeType.None) {
                tempMatch.Add(currentTile);
            }
        }

        #endregion
    }
}
