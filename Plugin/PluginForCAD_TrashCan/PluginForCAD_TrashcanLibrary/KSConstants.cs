using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginForCAD_TrashcanLibrary
{
    /// <summary>
    /// Константы компас api
    /// </summary>
    public enum KSConstants
    {
        /// <summary>
        /// Для доступа к параметрам круга
        /// </summary>
        ko_CircleParam = 20,
        /// <summary>
        /// Для доступа к параметрам прямоугольника
        /// </summary>
        ko_RectangleParam = 91,
        /// <summary>
        /// Для доступа к параметрам детали
        /// </summary>
        pTop_part = -1,
        /// <summary>
        /// Для доступа к параметрам эскиза
        /// </summary>
        o3d_sketch = 5,
        /// <summary>
        /// Для доступа к параметрам плсокости ХоУ
        /// </summary>
        o3d_planeXOY = 1,
        /// <summary>
        /// Для доступа к параметрам смещенной плоскости
        /// </summary>
        o3d_planeOffset = 14,
        /// <summary>
        /// Для доступа к параметрам базовой операции по сечениям
        /// </summary>
        o3d_baseLoft = 30,
        /// <summary>
        /// Для доступа к параметрам вырезния по сечениям
        /// </summary>
        o3d_cutLoft = 32,
        /// <summary>
        /// Для доступа к параметрам вырезания выдавливанием
        /// </summary>
        o3d_cutExtrusion = 26,
        /// <summary>
        /// Для доступа к параметрам плоскости XoZ
        /// </summary>
        o3d_planeXOZ = 2,
        /// <summary>
        /// Для доступа к параметрам отрезка
        /// </summary>
        ko_LineSegParam = 11,
        /// <summary>
        /// Для доступа к параметрам осевой линии
        /// </summary>
        ko_AxisLineParam = 123,
        /// <summary>
        /// Для доступа к параметрам базовой операции вращения
        /// </summary>
        o3d_baseRotated = 27,
        /// <summary>
        /// Для доступа к параметрам базовой операции выдавливания
        /// </summary>
        o3d_baseExtrusion = 24,
        /// <summary>
        /// Для доступа к параметрам операции приклеивания выдавливанием
        /// </summary>
        o3d_bossExtrusion = 25
    }
}
