using UnityEngine;

namespace Match3.InputHandlers {
    public class TouchInputHandler : InputHandler {

        public override bool GetUserClickPosiiton(out Vector3 pos) {
            if (Input.touchCount > 0) {
                pos = Input.GetTouch(0).position;
                return true;
            }

            pos = Vector3.zero;
            return false;
        }
    }
}
