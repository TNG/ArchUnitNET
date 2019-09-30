using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Syntax;

namespace ArchUnitNET.Fluent
{
    public partial class ArchRuleCreator<TRuleType>
    {
        private class ObjectFilterManager<T> : IHasDescription where T : ICanBeAnalyzed
        {
            private const string NotSet = "not set";
            private readonly List<ObjectFilterElement<T>> _objectFilterElements;
            private readonly ObjectProvider<T> _objectProvider;

            public ObjectFilterManager(ObjectProvider<T> objectProvider)
            {
                _objectProvider = objectProvider;
                _objectFilterElements = new List<ObjectFilterElement<T>>
                {
                    new ObjectFilterElement<T>(LogicalConjunctionDefinition.ForwardSecondValue,
                        new ObjectFilter<T>(t => true, NotSet))
                };
            }

            public string Description => _objectFilterElements.First().Description == NotSet
                ? _objectProvider.Description
                : _objectProvider.Description + " that" + _objectFilterElements.Aggregate("",
                      (current, objectFilterElement) => current + " " + objectFilterElement.Description);

            public IEnumerable<T> GetFilteredObjects(Architecture architecture)
            {
                return _objectProvider.GetObjects(architecture).Where(obj => _objectFilterElements.Aggregate(true,
                    (currentResult, objectFilterElement) =>
                        objectFilterElement.CheckFilter(currentResult, obj, architecture)));
            }

            public void AddFilter(IObjectFilter<T> objectFilter)
            {
                _objectFilterElements.Last().SetFilter(objectFilter);
            }

            public void AddReason(string reason)
            {
                _objectFilterElements.Last().AddReason(reason);
            }

            public void SetNextLogicalConjunction(LogicalConjunction logicalConjunction)
            {
                _objectFilterElements.Add(new ObjectFilterElement<T>(logicalConjunction));
            }

            public override string ToString()
            {
                return Description;
            }

#pragma warning disable 693
        private class ObjectFilterElement<T> : IHasDescription where T : ICanBeAnalyzed
        {
            private readonly LogicalConjunction _logicalConjunction;
            private IObjectFilter<T> _objectFilter;
            private string _reason;

            public ObjectFilterElement(LogicalConjunction logicalConjunction, IObjectFilter<T> objectFilter = null)
            {
                _objectFilter = objectFilter;
                _logicalConjunction = logicalConjunction;
                _reason = "";
            }

            public string Description => _objectFilter == null
                ? _logicalConjunction.Description
                : (_logicalConjunction.Description + " " + _objectFilter.Description + " " + _reason).Trim();

            public void AddReason(string reason)
            {
                if (_objectFilter == null)
                {
                    throw new InvalidOperationException(
                        "Can't add a reason to an ObjectFilterElement before the filter is set.");
                }

                if (_reason != "")
                {
                    throw new InvalidOperationException(
                        "Can't add a reason to an ObjectFilterElement which already has a reason.");
                }

                _reason = "because " + reason;
            }

            public void SetFilter(IObjectFilter<T> objectFilter)
            {
                _objectFilter = objectFilter;
            }

            public bool CheckFilter(bool currentResult, T obj, Architecture architecture)
            {
                if (_objectFilter == null)
                {
                    throw new InvalidOperationException(
                        "Can't Evaluate an ObjectFilterElement before the filter is set.");
                }

                return _logicalConjunction.Evaluate(currentResult, _objectFilter.CheckFilter(obj, architecture));
            }

            public override string ToString()
            {
                return Description;
            }
        }
    }
    }
}