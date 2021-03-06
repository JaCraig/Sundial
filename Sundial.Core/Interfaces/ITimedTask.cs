﻿/*
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

using System;

namespace Sundial.Core.Interfaces
{
    /// <summary>
    /// Timed task
    /// </summary>
    public interface ITimedTask : IDisposable
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Sundial.Core.Interfaces.ISeries"/> is
        /// the baseline.
        /// </summary>
        /// <value><c>true</c> if it is the baseline; otherwise, <c>false</c>.</value>
        bool Baseline { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; }

        /// <summary>
        /// Runs this instance.
        /// </summary>
        void Run();
    }
}