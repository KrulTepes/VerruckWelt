using UnityEngine;

namespace Utility
{
    public static class TransformExtension
    {
        public static Bounds InverseTransformBounds(this Transform transform, Bounds worldBounds)
        {
            var center = transform.InverseTransformPoint(worldBounds.center);

            // transform the local extents' axes
            var extents = worldBounds.extents;
            var axisX = transform.InverseTransformVector(extents.x, 0, 0);
            var axisY = transform.InverseTransformVector(0, extents.y, 0);
            var axisZ = transform.InverseTransformVector(0, 0, extents.z);

            // sum their absolute value to get the world extents
            extents.x = Mathf.Abs(axisX.x) + Mathf.Abs(axisY.x) + Mathf.Abs(axisZ.x);
            extents.y = Mathf.Abs(axisX.y) + Mathf.Abs(axisY.y) + Mathf.Abs(axisZ.y);
            extents.z = Mathf.Abs(axisX.z) + Mathf.Abs(axisY.z) + Mathf.Abs(axisZ.z);

            return new Bounds {center = center, extents = extents};
        }
    }
}