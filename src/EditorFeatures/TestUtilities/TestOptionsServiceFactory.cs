// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.CodeAnalysis.Options;
using Microsoft.CodeAnalysis.Options.Providers;
using Microsoft.CodeAnalysis.Test.Utilities;
using Roslyn.Utilities;

namespace Microsoft.CodeAnalysis.Editor.UnitTests
{
    [ExportWorkspaceServiceFactory(typeof(IOptionService), TestWorkspaceName.Name), Shared]
    internal class TestOptionsServiceFactory : IWorkspaceServiceFactory
    {
        private readonly ImmutableArray<Lazy<IOptionProvider>> _providers;

        [ImportingConstructor]
        public TestOptionsServiceFactory(
            [ImportMany] IEnumerable<Lazy<IOptionProvider>> optionProviders)
        {
            _providers = optionProviders.ToImmutableArray();
        }

        public IWorkspaceService CreateService(HostWorkspaceServices workspaceServices)
        {
            // give out new option service per workspace
            return new OptionServiceFactory.OptionService(
                new GlobalOptionService(_providers, SpecializedCollections.EmptyEnumerable<Lazy<IOptionPersister>>()),
                workspaceServices);
        }
    }
}