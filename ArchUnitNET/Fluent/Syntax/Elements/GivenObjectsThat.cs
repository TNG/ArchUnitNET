using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;
using static ArchUnitNET.Domain.Visibility;
using static ArchUnitNET.Fluent.Syntax.ActivatorHandler;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public abstract class GivenObjectsThat<TGivenRuleTypeConjunction, TRuleType> : SyntaxElement<TRuleType>,
        IObjectsThat<TGivenRuleTypeConjunction> where TRuleType : ICanBeAnalyzed
    {
        //TODO create ObjectFilterDefinition Class

        protected GivenObjectsThat(ArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }

        public TGivenRuleTypeConjunction Are(ICanBeAnalyzed obj)
        {
            _ruleCreator.AddObjectFilter(o => o.Equals(obj), "are \"" + obj.FullName + "\"");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DependOn(string pattern)
        {
            _ruleCreator.AddObjectFilter(obj => obj.DependsOn(pattern), "depend on \"" + pattern + "\"");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveName(string name)
        {
            _ruleCreator.AddObjectFilter(obj => obj.Name.Equals(name), "have name \"" + name + "\"");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveFullName(string fullname)
        {
            _ruleCreator.AddObjectFilter(obj => obj.FullName.Equals(fullname), "have full name \"" + fullname + "\"");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveNameStartingWith(string pattern)
        {
            _ruleCreator.AddObjectFilter(obj => obj.NameStartsWith(pattern),
                "have name starting with \"" + pattern + "\"");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveNameEndingWith(string pattern)
        {
            _ruleCreator.AddObjectFilter(obj => obj.NameEndsWith(pattern), "have name ending with \"" + pattern + "\"");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveNameContaining(string pattern)
        {
            _ruleCreator.AddObjectFilter(obj => obj.NameContains(pattern), "have name containing \"" + pattern + "\"");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction ArePrivate()
        {
            _ruleCreator.AddObjectFilter(obj => obj.Visibility == Private, "are private");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction ArePublic()
        {
            _ruleCreator.AddObjectFilter(obj => obj.Visibility == Public, "are public");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreProtected()
        {
            _ruleCreator.AddObjectFilter(obj => obj.Visibility == Protected, "are protected");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreInternal()
        {
            _ruleCreator.AddObjectFilter(obj => obj.Visibility == Internal, "are internal");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreProtectedInternal()
        {
            _ruleCreator.AddObjectFilter(obj => obj.Visibility == ProtectedInternal, "are protected internal");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction ArePrivateProtected()
        {
            _ruleCreator.AddObjectFilter(obj => obj.Visibility == PrivateProtected, "are private protected");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TGivenRuleTypeConjunction AreNot(ICanBeAnalyzed obj)
        {
            _ruleCreator.AddObjectFilter(o => !o.Equals(obj), "are not \"" + obj.FullName + "\"");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotDependOn(string pattern)
        {
            _ruleCreator.AddObjectFilter(obj => !obj.DependsOn(pattern), "do not depend on \"" + pattern + "\"");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveName(string name)
        {
            _ruleCreator.AddObjectFilter(obj => !obj.Name.Equals(name), "do not have name \"" + name + "\"");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveFullName(string fullname)
        {
            _ruleCreator.AddObjectFilter(obj => !obj.FullName.Equals(fullname),
                "do not have full name \"" + fullname + "\"");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveNameStartingWith(string pattern)
        {
            _ruleCreator.AddObjectFilter(obj => !obj.NameStartsWith(pattern),
                "do not have name starting with \"" + pattern + "\"");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveNameEndingWith(string pattern)
        {
            _ruleCreator.AddObjectFilter(obj => !obj.NameEndsWith(pattern),
                "do not have name ending with \"" + pattern + "\"");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveNameContaining(string pattern)
        {
            _ruleCreator.AddObjectFilter(obj => !obj.NameContains(pattern),
                "do not have name containing \"" + pattern + "\"");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotPrivate()
        {
            _ruleCreator.AddObjectFilter(obj => obj.Visibility != Private, "are not private");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotPublic()
        {
            _ruleCreator.AddObjectFilter(obj => obj.Visibility != Public, "are not public");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotProtected()
        {
            _ruleCreator.AddObjectFilter(obj => obj.Visibility != Protected, "are not protected");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotInternal()
        {
            _ruleCreator.AddObjectFilter(obj => obj.Visibility != Internal, "are not internal");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotProtectedInternal()
        {
            _ruleCreator.AddObjectFilter(obj => obj.Visibility != ProtectedInternal, "are not protected internal");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotPrivateProtected()
        {
            _ruleCreator.AddObjectFilter(obj => obj.Visibility != PrivateProtected, "are not private protected");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }
    }
}