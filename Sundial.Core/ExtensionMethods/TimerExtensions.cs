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

using System;
using System.ComponentModel;

namespace Sundial.Core.ExtensionMethods
{
    /// <summary>
    /// Holds timing/profiling related extensions
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class TimerExtensions
    {
        /// <summary>
        /// Times an action and places
        /// </summary>
        /// <param name="actionToTime">Action to time</param>
        /// <param name="functionName">Name to associate with the action</param>
        public static void Time(this Action actionToTime, string functionName = "")
        {
            if (actionToTime == null)
                return;
            using (new Profiler(functionName))
                actionToTime();
        }

        /// <summary>
        /// Times an action and places
        /// </summary>
        /// <typeparam name="T">Action input type</typeparam>
        /// <param name="actionToTime">Action to time</param>
        /// <param name="object1">Object 1</param>
        /// <param name="functionName">Name to associate with the action</param>
        public static void Time<T>(this Action<T> actionToTime, T object1, string functionName = "")
        {
            if (actionToTime == null)
                return;
            using (new Profiler(functionName))
                actionToTime(object1);
        }

        /// <summary>
        /// Times an action and places
        /// </summary>
        /// <typeparam name="T1">Action input type 1</typeparam>
        /// <typeparam name="T2">Action input type 2</typeparam>
        /// <param name="actionToTime">Action to time</param>
        /// <param name="object1">Object 1</param>
        /// <param name="object2">Object 2</param>
        /// <param name="functionName">Name to associate with the action</param>
        public static void Time<T1, T2>(this Action<T1, T2> actionToTime, T1 object1, T2 object2, string functionName = "")
        {
            if (actionToTime == null)
                return;
            using (new Profiler(functionName))
                actionToTime(object1, object2);
        }

        /// <summary>
        /// Times an action and places
        /// </summary>
        /// <typeparam name="T1">Action input type 1</typeparam>
        /// <typeparam name="T2">Action input type 2</typeparam>
        /// <typeparam name="T3">Action input type 3</typeparam>
        /// <param name="actionToTime">Action to time</param>
        /// <param name="object1">Object 1</param>
        /// <param name="object2">Object 2</param>
        /// <param name="object3">Object 3</param>
        /// <param name="functionName">Name to associate with the action</param>
        public static void Time<T1, T2, T3>(this Action<T1, T2, T3> actionToTime, T1 object1, T2 object2, T3 object3, string functionName = "")
        {
            if (actionToTime == null)
                return;
            using (new Profiler(functionName))
                actionToTime(object1, object2, object3);
        }

        /// <summary>
        /// Times an action and places
        /// </summary>
        /// <param name="funcToTime">Action to time</param>
        /// <param name="functionName">Name to associate with the action</param>
        /// <typeparam name="R">Type of the value to return</typeparam>
        /// <returns>The value returned by the Func</returns>
        public static R Time<R>(this Func<R> funcToTime, string functionName = "")
        {
            if (funcToTime == null)
                return default(R);
            using (new Profiler(functionName))
                return funcToTime();
        }

        /// <summary>
        /// Times an action and places
        /// </summary>
        /// <typeparam name="T1">Object type 1</typeparam>
        /// <typeparam name="R">Type of the value to return</typeparam>
        /// <param name="funcToTime">Action to time</param>
        /// <param name="object1">Object 1</param>
        /// <param name="functionName">Name to associate with the action</param>
        /// <returns>The value returned by the Func</returns>
        public static R Time<T1, R>(this Func<T1, R> funcToTime, T1 object1, string functionName = "")
        {
            if (funcToTime == null)
                return default(R);
            using (new Profiler(functionName))
                return funcToTime(object1);
        }

        /// <summary>
        /// Times an action and places
        /// </summary>
        /// <typeparam name="T1">Object type 1</typeparam>
        /// <typeparam name="T2">Object type 2</typeparam>
        /// <typeparam name="R">Type of the value to return</typeparam>
        /// <param name="funcToTime">Action to time</param>
        /// <param name="object1">Object 1</param>
        /// <param name="object2">Object 2</param>
        /// <param name="functionName">Name to associate with the action</param>
        /// <returns>The value returned by the Func</returns>
        public static R Time<T1, T2, R>(this Func<T1, T2, R> funcToTime, T1 object1, T2 object2, string functionName = "")
        {
            if (funcToTime == null)
                return default(R);
            using (new Profiler(functionName))
                return funcToTime(object1, object2);
        }

        /// <summary>
        /// Times an action and places
        /// </summary>
        /// <typeparam name="T1">Object type 1</typeparam>
        /// <typeparam name="T2">Object type 2</typeparam>
        /// <typeparam name="T3">Object type 3</typeparam>
        /// <typeparam name="R">Type of the value to return</typeparam>
        /// <param name="funcToTime">Action to time</param>
        /// <param name="object1">Object 1</param>
        /// <param name="object2">Object 2</param>
        /// <param name="object3">Object 3</param>
        /// <param name="functionName">Name to associate with the action</param>
        /// <returns>The value returned by the Func</returns>
        public static R Time<T1, T2, T3, R>(this Func<T1, T2, T3, R> funcToTime, T1 object1, T2 object2, T3 object3, string functionName = "")
        {
            if (funcToTime == null)
                return default(R);
            using (new Profiler(functionName))
                return funcToTime(object1, object2, object3);
        }
    }
}