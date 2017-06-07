using Sundial.Core.Attributes;
using Sundial.Core.Interfaces;

namespace TestApp.Tasks
{
    [Series("Series1")]
    public class Task2 : ITimedTask
    {
        public bool Baseline => true;

        public string Name => "Task 2";

        public void Dispose()
        {
        }

        public void Run()
        {
            for (int x = 0; x < 10000; ++x)
            {
            }
        }
    }
}