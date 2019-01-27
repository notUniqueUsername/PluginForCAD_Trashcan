using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginForCAD_TrashcanLibrary
{
    /// <summary>
    /// Класс с параметрами
    /// </summary>
    public class RectangleParameters
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
        /// Ширина нижнего основания
        /// </summary>
        public double WidthBottom { get; private set; }

        /// <summary>
        /// Ширина вернего основания
        /// </summary>
        public double WidthTop { get; private set; }

        /// <summary>
        /// Длинна нижнего основания
        /// </summary>
        public double LengthBottom { get; private set; }

        /// <summary>
        /// Длинна верхнего основания
        /// </summary>
        public double LengthTop { get; private set; }

        /// <summary>
        /// Наличие стойки
        /// </summary>
        public bool Stand { get; private set; }

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
                if (parameters.Count != 8)
                {
                    throw new ArgumentException("В листе параметров должно быть 8 значений");
                }
            }
            else
            {
                if (parameters.Count != 7)
                {
                    throw new ArgumentException("В листе параметров должно быть 7 значений");
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
                var tgAngle = height / Math.Abs(bot-top);
                double Angle = Math.Atan(tgAngle)*180/Math.PI;
                if (Angle<60 )
                {
                    throw new ArgumentException("Наклон превышает 60 градусов");
                }
            }

        }


        /// <summary>
        /// Монструозный конструктор
        /// </summary>
        /// <param name="parameters">
        /// Лист параметров 0-толщина дна,1-тольщина стенок,2-высота урны,3-ширина нижнего основания,
        /// 4-ширина верхнего основания,5-длина нижнего основания,6-длина верхнего основания
        /// </param>
        /// <param name="stand">Наличие стойки</param>
        public RectangleParameters(List<double> parameters, bool stand)
        {
            Stand = stand;

            ValidateParamList(parameters);

            if (parameters[0] > 0 && parameters[0] <= 2)
            {
                BottomThickness = parameters[0]*10;
            }
            else
            {
                throw new ArgumentException("Толщина дна должна быть меньше 2");
            }

            if (parameters[1] > 0 && parameters[1] <= 1)
            {
                WallThickness = parameters[1]*10;
            }
            else
            {
                throw new ArgumentException("Толщина стенок должна быть меньше 1");
            }

            if (parameters[2] > 0 && parameters[2] <= 40 && parameters[2] >= 30)
            {
                UrnHeight = parameters[2]*10;
            }
            else
            {
                throw new ArgumentException("Высота урны должна быть меньше 40 и больше 30");
            }

            if (Stand)
            {
                if (parameters[parameters.Count -1] > 0 && parameters[parameters.Count - 1] <= 60 && (parameters[parameters.Count - 1] - UrnHeight/10) >= 10 )
                {
                    StandHeight = parameters[parameters.Count -1]*10;
                }
                else
                {
                    throw new ArgumentException("Высота стойки должна быть меньше 60 и на 10 больше высоты урны");
                }
            }


            if (parameters[3] > 0 && parameters[3] <= 50 && parameters[3] >= 20)
            {
                WidthBottom = parameters[3] * 10;
            }
            else
            {
                throw new ArgumentException("Ширина нижнего основания урны должна быть меньше 50 и больше 20");
            }

            if (parameters[4] > 0 && parameters[4] <= 50 && parameters[4] >= 20)
            {
                WidthTop = parameters[4] * 10;
            }
            else
            {
                throw new ArgumentException("Ширина верхнего основания урны должна быть меньше 50 и больше 20");
            }

            if (parameters[5] > 0 && parameters[5] <= 50 && parameters[5] >= 20)
            {
                LengthBottom = parameters[5] * 10;
            }
            else
            {
                throw new ArgumentException("Длина нижнего основания урны должна быть меньше 50 и больше 20");
            }

            if (parameters[6] > 0 && parameters[6] <= 50 && parameters[6] >= 20)
            {
                LengthTop = parameters[6] * 10;
            }
            else
            {
                throw new ArgumentException("Длина верхнего основания урны должна быть меньше 50 и больше 20");
            }

            if (Stand)
            {
                if (!(LengthTop >= (0.5 * LengthBottom)
                    && LengthTop <= LengthBottom)
                    || !(WidthTop <= WidthBottom
                    && LengthTop >= (0.5 * WidthBottom)))
                {
                    throw new ArgumentException("" +
                        "Высота и ширина нижнего основания должны лежать в пределах от 0.5 до 1 размера верхнего основания");
                }
                if (LengthBottom != WidthBottom || LengthTop != WidthTop)
                {
                    throw new ArgumentException("" +
                        "Если выбрана стойка, верхнее и нижнее основания должны быть квадратами");
                }
            }
            else
            {
                if (!(LengthBottom >= (0.5 * LengthTop)
                    && LengthBottom <= (1.5 * LengthTop)
                    && WidthBottom <= (1.5 * WidthTop)
                    && WidthBottom >= (0.5 * LengthTop)))
                {
                    throw new ArgumentException("" +
                        "Высота и ширина нижнего основания должны лежать в пределах от 0.5 до 1.5 размера верхнего основания");
                }
            }


            ValidateAngle(LengthTop, LengthBottom, UrnHeight);
            ValidateAngle(WidthTop, WidthBottom, UrnHeight);

        }
    }
}
