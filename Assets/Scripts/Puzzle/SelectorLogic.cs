using UnityEngine;

namespace PuzzleLeague.Puzzle
{
    public class SelectorLogic : Riser
    {
        // TODO: Maybe find better place for this.
        private const uint GRID_WIDTH = 6;
        private const uint GRID_HEIGHT = 13;

        [SerializeField] private readonly GameObject _leftSelector;
        [SerializeField] private readonly GameObject _rightSelector;

        public enum MoveOption
        {
            LEFT,
            RIGHT,
            UP,
            DOWN
        }

        protected new void Update()
        {
            base.Update();

            HandleMovement();
            HandleSwap();
            HandleDelete();
        }

        private void HandleMovement()
        {
            // TODO: Allow custom keys.
            if (Input.GetKeyDown(KeyCode.A))
            {
                Move(MoveOption.LEFT);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                Move(MoveOption.RIGHT);
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                Move(MoveOption.UP);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                Move(MoveOption.DOWN);
            }
        }

        public void Move(MoveOption direction)
        {
            switch (direction)
            {
                case MoveOption.LEFT:
                    transform.position += new Vector3(-1, 0, 0);
                    if (!IsValidMove())
                    {
                        Move(MoveOption.RIGHT);
                    }
                    break;
                case MoveOption.RIGHT:
                    transform.position += new Vector3(1, 0, 0);
                    if (!IsValidMove())
                    {
                        Move(MoveOption.LEFT);
                    }
                    break;
                case MoveOption.UP:
                    transform.position += new Vector3(0, 1, 0);
                    if (!IsValidMove())
                    {
                        Move(MoveOption.DOWN);
                    }
                    break;
                case MoveOption.DOWN:
                    transform.position += new Vector3(0, -1, 0);
                    if (!IsValidMove())
                    {
                        Move(MoveOption.UP);
                    }
                    break;
            }
        }

        /// <summary>
        /// Using raycast to find blocks because I'm too dumb to figure out a
        /// grid system. That also means the z position of the selector is <0
        /// so the raycast can work. Oh well. TODO: See if it's even worth
        /// fixing?
        /// </summary>
        private void HandleSwap()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                var leftHit = Utility.GetRaycastResultCollider(_leftSelector.transform.position, _leftSelector.transform.TransformDirection(Vector3.forward));
                if (leftHit)
                {
                    DoSwap(leftHit.gameObject.transform, true);
                }

                var rightHit = Utility.GetRaycastResultCollider(_rightSelector.transform.position, _leftSelector.transform.TransformDirection(Vector3.forward));
                if (rightHit)
                {
                    DoSwap(rightHit.gameObject.transform, false);
                }
            }
        }

        private void HandleDelete()
        {
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                var leftHit = Utility.GetRaycastResultCollider(_leftSelector.transform.position, _leftSelector.transform.TransformDirection(Vector3.forward));
                if (leftHit)
                {
                    Destroy(leftHit.gameObject);
                }

                var rightHit = Utility.GetRaycastResultCollider(_rightSelector.transform.position, _leftSelector.transform.TransformDirection(Vector3.forward));
                if (rightHit)
                {
                    Destroy(rightHit.gameObject);
                }
            }
        }

        private void DoSwap(Transform tf, bool isLeft)
        {
            var newPostion = new Vector3(tf.position.x + (isLeft ? 1 : -1), tf.position.y, tf.position.z);
            tf.SetPositionAndRotation(newPostion, tf.rotation);
            tf.GetComponent<Block>().OnSwap();
        }

        private bool IsValidMove()
        {
            foreach (Transform children in transform)
            {
                var roundedX = Mathf.RoundToInt(children.transform.position.x);
                var roundedY = Mathf.RoundToInt(children.transform.position.y);

                // TODO: Disallow selector to go below/above play area.
                if (roundedX < 0 || roundedX >= GRID_WIDTH || roundedY < 0 || roundedY >= GRID_HEIGHT)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
