using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Environment = System.Environment;
using Kompas6API5;
using NUnit.Framework;
using PluginForCAD_TrashcanLibrary;

namespace PluginForCAD_TrashCanUnitTests
{
    class StresTest
    {
        private KompasObject _kompas;
        private StreamWriter _writer;
        private PerformanceCounter _ramCounter;
        private PerformanceCounter _cpuCounter;

        [SetUp]
        public void Test()
        {
            _writer = new StreamWriter(@"C:\Users\Valeriy\Desktop\StressTest.txt");
        }

        [Test]
        [TestCase(Ignore = "Нужен не так часто")]
        public void Start()
        {
            StartKompas();
            var paramList = new List<double>();
            paramList.Add(2);
            paramList.Add(1);
            paramList.Add(30);
            paramList.Add(20);
            paramList.Add(20);
            paramList.Add(40);
            var builder = new CircleUrnBuilder(_kompas);
            var parameters = new CircleParameters(paramList, true,true);//////////////////////////////////
            var count = 1000;

            for (int i = 0; i < count; i++)
            {
                var processes = Process.GetProcessesByName("KOMPAS");
                var process = processes.First();

                if (i == 0)
                {
                    _ramCounter = new PerformanceCounter("Process", "Working Set", process.ProcessName);
                    _cpuCounter = new PerformanceCounter("Process", "% Processor Time", process.ProcessName);
                }

                _cpuCounter.NextValue();

                builder.Build(parameters);

                var ram = _ramCounter.NextValue();
                var cpu = _cpuCounter.NextValue();

                _writer.Write($"{i}. ");
                _writer.Write($"RAM: {Math.Round(ram / 1024 / 1024)} MB");
                _writer.Write($"\tCPU: {cpu} %");
                _writer.Write(Environment.NewLine);
                _writer.Flush();
            }
        }

        public void StartKompas()
        {
            if (_kompas == null)
            {
                var type = Type.GetTypeFromProgID("KOMPAS.Application.5");
                _kompas = (KompasObject)Activator.CreateInstance(type);
                _kompas.Visible = true;
                _kompas.ActivateControllerAPI();
            }
        }
    }
}
