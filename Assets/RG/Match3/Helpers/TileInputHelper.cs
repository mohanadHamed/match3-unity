using UnityEngine;
using Match3.DataTypes;

namespace Match3.Helpers {

    public static class TileInputHelper {

        public static bool GetClickedTile(TileInfo[,] tileArray, Vector3 clickPos, out TileInfo clickedTile) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(clickPos);
            if (Physics.Raycast(ray, out hit)) {
                var numRows = tileArray.GetLength(0);
                var numColumns = tileArray.GetLength(1);

                for (int row = 0; row < numRows; row++) {

                    for (int col = 0; col < numColumns; col++) {

                        var tile = tileArray[row, col];

                        if (tile.TileInstanceObject.transform == hit.transform) {
                            clickedTile = tile;
                            return true;
                        }
                    }
                }
            }

            clickedTile = null;
            return false;
        }
    }
}
