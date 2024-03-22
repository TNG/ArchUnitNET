//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//
// 	SPDX-License-Identifier: Apache-2.0
//

using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Slices
{
    public class SliceRuleCreator : ICanBeEvaluated
    {
        private Func<
            IEnumerable<Slice>,
            ICanBeEvaluated,
            Architecture,
            IEnumerable<EvaluationResult>
        > _evaluationFunc;
        private SliceAssignment _sliceAssignment;

        public SliceRuleCreator()
        {
            Description = "Slices";
        }

        public string Description { get; private set; }

        public bool HasNoViolations(Architecture architecture)
        {
            return Evaluate(architecture).All(result => result.Passed);
        }

        public IEnumerable<EvaluationResult> Evaluate(Architecture architecture)
        {
            return _evaluationFunc(GetSlices(architecture), this, architecture);
        }

        public void SetSliceAssignment(SliceAssignment sliceAssignment)
        {
            _sliceAssignment = sliceAssignment;
            AddToDescription(sliceAssignment.Description);
        }

        public void SetEvaluationFunction(
            Func<
                IEnumerable<Slice>,
                ICanBeEvaluated,
                Architecture,
                IEnumerable<EvaluationResult>
            > evaluationFunc
        )
        {
            _evaluationFunc = evaluationFunc;
        }

        public void AddToDescription(string description)
        {
            Description += " " + description.Trim();
        }

        public IEnumerable<Slice> GetSlices(Architecture architecture)
        {
            if (_sliceAssignment == null)
            {
                throw new InvalidOperationException(
                    "The Slice Assignment has to be set before GetSlices() can be called."
                );
            }

            return _sliceAssignment
                .Apply(architecture.Types)
                .Where(slice => !slice.Identifier.Ignored);
        }
    }
}
