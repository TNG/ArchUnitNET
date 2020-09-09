# Object

```
//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
```
   

		TReturnType Exist();
        TReturnType Be(string pattern, bool useRegularExpressions = false);
        TReturnType Be(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType Be(ICanBeAnalyzed firstObject, params ICanBeAnalyzed[] moreObjects);
        TReturnType Be(IEnumerable<ICanBeAnalyzed> objects);
        TReturnType Be(IObjectProvider<ICanBeAnalyzed> objects);
        TReturnType CallAny(string pattern, bool useRegularExpressions = false);
        TReturnType CallAny(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType CallAny(MethodMember method, params MethodMember[] moreMethods);
        TReturnType CallAny(IEnumerable<MethodMember> methods);
        TReturnType CallAny(IObjectProvider<MethodMember> methods);
        TReturnType DependOnAny(string pattern, bool useRegularExpressions = false);
        TReturnType DependOnAny(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType DependOnAny(IType firstType, params IType[] moreTypes);
        TReturnType DependOnAny(Type firstType, params Type[] moreTypes);
        TReturnType DependOnAny(IObjectProvider<IType> types);
        TReturnType DependOnAny(IEnumerable<IType> types);
        TReturnType DependOnAny(IEnumerable<Type> types);
        TReturnType OnlyDependOn(string pattern, bool useRegularExpressions = false);
        TReturnType OnlyDependOn(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType OnlyDependOn(IType firstType, params IType[] moreTypes);
        TReturnType OnlyDependOn(Type firstType, params Type[] moreTypes);
        TReturnType OnlyDependOn(IObjectProvider<IType> types);
        TReturnType OnlyDependOn(IEnumerable<IType> types);
        TReturnType OnlyDependOn(IEnumerable<Type> types);
        TReturnType HaveAnyAttributes(string pattern, bool useRegularExpressions = false);
        TReturnType HaveAnyAttributes(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType HaveAnyAttributes(Attribute firstAttribute, params Attribute[] moreAttributes);
        TReturnType HaveAnyAttributes(Type firstAttribute, params Type[] moreAttributes);
        TReturnType HaveAnyAttributes(IObjectProvider<Attribute> attributes);
        TReturnType HaveAnyAttributes(IEnumerable<Attribute> attributes);
        TReturnType HaveAnyAttributes(IEnumerable<Type> attributes);
        TReturnType OnlyHaveAttributes(string pattern, bool useRegularExpressions = false);
        TReturnType OnlyHaveAttributes(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType OnlyHaveAttributes(Attribute firstAttribute, params Attribute[] moreAttributes);
        TReturnType OnlyHaveAttributes(Type firstAttribute, params Type[] moreAttributes);
        TReturnType OnlyHaveAttributes(IObjectProvider<Attribute> attributes);
        TReturnType OnlyHaveAttributes(IEnumerable<Attribute> attributes);
        TReturnType OnlyHaveAttributes(IEnumerable<Type> attributes);
        TReturnType HaveName(string name);
        TReturnType HaveNameMatching(string pattern);
        TReturnType HaveFullName(string fullname);
        TReturnType HaveFullNameMatching(string pattern);
        TReturnType HaveNameStartingWith(string pattern);
        TReturnType HaveNameEndingWith(string pattern);
        TReturnType HaveNameContaining(string pattern);
        TReturnType HaveFullNameContaining(string pattern);
        TReturnType BePrivate();
        TReturnType BePublic();
        TReturnType BeProtected();
        TReturnType BeInternal();
        TReturnType BeProtectedInternal();
        TReturnType BePrivateProtected();


        //Negations


        TReturnType NotExist();
        TReturnType NotBe(string pattern, bool useRegularExpressions = false);
        TReturnType NotBe(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType NotBe(ICanBeAnalyzed firstObject, params ICanBeAnalyzed[] moreObjects);
        TReturnType NotBe(IEnumerable<ICanBeAnalyzed> objects);
        TReturnType NotBe(IObjectProvider<ICanBeAnalyzed> objects);
        TReturnType NotCallAny(string pattern, bool useRegularExpressions = false);
        TReturnType NotCallAny(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType NotCallAny(MethodMember method, params MethodMember[] moreMethods);
        TReturnType NotCallAny(IEnumerable<MethodMember> methods);
        TReturnType NotCallAny(IObjectProvider<MethodMember> methods);
        TReturnType NotDependOnAny(string pattern, bool useRegularExpressions = false);
        TReturnType NotDependOnAny(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType NotDependOnAny(IType firstType, params IType[] moreTypes);
        TReturnType NotDependOnAny(Type firstType, params Type[] moreTypes);
        TReturnType NotDependOnAny(IObjectProvider<IType> types);
        TReturnType NotDependOnAny(IEnumerable<IType> types);
        TReturnType NotDependOnAny(IEnumerable<Type> types);
        TReturnType NotHaveAnyAttributes(string pattern, bool useRegularExpressions = false);
        TReturnType NotHaveAnyAttributes(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType NotHaveAnyAttributes(Attribute firstAttribute, params Attribute[] moreAttributes);
        TReturnType NotHaveAnyAttributes(Type firstAttribute, params Type[] moreAttributes);
        TReturnType NotHaveAnyAttributes(IObjectProvider<Attribute> attributes);
        TReturnType NotHaveAnyAttributes(IEnumerable<Attribute> attributes);
        TReturnType NotHaveAnyAttributes(IEnumerable<Type> attributes);
        TReturnType NotHaveName(string name);
        TReturnType NotHaveNameMatching(string pattern);
        TReturnType NotHaveFullName(string fullname);
        TReturnType NotHaveFullNameMatching(string pattern);
        TReturnType NotHaveNameStartingWith(string pattern);
        TReturnType NotHaveNameEndingWith(string pattern);
        TReturnType NotHaveNameContaining(string pattern);
        TReturnType NotHaveFullNameContaining(string pattern);
        TReturnType NotBePrivate();
        TReturnType NotBePublic();
        TReturnType NotBeProtected();
        TReturnType NotBeInternal();
        TReturnType NotBeProtectedInternal();
        TReturnType NotBePrivateProtected();