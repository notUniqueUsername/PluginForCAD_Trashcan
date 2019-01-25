using System;
using System.Collections.Generic;
using NUnit.Framework;
using PluginForCAD_TrashcanLibrary;

namespace PluginForCAD_TrashCanUnitTests
{
    [TestFixture]
    public class CircleParametersTest
    {
        private CircleParameters _circleParameters;
        private List<double> _parametersList;
        [SetUp]
        public void InitParameters()
        {
            _parametersList = new List<double>();
        }


        [Test]
        [TestCase(2, 1, 35, 20, 20, 45, true,
            TestName = "Тест конструктора со стойкой корректными значениями")]
        [TestCase(2, 1, 40, 25, 25, 60, true,
            TestName = "Тест конструктора со стойкой корректными максимальными значениями")]
        [TestCase(2, 1, 30, 20, 20, 40, true,
            TestName = "Тест конструктора со стойкой корректными минимальными значениями")]
        public void TestNameSet_CorrectValue_WithStand(double bottomThickness, double wallThickness, double urnHeight, double bottomRadius, double topRadius, double standHeight, bool stand)
        {
            _parametersList.Add(bottomThickness);
            _parametersList.Add(wallThickness);
            _parametersList.Add(urnHeight);
            _parametersList.Add(bottomRadius);
            _parametersList.Add(topRadius);
            _parametersList.Add(standHeight);
            var expectedBottomThickness = bottomThickness * 10;
            var expectedWallThickness = wallThickness * 10;
            var expectedUrnHeight = urnHeight * 10;
            var expectedBottomRadius = bottomRadius * 10;
            var expectedTopRadius = topRadius * 10;
            var expectedStandHeight = standHeight * 10;
            _circleParameters = new CircleParameters(_parametersList, stand);
            Assert.AreEqual(expectedBottomThickness, _circleParameters.BottomThickness);
            Assert.AreEqual(expectedWallThickness, _circleParameters.WallThickness);
            Assert.AreEqual(expectedUrnHeight, _circleParameters.UrnHeight);
            Assert.AreEqual(expectedBottomRadius, _circleParameters.RadiusBottom);
            Assert.AreEqual(expectedTopRadius, _circleParameters.RadiusTop);
            Assert.AreEqual(expectedStandHeight, _circleParameters.StandHeight);
            Assert.AreEqual(stand, _circleParameters.Stand);
        }

        [Test]
        [TestCase(2, 1, 35, 20, 20, 40, false,
            TestName = "Тест конструктора корректными значениями")]
        [TestCase(2, 1, 40, 25, 25, 60, false,
            TestName = "Тест конструктора корректными максимальными значениями")]
        [TestCase(2, 1, 30, 20, 20, 40, false,
            TestName = "Тест конструктора корректными минимальными значениями")]
        public void TestNameSet_CorrectValue(double bottomThickness, double wallThickness, double urnHeight, double bottomRadius, double topRadius, double standHeight, bool stand)
        {
            _parametersList.Add(bottomThickness);
            _parametersList.Add(wallThickness);
            _parametersList.Add(urnHeight);
            _parametersList.Add(bottomRadius);
            _parametersList.Add(topRadius);
            _parametersList.Add(standHeight);
            var expectedBottomThickness = bottomThickness * 10;
            var expectedWallThickness = wallThickness * 10;
            var expectedUrnHeight = urnHeight * 10;
            var expectedBottomRadius = bottomRadius * 10;
            var expectedTopRadius = topRadius * 10;
            var expectedStandHeight = 0;
            _circleParameters = new CircleParameters(_parametersList, stand);
            Assert.AreEqual(expectedBottomThickness, _circleParameters.BottomThickness);
            Assert.AreEqual(expectedWallThickness, _circleParameters.WallThickness);
            Assert.AreEqual(expectedUrnHeight, _circleParameters.UrnHeight);
            Assert.AreEqual(expectedBottomRadius, _circleParameters.RadiusBottom);
            Assert.AreEqual(expectedTopRadius, _circleParameters.RadiusTop);
            Assert.AreEqual(expectedStandHeight, _circleParameters.StandHeight);
            Assert.AreEqual(stand, _circleParameters.Stand);
        }

        [Test]
        [TestCase(3, 1, 35, 20, 20, 40, true,
            TestName = "Толщина дна должна быть меньше 2")]
        [TestCase(2, 2, 30, 20, 20, 40, true,
            TestName = "Толщина стенок должна быть меньше 1")]
        [TestCase(2, 1, 29, 20, 20, 40, true,
            TestName = "Высота урны должна быть меньше 40 и больше 30")]
        [TestCase(2, 1, 35, 25, 25, 70, true,
            TestName = "Высота стойки должна быть меньше (+)60 и на 10 больше высоты урны")]
        [TestCase(2, 1, 35, 20, 20, 40, true,
            TestName = "Высота стойки должна быть меньше 60 и на (+)10 больше высоты урны")]
        [TestCase(2, 1, 30, 12, 25, 40, true,
            TestName = "размеры нижнего основания должны лежать в пределах от 0,5 до 1 размера верхнего основания")]
        [TestCase(2, 1, 30, 20, 30, 40, true,
            TestName = "Радиус верхнего основания урны должен быть меньше 25")]
        [TestCase(2, 1, 30, 20, 30, 40, true,
            TestName = "Радиус нижнего основания урны должен быть меньше 25")]
        [TestCase(2, 1, 30, 12, 19, 40, false,
            TestName = "размеры нижнего основания должны лежать в пределах от 0,5 до 1,5 размера верхнего основания")]

        public void TestNameSet_InCorrectValue(double bottomThickness, double wallThickness, double urnHeight, double bottomRadius, double topRadius, double standHeight, bool stand)
        {
            _parametersList.Add(bottomThickness);
            _parametersList.Add(wallThickness);
            _parametersList.Add(urnHeight);
            _parametersList.Add(bottomRadius);
            _parametersList.Add(topRadius);
            _parametersList.Add(standHeight);
            
            Assert.Throws<ArgumentException>(
                () => { _circleParameters = new CircleParameters(_parametersList, stand); },
                "Должна быть ошибка");
        }
    }
}
