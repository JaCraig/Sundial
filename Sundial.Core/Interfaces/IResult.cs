/*
Copyright 2017 James Craig

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using BigBook;
using Sundial.Core.Manager.Interfaces;

namespace Sundial.Core.Interfaces
{
    /// <summary>
    /// Result interface
    /// </summary>
    public interface IResult
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; }

        /// <summary>
        /// Gets the task.
        /// </summary>
        /// <value>The task.</value>
        ITimedTask Task { get; }

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <value>The values.</value>
        ListMapping<string, IResultEntry> Values { get; }

        /// <summary>
        /// Averages the values of the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The average value</returns>
        decimal Average(string type);

        /// <summary>
        /// Percentiles the specified name.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="percentage">The percentage.</param>
        /// <returns>The result entry</returns>
        IResultEntry Percentile(string type, decimal percentage);
    }
}