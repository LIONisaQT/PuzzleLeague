using UnityEngine;

namespace PuzzleLeague.Puzzle
{
    public class Riser : MonoBehaviour
    {
        public const float RISE_TIME = 0.0025f;  // Time between rise increments (in seconds).
        public const float RISE_AMOUNT = 0.002f; // Base rise amount.
        public const float RISE_MULTIPLIER = 1f; // Increments rise amount over the duration of a match.
        public const float RISE_FORCE = 4f;      // Multiplier for when the player forces rise.

        private float _previousTime;

        protected void Update()
        {
            // TODO: This shitty ass logic is probably why blocks get misaligned. Find something better.
            if (Time.time - _previousTime > RISE_TIME)
            {
                transform.position += new Vector3(0, (Input.GetKey(KeyCode.LeftShift) ? RISE_AMOUNT * RISE_FORCE : RISE_AMOUNT) * RISE_MULTIPLIER, 0);
                _previousTime = Time.time;
            }
        }
    }
}
