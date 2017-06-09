using System;

namespace Sundial.Core.Manager.Interfaces
{
    /// <summary>
    /// Measurement interface
    /// </summary>
    public interface IMeasurement : IDisposable
    {
        /// <summary>
        /// Gets the measurement type.
        /// </summary>
        /// <value>The measurement type.</value>
        string Type { get; }

        /// <summary>
        /// Starts profiling
        /// </summary>
        /// <returns>Starts profiling</returns>
        IMeasurement StartProfiling();

        /// <summary>
        /// Stops profiling and returns the information captured
        /// </summary>
        /// <param name="discardResults">Determines if the results are kept or discarded</param>
        /// <returns>The results from the profiling</returns>
        IResultEntry StopProfiling(bool discardResults);
    }
}