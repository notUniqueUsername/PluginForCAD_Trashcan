using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginForCAD_TrashcanLibrary
{
    public class CircleParameters
    {
        /// <summary>
        /// Толщина дна
        /// </summary>
        public double BottomThickness { get; private set; }

        /// <summary>
        /// Толщина стенок
        /// </summary>
        public double WallThickness { get; private set; }

        /// <summary>
        /// Высота урны
        /// </summary>
        public double UrnHeight { get; private set; }

        /// <summary>
        /// Радиус верхнего основания
        /// </summary>
        public double RadiusTop { get; private set; }

        /// <summary>
        /// Радиус нижнего основания
        /// </summary>
        public double RadiusBottom { get; private set; }

        /// <summary>
        /// Наличие стойки
        /// </summary>
        public bool Stand { get; private set; }

        /// <summary>
        /// Наличие пепельницы
        /// </summary>
        public bool Ashtray { get; private set; }

        /// <summary>
        /// Высота стойки
        /// </summary>
        public double StandHeight { get; private set; }


        /// <summary>
        /// Проверка листа параметров
        /// </summary>
        /// <param name="parameters"></param>
        private void ValidateParamList(List<double> parameters)
        {
            if (Stand)
            {
                if (parameters.Count != 6)
                {
                    throw new ArgumentException("В листе параметров должно быть 6 значений");
                }
            }
            else
            {
                if (parameters.Count != 5)
                {
                    throw new ArgumentException("В листе параметров должно быть 5 значений");
                }
            }
            foreach (var parameter in parameters)
            {
                if (double.IsInfinity(parameter) || double.IsNaN(parameter))
                {
                    throw new ArgumentException("Параметры не должны быть NaN или infinity");
                }
            }
        }

        /// <summary>
        /// Проверка угла наклона
        /// </summary>
        /// <param name="top"></param>
        /// <param name="bot"></param>
        /// <param name="height"></param>
        private void ValidateAngle(double top, double bot, double height)
        {
            var L = Math.Sqrt(Math.Pow(height, 2) + Math.Pow(bot - top, 2));
            if (bot != top)
            {
                var tgAngle = height / Math.Abs(bot - top);
                double Angle = Math.Atan(tgAngle) * 180 / Math.PI;
                if (Angle < 60)
                {
                    throw new ArgumentException("Наклон превышает 60 градусов");
                }
            }

        }


        /// <summary>
        /// Монструозный конструктор
        /// </summary>
        /// <param name="parameters">
        /// Лист параметров
        /// </param>
        /// <param name="stand">Наличие стойки</param>
        public CircleParameters(List<double> parameters, bool stand, bool ashtray)
        {
            Stand = stand;
            Ashtray = ashtray;
            ValidateParamList(parameters);
            if (parameters[0] > 0 && parameters[0] <= 2)
            {
                BottomThickness = parameters[0] * 10;
            }
            else
            {
                throw new ArgumentException("Толщина дна должна быть меньше 2");
            }

            if (parameters[1] > 0 && parameters[1] <= 1)
            {
                WallThickness = parameters[1] * 10;
            }
            else
            {
                throw new ArgumentException("Толщина стенок должна быть меньше 1");
            }

            if (parameters[2] > 0 && parameters[2] <= 40 && parameters[2] >= 30)
            {
                UrnHeight = parameters[2] * 10;
            }
            else
            {
                throw new ArgumentException("Высота урны должна быть меньше 40 и больше 30");
            }

            if (Stand)
            {
                if (parameters[parameters.Count - 1] > 0 && parameters[parameters.Count - 1] <= 60 && (parameters[parameters.Count - 1] - UrnHeight / 10) >= 10)
                {
                    StandHeight = parameters[parameters.Count - 1] * 10;
                }
                else
                {
                    throw new ArgumentException("Высота стойки должна быть меньше 60 и на 10 больше высоты урны");
                }
            }


            if (parameters[3] > 0 && parameters[3] <= 25 && parameters[3] >= 20)
            {
                RadiusTop = parameters[3] * 10;
            }
            else
            {
                throw new ArgumentException("Радиус верхнего основания урны должен быть меньше 25 и больше 20");
            }

            if (parameters[4] > 0 && parameters[4] <= 25 && parameters[4] >= 20)
            {
                RadiusBottom = parameters[4] * 10;
            }
            else
            {
                throw new ArgumentException("Радиус нижнего основания урны должен быть меньше 25 и больше 20");
            }

            if (Stand)
            {
                if (!(RadiusTop >= (0.5 * RadiusBottom)
                    && RadiusTop <= RadiusBottom))
                {
                    throw new ArgumentException("" +
                        "размеры нижнего основания должны лежать в пределах от 0.5 до 1 размера верхнего основания");
                }
            }
            else
            {
                if (!(RadiusBottom >= (0.5 * RadiusTop)
                    && RadiusBottom <= (1.5 * RadiusTop)))
                {
                    throw new ArgumentException("" +
                        "размеры нижнего основания должны лежать в пределах от 0.5 до 1.5 размера верхнего основания");
                }
            }
            ValidateAngle(RadiusTop, RadiusBottom, UrnHeight);

        }
    }
}
