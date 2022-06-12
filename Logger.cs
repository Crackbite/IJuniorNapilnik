using System;
using System.IO;

namespace IJuniorNapilnik
{
    internal interface ILogger
    {
        void WriteError(string message);
    }

    internal static class Logger
    {
        public static void TestSystem()
        {
            var pathfinder = new Pathfinder(new FileLogWritter());
            pathfinder.Find("Пишет лог в файл");

            pathfinder = new Pathfinder(new ConsoleLogWritter());
            pathfinder.Find("Пишет лог в консоль");

            pathfinder = new Pathfinder(new SecureLogWritter(new FileLogWritter()));
            pathfinder.Find("Пишет лог в файл по пятницам");

            pathfinder = new Pathfinder(new SecureLogWritter(new ConsoleLogWritter()));
            pathfinder.Find("Пишет лог в консоль по пятницам");

            pathfinder = new Pathfinder(new ConsoleLogWritter(), new SecureLogWritter(new FileLogWritter()));
            pathfinder.Find("Пишет лог в консоль а по пятницам ещё и в файл");
        }
    }

    internal class SecureLogWritter : ILogger
    {
        private readonly ILogger _logger;

        public SecureLogWritter(ILogger logger)
        {
            _logger = logger;
        }

        public void WriteError(string message)
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            {
                _logger.WriteError(message);
            }
        }
    }

    internal class ConsoleLogWritter : ILogger
    {
        public virtual void WriteError(string message)
        {
            Console.WriteLine(message);
        }
    }

    internal class FileLogWritter : ILogger
    {
        private readonly string _fileName;

        public FileLogWritter(string fileName = "log.txt")
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            _fileName = fileName;
        }

        public virtual void WriteError(string message)
        {
            File.WriteAllText(_fileName, message);
        }
    }

    internal class Pathfinder
    {
        private readonly ILogger[] _logger;

        public Pathfinder(params ILogger[] logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Find(string message)
        {
            foreach (ILogger logger in _logger)
            {
                logger.WriteError(message);
            }
        }
    }
}