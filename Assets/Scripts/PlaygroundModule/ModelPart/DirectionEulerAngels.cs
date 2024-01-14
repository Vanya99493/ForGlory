using UnityEngine;

namespace PlaygroundModule.ModelPart
{
    public static class DirectionEulerAngels
    {
        public static Vector3 GetDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return new Vector3(0, 180, 0);
                case Direction.Down:
                    return new Vector3(0, 0, 0);
                case Direction.Left:
                    return new Vector3(0, 90, 0);
                case Direction.Right:
                    return new Vector3(0, -90, 0);
            }

            return new Vector3(0, 0, 0);
        }
    }
}