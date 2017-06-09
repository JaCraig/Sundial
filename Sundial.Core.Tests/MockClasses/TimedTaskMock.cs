using Sundial.Core.Interfaces;

namespace Sundial.Core.Tests.MockClasses
{
    public class TimedTaskMock : ITimedTask
    {
        public bool Baseline => true;

        public string Name => nameof(TimedTaskMock);

        public void Dispose()
        {
        }

        public void Run()
        {
        }
    }
}