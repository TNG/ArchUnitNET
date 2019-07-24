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

using ArchUnitNET.Domain.Dependencies.Members;
using ArchUnitNET.Fluent;
using JetBrains.Annotations;
using Xunit;

namespace ArchUnitNETTests.Dependencies.Members
{
    public class MemberDependencyTests
    {
        [Theory]
        [ClassData(typeof(MemberDependencyTestBuild.MemberDependencyModelingTestData))]
        public void MemberTypeDependencyEquivalencyTests(IMemberTypeDependency memberTypeDependency,
            object duplicateMemberTypeDependency, IMemberTypeDependency dependencyReferenceDuplicate,
            [CanBeNull] object objectReferenceDuplicate)
        {
            if (memberTypeDependency == null || duplicateMemberTypeDependency == null
                                             || dependencyReferenceDuplicate == null)
            {
                return;
            }
            
            DuplicateMemberDependenciesAreEqual(memberTypeDependency, duplicateMemberTypeDependency);
            DuplicateMemberDependencyObjectReferencesAreEqual(memberTypeDependency, objectReferenceDuplicate);
            DuplicateMemberDependencyReferencesAreEqual(memberTypeDependency, dependencyReferenceDuplicate);
            MemberDependencyDoesNotEqualNull(memberTypeDependency);
            MemberDependencyHasConsistentHashCode(memberTypeDependency, duplicateMemberTypeDependency);
        }
        
        private static void DuplicateMemberDependenciesAreEqual([NotNull] IMemberTypeDependency originMember,
            [NotNull] object duplicateMember)
        {
            originMember.RequiredNotNull();
            duplicateMember.RequiredNotNull();

            Assert.Equal(originMember, duplicateMember);
        }

        private static void DuplicateMemberDependencyObjectReferencesAreEqual([NotNull] IMemberTypeDependency memberDependency,
            object objectReferenceDuplicate)
        {
            memberDependency.RequiredNotNull();
            objectReferenceDuplicate.RequiredNotNull();

            Assert.Equal(memberDependency, objectReferenceDuplicate);
        }

        private static void DuplicateMemberDependencyReferencesAreEqual([NotNull] IMemberTypeDependency dependency,
            [NotNull] IMemberTypeDependency dependencyReferenceDuplicate)
        {
            dependency.RequiredNotNull();
            dependencyReferenceDuplicate.RequiredNotNull();
            
            Assert.True(dependency.Equals(dependencyReferenceDuplicate));
        }

        private static void MemberDependencyDoesNotEqualNull([NotNull] IMemberTypeDependency memberDependency)
        {
            memberDependency.RequiredNotNull();
            
            Assert.False(memberDependency.Equals(null));
        }

        private static void MemberDependencyHasConsistentHashCode([NotNull] IMemberTypeDependency memberDependency,
            [NotNull] object duplicateMemberDependency)
        {
            memberDependency.RequiredNotNull();
            duplicateMemberDependency.RequiredNotNull();
            
            var hash = memberDependency.GetHashCode();
            var duplicateHash = duplicateMemberDependency.GetHashCode();
            Assert.Equal(hash, duplicateHash);
        }
    }

    
}