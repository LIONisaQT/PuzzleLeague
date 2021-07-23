using System.Collections.Generic;
using UnityEngine;

namespace PuzzleLeague.Puzzle
{
    public class Block : Riser
    {
        private Rigidbody2D _rb2d;
        private SpriteRenderer _spriteRenderer;

        private bool _isFalling = false;
        private float _previousVelocity;
        private readonly float _lookDistance = 1f;

        private List<Block> _blocksToDestroy;

        private void Awake()
        {
            _rb2d = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _blocksToDestroy = new List<Block>();
        }

        public void OnSwap()
        {
            CheckBelow();
        }

        private void CheckBelow()
        {
            var downHit = GetAdjacentBlock(transform.TransformDirection(Vector3.down));
            if (downHit)
            {
                CompareNeighbors();
            }
            else
            {
                _isFalling = true;
            }
        }

        protected new void Update()
        {
            base.Update();

            if (_isFalling)
            {
                if (_previousVelocity < 0 && _previousVelocity < _rb2d.velocity.y)
                {
                    _isFalling = false;
                    CompareNeighbors();
                }
                else
                {
                    _previousVelocity = _rb2d.velocity.y;
                }
            }
        }

        private void CompareNeighbors()
        {
            // Change layer because it was colliding with itself, but setting "Queries Start In Colliders" in Physics2D settings broke raycasts.
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

            var adjacentBlock = GetAdjacentBlock(transform.TransformDirection(Vector3.left));
            if (adjacentBlock != null)
            {
                if (GetColor() == adjacentBlock.GetColor())
                {
                    _blocksToDestroy.Add(adjacentBlock);
                }
            }

            adjacentBlock = GetAdjacentBlock(transform.TransformDirection(Vector3.right));
            if (adjacentBlock != null)
            {
                if (GetColor() == adjacentBlock.GetColor())
                {
                    _blocksToDestroy.Add(adjacentBlock);
                }
            }

            adjacentBlock = GetAdjacentBlock(transform.TransformDirection(Vector3.up));
            if (adjacentBlock != null)
            {
                if (GetColor() == adjacentBlock.GetColor())
                {
                    _blocksToDestroy.Add(adjacentBlock);
                }
            }

            adjacentBlock = GetAdjacentBlock(transform.TransformDirection(Vector3.down));
            if (adjacentBlock != null)
            {
                if (GetColor() == adjacentBlock.GetColor())
                {
                    _blocksToDestroy.Add(adjacentBlock);
                }
            }

            if (_blocksToDestroy.Count > 0)
            {
                _blocksToDestroy.Add(this);
                DeleteBlocks(_blocksToDestroy);
            }
            else
            {
                gameObject.layer = LayerMask.NameToLayer("Default");
            }
        }

        private void DeleteBlocks(List<Block> blocks)
        {
            foreach (var block in blocks)
            {
                Destroy(block.gameObject);
            }
        }

        public void EnableRigidbody(bool enable) => _rb2d.bodyType = enable ? RigidbodyType2D.Dynamic : RigidbodyType2D.Kinematic;

        public string GetColor() => Utility.GetColorName(_spriteRenderer.color);

        private Block GetAdjacentBlock(Vector3 direction)
        {
            var adjacentCollider = Utility.GetRaycastResultCollider(transform.position, direction, _lookDistance);
            if (adjacentCollider)
            {
                if (adjacentCollider.TryGetComponent<Block>(out var block))
                {
                    return block;
                }
            }

            return null;
        }
    }
}
