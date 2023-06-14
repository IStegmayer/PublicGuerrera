using UnityEngine;

namespace Game._Scripts.Player
{
    public struct FrameInput
    {
        public bool Jump;
        public bool Run;
        public bool Attack;
        public bool Roll;
        public bool Block;
        public Vector2 Movement;
        public Vector2 CameraMovement;
    }
}