using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kompas6API5;

namespace PluginForCAD_TrashcanLibrary
{
    /// <summary>
    /// Класс для работы с окном компаса
    /// </summary>
    public class KompasConnector
    {
        /// <summary>
        /// Экзкмпляр компаса.
        /// </summary>
        public KompasObject KompasObject { get; set; }

        /// <summary>
        /// Запуск компаса.
        /// </summary>
        public void StartKompas()
        {
            if (KompasObject == null)
            {
                var type = Type.GetTypeFromProgID("KOMPAS.Application.5");
                KompasObject = (KompasObject)Activator.CreateInstance(type);
            }

            if (KompasObject != null)
            {
                KompasObject.Visible = true;
                KompasObject.ActivateControllerAPI();
            }
        }

        /// <summary>
        /// Закрытие компаса.
        /// </summary>
        public void CloseKompas()
        {
            try
            {
                KompasObject.Quit();
                KompasObject = null;
            }
            catch
            {
                KompasObject = null;
            }

        }

    }
}
