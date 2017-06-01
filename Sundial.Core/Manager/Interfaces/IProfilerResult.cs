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
using System.Collections.Generic;

namespace Sundial.Core.Manager.Interfaces
{
    /// <summary>
    /// Profiler results interface
    /// </summary>
    public interface IProfilerResult
    {
        /// <summary>
        /// Any child results (Key = Name/Identifier, Value = IResult object)
        /// </summary>
        IDictionary<string, IProfilerResult> Children { get; }

        /// <summary>
        /// Gets the entries.
        /// </summary>
        /// <value>The entries.</value>
        ListMapping<string, IResultEntry> Entries { get; }

        /// <summary>
        /// Where the profiler was started at
        /// </summary>
        string Function { get; }

        /// <summary>
        /// Gets the value at a specific percentile
        /// </summary>
        /// <param name="percentage">The percentage.</param>
        /// <param name="type">The type.</param>
        /// <returns>The value at a specific percentile</returns>
        IResultEntry Percentile(decimal percentage, string type);
    }
}