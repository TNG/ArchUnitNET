/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using TestAssembly.Domain.Marker;
using TestAssembly.Domain.Services;

// ReSharper disable UnusedMember.Global

namespace TestAssembly.Domain.Repository
{
    public class RepositoryWithDependencyToService : IRepository
    {
        private TestService _badDependency;
    }
}