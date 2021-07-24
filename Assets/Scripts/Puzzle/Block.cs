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

        private bool _isInCombo = false;
        private bool _swapFromLeft = false;

        private void Awake()
        {
            _rb2d = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void OnSwap(bool swapFromLeft)
        {
            _swapFromLeft = swapFromLeft;
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

        #region Neighbor calculations
        private void CompareNeighbors()
        {
            var totalBlocksInCombo = new List<Block>();
            var directionalBlockList = new List<Block>();

            if (!_swapFromLeft)
            {
                GetNeighborsInDirectionRecursive(this, Vector3.left, directionalBlockList);
                HandleNeighborsList(directionalBlockList, totalBlocksInCombo);
            }
            else
            {
                GetNeighborsInDirectionRecursive(this, Vector3.right, directionalBlockList);
                HandleNeighborsList(directionalBlockList, totalBlocksInCombo);
            }

            GetNeighborsInDirectionRecursive(this, Vector3.up, directionalBlockList);
            HandleNeighborsList(directionalBlockList, totalBlocksInCombo);

            GetNeighborsInDirectionRecursive(this, Vector3.down, directionalBlockList);
            HandleNeighborsList(directionalBlockList, totalBlocksInCombo);

            DeleteBlocks(totalBlocksInCombo);
        }

        private void GetNeighborsInDirectionRecursive(Block start, Vector3 direction, List<Block> blockList)
        {
            // Need to change raycast layer because starting block will block 2D raycasts.
            start.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            var adjacentBlock = start.GetAdjacentBlock(start.transform.TransformDirection(direction));
            start.gameObject.layer = LayerMask.NameToLayer("Default");

            if (adjacentBlock == null)
            {
                return;
            }

            if (start.GetColor() != adjacentBlock.GetColor())
            {
                return;
            }

            blockList.Add(adjacentBlock);
            GetNeighborsInDirectionRecursive(adjacentBlock, direction, blockList);
        }

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

        private void HandleNeighborsList(List<Block> directionalBlockList, List<Block> totalBlocksInCombo)
        {
            if (directionalBlockList.Count >= 2)
            {
                if (!_isInCombo)
                {
                    directionalBlockList.Add(this);
                    _isInCombo = true;
                }
                totalBlocksInCombo.AddRange(directionalBlockList);
            }
            directionalBlockList.Clear();
        }

        private void DeleteBlocks(List<Block> blocks)
        {
            foreach (var block in blocks)
            {
                Destroy(block.gameObject);
            }
            blocks.Clear();
        }
        #endregion

        #region Utility methods
        public void EnableRigidbody(bool enable) => _rb2d.bodyType = enable ? RigidbodyType2D.Dynamic : RigidbodyType2D.Kinematic;

        public string GetColor() => Utility.GetColorName(_spriteRenderer.color);

        public void SetColor(Color color) => _spriteRenderer.color = color;
        #endregion
    }
}
