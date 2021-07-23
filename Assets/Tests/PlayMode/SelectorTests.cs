using NUnit.Framework;
using PuzzleLeague.Puzzle;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class SelectorTests
    {
        [UnityTest]
        public IEnumerator MoveUp()
        {
            var selector = SetUpMoveTestsAndGetSelector();
            selector.Move(SelectorLogic.MoveOption.UP);

            yield return null;

            Assert.GreaterOrEqual(selector.transform.position.y, 1);
        }

        [UnityTest]
        public IEnumerator MoveDown()
        {
            var selector = SetUpMoveTestsAndGetSelector();
            selector.Move(SelectorLogic.MoveOption.DOWN);

            yield return null;

            Assert.GreaterOrEqual(selector.transform.position.y, -1);
        }

        [UnityTest]
        public IEnumerator MoveLeft()
        {
            var selector = SetUpMoveTestsAndGetSelector();
            selector.Move(SelectorLogic.MoveOption.LEFT);

            yield return null;

            Assert.GreaterOrEqual(selector.transform.position.x, -1);
        }

        [UnityTest]
        public IEnumerator MoveRight()
        {
            var selector = SetUpMoveTestsAndGetSelector();
            selector.Move(SelectorLogic.MoveOption.RIGHT);

            yield return null;

            Assert.GreaterOrEqual(selector.transform.position.x, 1);
        }

        private SelectorLogic SetUpMoveTestsAndGetSelector()
        {
            var playArea = SpawnPlayArea();
            var selectorObject = SpawnSelectorObject(playArea);
            return selectorObject.AddComponent<SelectorLogic>();
        }

        private GameObject SpawnPlayArea()
        {
            var playArea = new GameObject();
            playArea.transform.position.Set(2.5f, 6, 0);
            playArea.transform.localScale.Set(6, 13, 1);
            return playArea;
        }

        private GameObject SpawnSelectorObject(GameObject playArea)
        {
            var selectorObject = new GameObject();
            selectorObject.transform.parent = playArea.transform;
            selectorObject.transform.position.Set(0, 0, -1);
            return selectorObject;
        }
    }
}
