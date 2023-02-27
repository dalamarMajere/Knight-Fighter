using UnityEngine;

namespace Movement
{
    public enum Direction
    {
        Backward,
        Forward
    }
    
    public class PlayerSpriteDirection
    {
        private GameObject _spriteGameObject;
        private Direction _currentDirection = Direction.Forward;

        public PlayerSpriteDirection(GameObject playerSprite)
        {
            _spriteGameObject = playerSprite;
        }

        public void SetSpriteDirectionByInput(float horizontalInput)
        {
            Direction newDirection = GetNewDirection(horizontalInput);

            if (newDirection != _currentDirection)
            {
                FlipSprite();
            }

            _currentDirection = newDirection;
        }

        private Direction GetNewDirection(float horizontalInput)
        {
            return horizontalInput switch
            {
                -1 => Direction.Backward,
                1 => Direction.Forward,
                _ => _currentDirection
            };
        }

        private void FlipSprite()
        {
            var scale = _spriteGameObject.transform.localScale;
            _spriteGameObject.transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
        }
    }
}