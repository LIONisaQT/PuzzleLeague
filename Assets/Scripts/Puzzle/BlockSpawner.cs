using System.Collections.Generic;
using UnityEngine;

namespace PuzzleLeague.Puzzle
{
    public class BlockSpawner : MonoBehaviour
    {
        private const int GRID_WIDTH = 6;
        private const float RISE_TIME = 2f;

        [SerializeField] private Transform _blockParent;
        [SerializeField] private List<Block> _blockList;

        private Block[] _blockBase = new Block[GRID_WIDTH];

        private float _previousTime;

        private void Update()
        {
            // TODO: This shitty ass logic is probably why blocks get misaligned. Find something better.
            if (Time.time - _previousTime > (Input.GetKey(KeyCode.LeftShift) ? RISE_TIME / Riser.RISE_FORCE : RISE_TIME))
            {
                SpawnBlock();
                _previousTime = Time.time;
            }

            if (Input.GetKeyDown(KeyCode.Delete)) DestroyAllBlocks();
        }

        private void SpawnBlock()
        {
            for (var i = 0; i < GRID_WIDTH; i++)
            {
                // TODO: Use some sort of weighted random so adjacent blocks aren't the same as often.
                var randomBlock = Utility.GetRandomElementFromList(_blockList);
                var position = GetLowestPositionForBlock(i);
                var block = Instantiate(randomBlock, position, Quaternion.identity, _blockParent);
                block.EnableRigidbody(false);

                if (_blockBase[i] != null)
                {
                    _blockBase[i].EnableRigidbody(true);
                }

                _blockBase[i] = block;
            }
        }

        /// <summary>
        /// Finds lowest y position to place an incoming block for a given
        /// column. If there are no blocks at the base level, sets to block
        /// parent's position. Otherwise, places block under lowest block for
        /// that column.
        /// </summary>
        /// <param name="col">Column.</param>
        /// <returns>Vector3 position for an incoming block.</returns>
        private Vector3 GetLowestPositionForBlock(int col) => new Vector3(col, _blockBase[col] != null ? _blockBase[col].transform.position.y - 1 : _blockParent.position.y, 0);

        private void DestroyAllBlocks()
        {
            foreach (Transform child in _blockParent.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
