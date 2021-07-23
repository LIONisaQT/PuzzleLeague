using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class DirectionTest
    {
        [Test]
        public void BlueCheck()
        {
            var blue = GetColorFromString("#24AAC3");
            Assert.AreEqual("BLUE", Utility.GetColorName(blue));
        }

        [Test]
        public void GreenCheck()
        {
            var green = GetColorFromString("#24AAC3");
            Assert.AreEqual("BLUE", Utility.GetColorName(green));
        }

        [Test]
        public void PurpleCheck()
        {
            var purple = GetColorFromString("#24AAC3");
            Assert.AreEqual("BLUE", Utility.GetColorName(purple));
        }

        [Test]
        public void RedCheck()
        {
            var red = GetColorFromString("#24AAC3");
            Assert.AreEqual("BLUE", Utility.GetColorName(red));
        }

        [Test]
        public void YellowCheck()
        {
            var yellow = GetColorFromString("#24AAC3");
            Assert.AreEqual("BLUE", Utility.GetColorName(yellow));
        }

        [Test]
        public void ColorNotValidCheck()
        {
            var invalidColor = GetColorFromString("#FFFFFF");
            Assert.AreEqual("Color is not defined!", Utility.GetColorName(invalidColor));
        }

        [Test]
        public void BadColorHexCheck()
        {
            var bad = GetColorFromString("EJIAF;JI");
            Assert.AreEqual("Color is not defined!", Utility.GetColorName(bad));
        }

        private Color GetColorFromString(string htmlValue)
        {
            if (ColorUtility.TryParseHtmlString(htmlValue, out var color))
            {
                return color;
            }

            return Color.white;
        }
    }
}
