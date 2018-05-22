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

using Sundial.Core.Interfaces;
using System;

namespace Sundial.Core.Attributes
{
    /// <summary>
    /// Series attribute. Used to divide the tasks into groups for comparison purposes.
    /// </summary>
    /// <seealso cref="Attribute"/>
    /// <seealso cref="ISeries"/>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class SeriesAttribute : Attribute, ISeries
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeriesAttribute"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="iterations">The iterations.</param>
        /// <param name="exporters">Exporters to use.</param>
        public SeriesAttribute(string name, int iterations = 1000, params string[] exporters)
        {
            Name = name ?? "";
            Iterations = iterations;
            Exporters = exporters ?? new string[] { "Console" };
        }

        /// <summary>
        /// Gets the name of the exporter to use.
        /// </summary>
        /// <value>The name of the exporter to use.</value>
        public string[] Exporters { get; }

        /// <summary>
        /// Gets the number of iterations to run.
        /// </summary>
        /// <value>The number of iterations to run.</value>
        public int Iterations { get; }

        /// <summary>
        /// Gets the name of the series.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/>, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance;
        /// otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            var TempObj = obj as SeriesAttribute;
            return !(TempObj is null) && string.Equals(TempObj.Name, Name, StringComparison.Ordinal);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures
        /// like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}