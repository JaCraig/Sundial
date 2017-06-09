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

using Sundial.Core.Analysis.Enums;

namespace Sundial.Core.Analysis
{
    /// <summary>
    /// Finding data holder
    /// </summary>
    public class Finding
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Finding"/> class.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="type">The type.</param>
        public Finding(string description, FindingType type = FindingType.Info)
        {
            Description = description ?? "";
            Type = type;
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public FindingType Type { get; }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return Description;
        }
    }
}