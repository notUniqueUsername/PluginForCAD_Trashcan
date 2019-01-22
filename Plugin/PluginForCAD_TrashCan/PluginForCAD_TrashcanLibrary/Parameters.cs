using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginForCAD_TrashcanLibrary
{
    public class Parameters
    {
        public double BottomThickness { get; private set; }

        public double WallThickness { get; private set; }

        public double UrnHeight { get; private set; }

        public double WidthBottom { get; private set; }

        public double WidthTop { get; private set; }

        public double LengthBottom { get; private set; }

        public double LengthTop { get; private set; }

        public double RadiusTop { get; private set; }

        public double RadiusBottom { get; private set; }

        public bool Stand { get; private set; }

        public double StandHeight { get; private set; }

        /// <summary>
        /// Монструозный конструктор
        /// </summary>
        /// <param name="parameters">
        /// Лист параметров
        /// </param>
        /// <param name="urnForm">Форма урны</param>
        /// <param name="stand">Наличие стойки</param>
        public Parameters(List<double> parameters, UrnForms urnForm, bool stand)
        {
            Stand = stand;

            if (parameters[0] > 0 && parameters[0] <= 2)
            {
                BottomThickness = parameters[0];
            }
            else
            {
                throw new ArgumentException("Толщина дна должна быть меньше 2");
            }

            if (parameters[1] > 0 && parameters[1] <= 1)
            {
                WallThickness = parameters[1];
            }
            else
            {
                throw new ArgumentException("Толщина стенок должна быть меньше 1");
            }

            if (parameters[2] > 0 && parameters[2] <= 40)
            {
                UrnHeight = parameters[2];
            }
            else
            {
                throw new ArgumentException("Высота урны должна быть меньше 40");
            }

            if (Stand)
            {
                if (parameters[parameters.Count -1] > 0 && parameters[parameters.Count - 1] <= 60)
                {
                    StandHeight = parameters[parameters.Count -1];
                }
                else
                {
                    throw new ArgumentException("Высота стойки должна быть меньше 60");
                }
            }


            switch (urnForm)
            {
                case UrnForms.Circle:

                    if (parameters[3] > 0 && parameters[3] <= 25)
                    {
                        RadiusTop = parameters[3];
                    }
                    else
                    {
                        throw new ArgumentException("Радиус верхнего основания урны должен быть меньше 25");
                    }

                    if (parameters[4] > 0 && parameters[4] <= 25)
                    {
                        RadiusBottom = parameters[4];
                    }
                    else
                    {
                        throw new ArgumentException("Радиус нижнего основания урны должен быть меньше 25");
                    }

                    if (Stand)
                    {
                        if (!(RadiusBottom >= (0.5 * RadiusTop) 
                            && RadiusBottom <= RadiusTop))
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
                    break;
                case UrnForms.Rectangle:
                    if (parameters[3] > 0 && parameters[3] <= 50)
                    {
                        WidthBottom = parameters[3];
                    }
                    else
                    {
                        throw new ArgumentException("Ширина нижнего основания урны должна быть меньше 50");
                    }

                    if (parameters[4] > 0 && parameters[4] <= 50)
                    {
                        WidthTop = parameters[4];
                    }
                    else
                    {
                        throw new ArgumentException("Ширина верхнего основания урны должна быть меньше 50");
                    }

                    if (parameters[5] > 0 && parameters[5] <= 50)
                    {
                        LengthBottom = parameters[5];
                    }
                    else
                    {
                        throw new ArgumentException("Длина нижнего основания урны должна быть меньше 50");
                    }

                    if (parameters[6] > 0 && parameters[6] <= 50)
                    {
                        LengthTop = parameters[6];
                    }
                    else
                    {
                        throw new ArgumentException("Длина верхнего основания урны должна быть меньше 50");
                    }

                    if (Stand)
                    {
                        if (!(LengthBottom >= (0.5 * LengthTop)
                            && LengthBottom <= LengthTop
                            && WidthBottom <= WidthTop
                            && WidthBottom >= (0.5 * LengthTop)))
                        {
                            throw new ArgumentException("" +
                                "Высота и ширина нижнего основания должны лежать в пределах от 0.5 до 1 размера верхнего основания");
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

                    break;
                default:
                    break;
            }
        }
    }
}
