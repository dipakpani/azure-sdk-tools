﻿// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

namespace Microsoft.WindowsAzure.Commands.ServiceManagement.AffinityGroups
{
    using System.Linq;
    using System.Management.Automation;
    using Management.Models;
    using Model;
    using Utilities.Common;

    /// <summary>
    /// List the properties for the specified affinity group.
    /// </summary>
    [Cmdlet(
        VerbsCommon.Get,
        "AzureAffinityGroup"),
    OutputType(
        typeof(AffinityGroupContext))]
    public class GetAzureAffinityGroup : ServiceManagementBaseCmdlet
    {
        [Parameter(
            Position = 0,
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Affinity Group name")]
        [ValidateNotNullOrEmpty]
        public string Name
        {
            get;
            set;
        }

        protected override void OnProcessRecord()
        {
            ServiceManagementProfile.Initialize();

            if (this.Name != null)
            {
                ExecuteClientActionNewSM(null, 
                    CommandRuntime.ToString(), 
                    () => this.ManagementClient.AffinityGroups.Get(this.Name),
                    (s, affinityGroup) => Enumerable.Repeat(affinityGroup, 1).Select(
                        ag => ContextFactory<AffinityGroupGetResponse, AffinityGroupContext>(ag, s))
                );
            }
            else
            {
                ExecuteClientActionNewSM(null, 
                    CommandRuntime.ToString(), 
                    () => this.ManagementClient.AffinityGroups.List(),
                    (s, affinityGroups) => affinityGroups.AffinityGroups.Select(
                        ag => ContextFactory<AffinityGroupListResponse.AffinityGroup, AffinityGroupContext>(ag, s))
                );
            }
        }
    }
}