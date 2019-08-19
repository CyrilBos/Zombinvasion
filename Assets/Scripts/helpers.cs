using UnityEngine;

namespace Helpers
{
    static public class PositionHelpers
    {
        /**
        * Computes the position of the unit to form a circle with the others
*/
        static public Vector3 ComputeUnitPositionAroundTarget(Vector3 center, int index, float positioningOffset = 0.3f)
        {
            if (index == 0)
                return center;

            int angle = index * (360 / 9);
            Vector3 direction = Quaternion.Euler(0, 0, angle) * Vector3.forward;
            // index / 9 to move to outer circles after the ninth
            return center + direction * positioningOffset * (1 + index / 9);
        }
    }

    static public class LayerHelpers
    {
        static public int NoGroundMask = ~LayerMask.GetMask("Ground");

    }

}
