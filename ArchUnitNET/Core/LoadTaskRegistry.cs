/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paula.ruiz@tngtech.com>
 *
 * SPDX-License-Identifier: Apache-2.0
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