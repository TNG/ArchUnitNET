using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;
using static ArchUnitNET.Domain.Visibility;
using static ArchUnitNET.Fluent.Syntax.ActivatorHandler;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public class ObjectsShouldThat<TRuleTypeShouldConjunction, TReferenceType, TRuleType> : SyntaxElement<TRuleType>,
        IObjectsThat<TRuleTypeShouldConjunction>
        where TReferenceType : ICanBeAnalyzed
        where TRuleType : ICanBeAnalyzed
    {
        // ReSharper disable once InconsistentNaming
        protected readonly ObjectProvider<TReferenceType> _referenceObjectProvider;

        // ReSharper disable once MemberCanBeProtected.Global
        public ObjectsShouldThat(ArchRuleCreator<TRuleType> ruleCreator,
            ObjectProvider<TReferenceType> referenceObjectProvider) : base(ruleCreator)
        {
            _referenceObjectProvider = referenceObjectProvider;
        }

        public TRuleTypeShouldConjunction Are(ICanBeAnalyzed obj)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider, o => o.Equals(obj),
                "are \"" + obj.FullName + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DependOn(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                obj => obj.DependsOn(pattern), "depend on \"" + pattern + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveName(string name)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                obj => obj.Name.Equals(name), "have name \"" + name + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveFullName(string fullname)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                obj => obj.FullName.Equals(fullname), "have full name \"" + fullname + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveNameStartingWith(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                obj => obj.NameStartsWith(pattern),
                "have name starting with \"" + pattern + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveNameEndingWith(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                obj => obj.NameEndsWith(pattern), "have name ending with \"" + pattern + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveNameContaining(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                obj => obj.NameContains(pattern), "have name containing \"" + pattern + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction ArePrivate()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                obj => obj.Visibility == Private, "are private");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction ArePublic()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                obj => obj.Visibility == Public, "are public");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreProtected()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                obj => obj.Visibility == Protected, "are protected");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreInternal()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                obj => obj.Visibility == Internal, "are internal");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreProtectedInternal()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                obj => obj.Visibility == ProtectedInternal, "are protected internal");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction ArePrivateProtected()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                obj => obj.Visibility == PrivateProtected, "are private protected");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TRuleTypeShouldConjunction AreNot(ICanBeAnalyzed obj)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider, o => !o.Equals(obj),
                "are not \"" + obj.FullName + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotDependOn(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                obj => !obj.DependsOn(pattern), "do not depend on \"" + pattern + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveName(string name)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                obj => !obj.Name.Equals(name), "do not have name \"" + name + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveFullName(string fullname)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                obj => !obj.FullName.Equals(fullname), "do not have full name \"" + fullname + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveNameStartingWith(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                obj => !obj.NameStartsWith(pattern), "do not have name starting with \"" + pattern + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveNameEndingWith(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                obj => !obj.NameEndsWith(pattern), "do not have name ending with \"" + pattern + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveNameContaining(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                obj => !obj.NameContains(pattern), "do not have name containing \"" + pattern + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotPrivate()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                obj => obj.Visibility != Private, "are not private");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotPublic()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                obj => obj.Visibility != Public, "are not public");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotProtected()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                obj => obj.Visibility != Protected, "are not protected");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotInternal()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                obj => obj.Visibility != Internal, "are not internal");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotProtectedInternal()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                obj => obj.Visibility != ProtectedInternal, "are not protected internal");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotPrivateProtected()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                obj => obj.Visibility != PrivateProtected, "are not private protected");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}