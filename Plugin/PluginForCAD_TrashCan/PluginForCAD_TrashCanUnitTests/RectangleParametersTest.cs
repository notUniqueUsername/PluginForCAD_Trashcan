using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PluginForCAD_TrashcanLibrary;

namespace PluginForCAD_TrashCanUnitTests
{
    [TestFixture]
    class RectangleParametersTest
    {
        private RectangleParameters _rectangleParameters;
        private List<double> _parametersList;
        [SetUp]
        public void InitParameters()
        {
            _parametersList = new List<double>();
        }


        [Test]
        [TestCase(2, 1, 35, 20, 25, 20, 25, 45, true,
            TestName = "Тест конструктора со стойкой корректными значениями")]
        [TestCase(2, 1, 40, 50, 50, 50, 50, 60, true,
            TestName = "Тест конструктора со стойкой корректными максимальными значениями")]
        [TestCase(2, 1, 30, 20, 20, 20, 20, 40, true,
            TestName = "Тест конструктора со стойкой корректными минимальными значениями")]
        public void TestNameSet_CorrectValue_WithStand(double bottomThickness, double wallThickness, double urnHeight, double topWidth, double bottomWidth, double topLength, double bottomLength, double standHeight, bool stand)
        {
            _parametersList.Add(bottomThickness);
            _parametersList.Add(wallThickness);
            _parametersList.Add(urnHeight);
            _parametersList.Add(bottomWidth);
            _parametersList.Add(topWidth);
            _parametersList.Add(bottomLength);
            _parametersList.Add(topLength);
            _parametersList.Add(standHeight);
            var expectedBottomThickness = bottomThickness * 10;
            var expectedWallThickness = wallThickness * 10;
            var expectedUrnHeight = urnHeight * 10;
            var expectedTopWidth = topWidth * 10;
            var expectedBotomWidth = bottomWidth * 10;
            var expectedTopLength = topLength * 10;
            var expectedBottomLength = bottomLength * 10;
            var expectedStandHeight = standHeight * 10;
            _rectangleParameters = new RectangleParameters(_parametersList, stand);
            Assert.AreEqual(expectedBottomThickness, _rectangleParameters.BottomThickness);
            Assert.AreEqual(expectedWallThickness, _rectangleParameters.WallThickness);
            Assert.AreEqual(expectedUrnHeight, _rectangleParameters.UrnHeight);
            Assert.AreEqual(expectedTopWidth, _rectangleParameters.WidthTop);
            Assert.AreEqual(expectedBotomWidth, _rectangleParameters.WidthBottom);
            Assert.AreEqual(expectedTopLength, _rectangleParameters.LengthTop);
            Assert.AreEqual(expectedBottomLength, _rectangleParameters.LengthBottom);
            Assert.AreEqual(expectedStandHeight, _rectangleParameters.StandHeight);
            Assert.AreEqual(stand, _rectangleParameters.Stand);
        }

        [Test]
        [TestCase(2, 1, 35, 20, 25, 20, 25, 45, false,
            TestName = "Тест конструктора корректными значениями")]
        [TestCase(2, 1, 40, 50, 50, 50, 50, 60, false,
            TestName = "Тест конструктора корректными максимальными значениями")]
        [TestCase(2, 1, 30, 20, 20, 20, 20, 40, false,
            TestName = "Тест конструктора корректными минимальными значениями")]
        public void TestNameSet_CorrectValue(double bottomThickness, double wallThickness,
            double urnHeight, double topWidth, double bottomWidth, double topLength,
            double bottomLength, double standHeight, bool stand)
        {
            _parametersList.Add(bottomThickness);
            _parametersList.Add(wallThickness);
            _parametersList.Add(urnHeight);
            _parametersList.Add(bottomWidth);
            _parametersList.Add(topWidth);
            _parametersList.Add(bottomLength);
            _parametersList.Add(topLength);
            _parametersList.Add(standHeight);
            var expectedBottomThickness = bottomThickness * 10;
            var expectedWallThickness = wallThickness * 10;
            var expectedUrnHeight = urnHeight * 10;
            var expectedTopWidth = topWidth * 10;
            var expectedBotomWidth = bottomWidth * 10;
            var expectedTopLength = topLength * 10;
            var expectedBottomLength = bottomLength * 10;
            var expectedStandHeight = 0;
            _rectangleParameters = new RectangleParameters(_parametersList, stand);
            Assert.AreEqual(expectedBottomThickness, _rectangleParameters.BottomThickness);
            Assert.AreEqual(expectedWallThickness, _rectangleParameters.WallThickness);
            Assert.AreEqual(expectedUrnHeight, _rectangleParameters.UrnHeight);
            Assert.AreEqual(expectedTopWidth, _rectangleParameters.WidthTop);
            Assert.AreEqual(expectedBotomWidth, _rectangleParameters.WidthBottom);
            Assert.AreEqual(expectedTopLength, _rectangleParameters.LengthTop);
            Assert.AreEqual(expectedBottomLength, _rectangleParameters.LengthBottom);
            Assert.AreEqual(expectedStandHeight, _rectangleParameters.StandHeight);
            Assert.AreEqual(stand, _rectangleParameters.Stand);
        }

        [Test]
        [TestCase(3, 1, 35, 20, 25, 20, 25, 45, true,
            TestName = "Толщина дна должна быть меньше 2")]
        [TestCase(2, 2, 35, 20, 25, 20, 25, 45, true,
            TestName = "Толщина стенок должна быть меньше 1")]
        [TestCase(2, 1, 45, 20, 25, 20, 25, 45, true,
            TestName = "Высота урны должна быть меньше 40 и больше 30")]
        [TestCase(2, 1, 40, 20, 25, 20, 25, 70, true,
            TestName = "Высота стойки должна быть меньше 60 и на 10 больше высоты урны")]
        [TestCase(2, 1, 35, 20, 51, 20, 25, 45, false,
            TestName = "Ширина нижнего основания урны должна быть меньше 50")]
        [TestCase(2, 1, 35, 25, 20, 25, 20, 45, true,
            TestName = "Высота и ширина нижнего основания должны лежать в пределах от 0,5 до 1 размера верхнего основания")]
        [TestCase(2, 1, 35, 21, 25, 20, 25, 45, true,
            TestName = "Если выбрана стойка, верхнее и нижнее основания должны быть квадратами")]
        [TestCase(2, 1, 35, 25, 50, 25, 50, 45, true,
            TestName = "Наклон превышает 60 градусов")]
        public void TestNameSet_InCorrectValue(double bottomThickness, double wallThickness,
            double urnHeight, double topWidth, double bottomWidth, double topLength,
            double bottomLength, double standHeight, bool stand)
        {
            _parametersList.Add(bottomThickness);
            _parametersList.Add(wallThickness);
            _parametersList.Add(urnHeight);
            _parametersList.Add(bottomWidth);
            _parametersList.Add(topWidth);
            _parametersList.Add(bottomLength);
            _parametersList.Add(topLength);
            _parametersList.Add(standHeight);

            Assert.Throws<ArgumentException>(
                () => { _rectangleParameters = new RectangleParameters(_parametersList, stand); },
                "Должна быть ошибка");
        }
    }
}
