/*
 * Copyright 2019 TNG Technology Consulting GmbH
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Core.LoadTasks;
using ArchUnitNET.Fluent;

namespace ArchUnitNET.Core
{
    internal class LoadTaskRegistry
    {
        private readonly Dictionary<System.Type, Queue<ILoadTask>> _tasksTodo =
            new Dictionary<System.Type, Queue<ILoadTask>>();

        public void ExecuteTasks(List<System.Type> taskOrder)
        {
            var comparer = new TaskComparer(taskOrder);

            ICollection<System.Type> tasksTodoKeys = _tasksTodo.Keys;

            var prioritizedTodoKeys = tasksTodoKeys.OrderBy(taskType => taskType, comparer);
            prioritizedTodoKeys.ForEach(taskKey =>
            {
                if (!_tasksTodo.TryGetValue(taskKey, out var queueOfTasks))
                {
                    return;
                }

                while (queueOfTasks.Count > 0)
                {
                    var nextTask = queueOfTasks.Peek();
                    nextTask.Execute();
                    queueOfTasks.Dequeue();
                }
            });
        }

        public void Add(System.Type taskType, ILoadTask task)
        {
            if (taskType == null)
            {
                throw new ArgumentNullException(nameof(taskType));
            }

            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            var taskQueueExists = _tasksTodo.TryGetValue(taskType, out var queueOfTasksOfType);
            if (!taskQueueExists)
            {
                queueOfTasksOfType = new Queue<ILoadTask>();
            }

            queueOfTasksOfType.Enqueue(task);
            _tasksTodo[taskType] = queueOfTasksOfType;
        }

        private class TaskComparer : IComparer<System.Type>

        {
            private readonly List<System.Type> _taskOrder;

            public TaskComparer(List<System.Type> taskOrder)
            {
                _taskOrder = taskOrder;
            }

            public int Compare(System.Type x, System.Type y)
            {
                var a = _taskOrder.IndexOf(x);
                var b = _taskOrder.IndexOf(y);

                if (a > b)
                {
                    return 1;
                }

                if (a < b)
                {
                    return -1;
                }

                return 0;
            }
        }
    }
}