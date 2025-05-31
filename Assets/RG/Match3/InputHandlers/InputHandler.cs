using UnityEngine;

namespace Match3.InputHandlers {
    public abstract class InputHandler {
        public abstract bool GetUserClickPosiiton(out Vector3 pos);

        public static InputHandler CreateInstance() {
#if UNITY_EDITOR || UNITY_STANDALONE
            return new MouseInputHandler();
#else
            return new TouchInputHandler();
#endif
        }
    }
}
