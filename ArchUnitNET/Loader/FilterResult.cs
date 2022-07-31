//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using Mono.Cecil;

namespace ArchUnitNET.Loader
{
    /// <summary>
    /// Type of delegate to control assemblies loading
    /// </summary>
    /// <param name="assemblyDefinition">Current assembly definition</param>
    public delegate FilterResult FilterFunc(AssemblyDefinition assemblyDefinition);
    
    /// <summary>
    /// Filter function result options
    /// </summary>
    public struct FilterResult
    {
        /// <summary>
        /// Load this assembly and traverse its dependencies
        /// </summary>
        public static FilterResult LoadAndContinue = new FilterResult(true, true);
        
        /// <summary>
        /// Do not load this assembly, but traverse its dependencies
        /// </summary>
        public static FilterResult SkipAndContinue = new FilterResult(true, false);
        
        /// <summary>
        /// Load this assembly and do not traverse its dependencies
        /// </summary>
        public static FilterResult LoadAndStop = new FilterResult(false, true);
        
        /// <summary>
        /// Do not load this assembly and do not traverse its dependencies
        /// </summary>
        public static FilterResult DontLoadAndStop = new FilterResult(false, false);
        
        private FilterResult(bool traverseDependencies, bool loadThisAssembly)
        {
            TraverseDependencies = traverseDependencies;
            LoadThisAssembly = loadThisAssembly;
        }

        internal bool TraverseDependencies { get; }
        
        internal bool LoadThisAssembly { get; }
    }
}