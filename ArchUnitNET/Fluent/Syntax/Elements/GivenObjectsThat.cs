using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;
using static ArchUnitNET.Domain.Visibility;
using static ArchUnitNET.Fluent.Syntax.ActivatorHandler;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public abstract class GivenObjectsThat<TGivenRuleTypeConjunction, TRuleType> : SyntaxElement<TRuleType>,
        IObjectsThat<TGivenRuleTypeConjunction> where TRuleType : ICanBeAnalyzed
    {
        protected GivenObjectsThat(ArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }

        public TGivenRuleTypeConjunction Are(ICanBeAnalyzed obj)
        {
            _ruleCreator.AddObjectFilter(o => o.Equals(obj));
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DependOn(string pattern)
        {
            _ruleCreator.AddObjectFilter(obj => obj.DependsOn(pattern));
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveName(string name)
        {
            _ruleCreator.AddObjectFilter(obj => obj.Name.Equals(name));
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveFullName(string fullname)
        {
            _ruleCreator.AddObjectFilter(obj => obj.FullName.Equals(fullname));
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveNameStartingWith(string pattern)
        {
            _ruleCreator.AddObjectFilter(obj => obj.NameStartsWith(pattern));
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveNameEndingWith(string pattern)
        {
            _ruleCreator.AddObjectFilter(obj => obj.NameEndsWith(pattern));
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveNameContaining(string pattern)
        {
            _ruleCreator.AddObjectFilter(obj => obj.NameContains(pattern));
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction ArePrivate()
        {
            _ruleCreator.AddObjectFilter(obj => obj.Visibility == Private);
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction ArePublic()
        {
            _ruleCreator.AddObjectFilter(obj => obj.Visibility == Public);
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreProtected()
        {
            _ruleCreator.AddObjectFilter(obj => obj.Visibility == Protected);
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreInternal()
        {
            _ruleCreator.AddObjectFilter(obj => obj.Visibility == Internal);
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreProtectedInternal()
        {
            _ruleCreator.AddObjectFilter(obj => obj.Visibility == ProtectedInternal);
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction ArePrivateProtected()
        {
            _ruleCreator.AddObjectFilter(obj => obj.Visibility == PrivateProtected);
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TGivenRuleTypeConjunction AreNot(ICanBeAnalyzed obj)
        {
            _ruleCreator.AddObjectFilter(o => !o.Equals(obj));
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotDependOn(string pattern)
        {
            _ruleCreator.AddObjectFilter(obj => !obj.DependsOn(pattern));
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveName(string name)
        {
            _ruleCreator.AddObjectFilter(obj => !obj.Name.Equals(name));
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveFullName(string fullname)
        {
            _ruleCreator.AddObjectFilter(obj => !obj.FullName.Equals(fullname));
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveNameStartingWith(string pattern)
        {
            _ruleCreator.AddObjectFilter(obj => !obj.NameStartsWith(pattern));
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveNameEndingWith(string pattern)
        {
            _ruleCreator.AddObjectFilter(obj => !obj.NameEndsWith(pattern));
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveNameContaining(string pattern)
        {
            _ruleCreator.AddObjectFilter(obj => !obj.NameContains(pattern));
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotPrivate()
        {
            _ruleCreator.AddObjectFilter(obj => obj.Visibility != Private);
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotPublic()
        {
            _ruleCreator.AddObjectFilter(obj => obj.Visibility != Public);
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotProtected()
        {
            _ruleCreator.AddObjectFilter(obj => obj.Visibility != Protected);
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotInternal()
        {
            _ruleCreator.AddObjectFilter(obj => obj.Visibility != Internal);
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotProtectedInternal()
        {
            _ruleCreator.AddObjectFilter(obj => obj.Visibility != ProtectedInternal);
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotPrivateProtected()
        {
            _ruleCreator.AddObjectFilter(obj => obj.Visibility != PrivateProtected);
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }
    }
}