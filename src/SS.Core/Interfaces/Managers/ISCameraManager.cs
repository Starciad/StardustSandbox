using Microsoft.Xna.Framework;

using StardustSandbox.Core.Mathematics.Primitives;

namespace StardustSandbox.Core.Interfaces.Managers
{
    public interface ISCameraManager : ISManager
    {
        Vector2 Position { get; set; }
        float Rotation { get; set; }
        Vector2 Origin { get; set; }
        float Zoom { get; set; }
        float MinimumZoom { get; set; }
        float MaximumZoom { get; set; }

        void Move(Vector2 direction);
        void Rotate(float deltaRadians);
        void ZoomIn(float deltaZoom);
        void ZoomOut(float deltaZoom);
        void ClampZoom(float value);
        Vector2 WorldToScreen(Vector2 worldPosition);
        Vector2 ScreenToWorld(Vector2 screenPosition);
        Matrix GetViewMatrix();

        bool InsideCameraBounds(Vector2 targetPosition, SSize2 targetSize, bool inWorldPosition, float toleranceFactor = 0f);
    }
}
