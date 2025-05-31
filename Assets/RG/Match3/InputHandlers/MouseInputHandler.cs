using UnityEngine;

namespace Match3.InputHandlers {
    public class MouseInputHandler : InputHandler {

        public override bool GetUserClickPosiiton(out Vector3 pos) {
            if(Input.GetMouseButton(0)) {
                pos = Input.mousePosition;
                return true;
            }

            pos = Vector3.zero;
            return false;
        }
    }
}
