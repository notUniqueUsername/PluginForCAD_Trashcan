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
        [TestCase(2, 1, 35, 20, 25, 20, 25, 45, true, false,
            TestName = "Тест конструктора со стойкой корректными значениями")]
        [TestCase(2, 1, 40, 50, 50, 50, 50, 60, true, true,
            TestName = "Тест конструктора со стойкой корректными максимальными значениями")]
        [TestCase(2, 1, 30, 20, 20, 20, 20, 40, true, false,
            TestName = "Тест конструктора со стойкой корректными минимальными значениями")]
        public void TestConstructor_CorrectValue_WithStand(double bottomThickness, double wallThickness, double urnHeight,
            double topWidth, double bottomWidth, double topLength,
            double bottomLength, double standHeight, bool stand, bool ashtray)
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
            _rectangleParameters = new RectangleParameters(_parametersList, stand, ashtray);
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
        [TestCase(2, 1, 35, 20, 25, 20, 25, false, true,
            TestName = "Тест конструктора корректными значениями")]
        [TestCase(2, 1, 40, 50, 50, 50, 50, false, false,
            TestName = "Тест конструктора корректными максимальными значениями")]
        [TestCase(2, 1, 30, 20, 20, 20, 20, false, true,
            TestName = "Тест конструктора корректными минимальными значениями")]
        public void TestConstructor_CorrectValue(double bottomThickness, double wallThickness,
            double urnHeight, double topWidth, double bottomWidth, double topLength,
            double bottomLength, bool stand, bool ashtray)
        {
            _parametersList.Add(bottomThickness);
            _parametersList.Add(wallThickness);
            _parametersList.Add(urnHeight);
            _parametersList.Add(bottomWidth);
            _parametersList.Add(topWidth);
            _parametersList.Add(bottomLength);
            _parametersList.Add(topLength);
            var expectedBottomThickness = bottomThickness * 10;
            var expectedWallThickness = wallThickness * 10;
            var expectedUrnHeight = urnHeight * 10;
            var expectedTopWidth = topWidth * 10;
            var expectedBotomWidth = bottomWidth * 10;
            var expectedTopLength = topLength * 10;
            var expectedBottomLength = bottomLength * 10;
            _rectangleParameters = new RectangleParameters(_parametersList, stand, ashtray);
            Assert.AreEqual(expectedBottomThickness, _rectangleParameters.BottomThickness);
            Assert.AreEqual(expectedWallThickness, _rectangleParameters.WallThickness);
            Assert.AreEqual(expectedUrnHeight, _rectangleParameters.UrnHeight);
            Assert.AreEqual(expectedTopWidth, _rectangleParameters.WidthTop);
            Assert.AreEqual(expectedBotomWidth, _rectangleParameters.WidthBottom);
            Assert.AreEqual(expectedTopLength, _rectangleParameters.LengthTop);
            Assert.AreEqual(expectedBottomLength, _rectangleParameters.LengthBottom);
            Assert.AreEqual(stand, _rectangleParameters.Stand);
        }

        [Test]
        [TestCase(3, 1, 35, 20, 25, 20, 25, 45, true, false,
            TestName = "Толщина дна должна быть меньше 2")]
        [TestCase(2, 2, 35, 20, 25, 20, 25, 45, true, true,
            TestName = "Толщина стенок должна быть меньше 1")]
        [TestCase(2, 1, 45, 20, 25, 20, 25, 45, true, true,
            TestName = "Высота урны должна быть меньше 40 и больше 30")]
        [TestCase(2, 1, 40, 20, 25, 20, 25, 70, true, false,
            TestName = "Высота стойки должна быть меньше 60 и на 10 больше высоты урны")]
        [TestCase(2, 1, 35, 20, 51, 20, 25, 45, false, true,
            TestName = "Ширина нижнего основания урны должна быть меньше 50")]
        [TestCase(2, 1, 35, 25, 20, 25, 20, 45, true, false,
            TestName = "Высота и ширина нижнего основания должны лежать в пределах от 0,5 до 1 размера верхнего основания")]
        [TestCase(2, 1, 35, 21, 25, 20, 25, 45, true, true,
            TestName = "Если выбрана стойка, верхнее и нижнее основания должны быть квадратами")]
        [TestCase(2, 1, 35, 25, 50, 25, 50, 45, true, false,
            TestName = "Наклон превышает 60 градусов")]
        public void TestConstructor_InCorrectValue(double bottomThickness, double wallThickness,
            double urnHeight, double topWidth, double bottomWidth, double topLength,
            double bottomLength, double standHeight, bool stand, bool ashtray)
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
                () => { _rectangleParameters = new RectangleParameters(_parametersList, stand, ashtray); },
                "Должна быть ошибка");
        }

        [Test]
        [TestCase(double.NaN, 1, 35, 25, 50, 25, 50, 45, true, false,
            TestName = "Тест параметра 0 на NaN")]
        [TestCase(2, double.NaN, 35, 25, 50, 25, 50, 45, true, false,
            TestName = "Тест параметра 1 на NaN")]
        [TestCase(2, 1, double.NaN, 25, 50, 25, 50, 45, true, false,
            TestName = "Тест параметра 2 на NaN")]
        [TestCase(2, 1, 35, double.NaN, 50, 25, 50, 45, true, false,
            TestName = "Тест параметра 3 на NaN")]
        [TestCase(2, 1, 35, 25, double.NaN, 25, 50, 45, true, false,
            TestName = "Тест параметра 4 на NaN")]
        [TestCase(2, 1, 35, 25, 50, double.NaN, 50, 45, true, false,
            TestName = "Тест параметра 5 на NaN")]
        [TestCase(2, 1, 35, 25, 50, 25, double.NaN, 45, true, false,
            TestName = "Тест параметра 6 на NaN")]
        [TestCase(2, 1, 35, 25, 50, 25, 50, double.NaN, true, false,
            TestName = "Тест параметра 7 на NaN")]
        [TestCase(double.PositiveInfinity, 1, 35, 25, 50, 25, 50, 45, true, false,
            TestName = "Тест параметра 0 на infinity")]
        [TestCase(2, double.PositiveInfinity, 35, 25, 50, 25, 50, 45, true, false,
            TestName = "Тест параметра 1 на infinity")]
        [TestCase(2, 1, double.PositiveInfinity, 25, 50, 25, 50, 45, true, false,
            TestName = "Тест параметра 2 на infinity")]
        [TestCase(2, 1, 35, double.PositiveInfinity, 50, 25, 50, 45, true, false,
            TestName = "Тест параметра 3 на infinity")]
        [TestCase(2, 1, 35, 25, double.PositiveInfinity, 25, 50, 45, true, false,
            TestName = "Тест параметра 4 на infinity")]
        [TestCase(2, 1, 35, 25, 50, double.PositiveInfinity, 50, 45, true, false,
            TestName = "Тест параметра 5 на infinity")]
        [TestCase(2, 1, 35, 25, 50, 25, double.PositiveInfinity, 45, true, false,
            TestName = "Тест параметра 6 на infinity")]
        [TestCase(2, 1, 35, 25, 50, 25, 50, double.PositiveInfinity, true, false,
            TestName = "Тест параметра 7 на infinity")]
        public void TestConstructor_ParameterListTest(double bottomThickness, double wallThickness,
            double urnHeight, double topWidth, double bottomWidth, double topLength,
            double bottomLength, double standHeight, bool stand, bool ashtray)
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
                () => { _rectangleParameters = new RectangleParameters(_parametersList, stand, ashtray); },
                "Должна быть ошибка");
        }


        [Test]
        [TestCase(2, 1, 30, 20, 20, 20, 20, 40, false, true,
            TestName = "Тест конструктора списком параметров с неправильным количеством параметров")]
        public void TestConstrutor_InCorrectParameterListCount(double bottomThickness, double wallThickness,
            double urnHeight, double topWidth, double bottomWidth, double topLength,
            double bottomLength, double standHeight, bool stand, bool ashtray)
        {
            _parametersList.Add(bottomThickness);
            Assert.Throws<ArgumentException>(
                () => { _rectangleParameters = new RectangleParameters(_parametersList, stand, ashtray); },
                "Должна быть ошибка");
        }
    }
}
