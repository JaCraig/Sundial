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

namespace Sundial.Core.Manager.Interfaces
{
    /// <summary>
    /// Profiler result entry interface
    /// </summary>
    public interface IResultEntry
    {
        /// <summary>
        /// Gets the value for the result.
        /// </summary>
        /// <value>The value for the result.</value>
        decimal Value { get; }
    }
}