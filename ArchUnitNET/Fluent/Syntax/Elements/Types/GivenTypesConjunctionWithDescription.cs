//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//
// 	SPDX-License-Identifier: Apache-2.0

using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public class GivenTypesConjunctionWithDescription
        : GivenObjectsConjunctionWithDescription<
            GivenTypesThat<GivenTypesConjunction, IType>,
            TypesShould<TypesShouldConjunction, IType>,
            IType
        >
    {
        public GivenTypesConjunctionWithDescription(IArchRuleCreator<IType> ruleCreator)
            : base(ruleCreator) { }
    }
}
