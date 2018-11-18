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

namespace Microsoft.Azure.Commands.LogicApp.Cmdlets
{
    using Management.Logic.Models;
    using Microsoft.Azure.Commands.LogicApp.Utilities;
    using ResourceManager.Common.ArgumentCompleters;
    using System.Collections.Generic;
    using System.Management.Automation;

    /// <summary>
    /// Creates a new LogicApp workflow 
    /// </summary>
    [Cmdlet("Get", ResourceManager.Common.AzureRMConstants.AzureRMPrefix + "LogicApp"), OutputType(typeof(Workflow), typeof(WorkflowVersion))]
    public class GetAzureLogicAppCommand : LogicAppBaseCmdlet
    {

        #region Input Paramters

        [Parameter(Mandatory = true, HelpMessage = "The targeted resource group for the workflow.",
            ValueFromPipelineByPropertyName = true)]
        [ResourceGroupCompleter]
        [ValidateNotNullOrEmpty]
        public string ResourceGroupName { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "Item", HelpMessage = "The name of the workflow.")]
        [Parameter(Mandatory = false, ParameterSetName = "List", HelpMessage = "The name of the workflow.")]
        [Alias("ResourceName")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "Item", HelpMessage = "The version of the workflow.")]
        [ValidateNotNullOrEmpty]
        public string Version { get; set; }

        #endregion Input Parameters

        /// <summary>
        /// Executes the get workflow command
        /// </summary>
        public override void ExecuteCmdlet()
        {
            base.ExecuteCmdlet();
            if (string.IsNullOrWhiteSpace(this.Name))
            {
                if (string.IsNullOrWhiteSpace(this.Version))
                {
                    foreach (var workflow in LogicAppClient.ListWorkflows(this.ResourceGroupName))
                    { 
                        this.WriteObject(workflow, true);
                    }
                }
                else
                {
                    throw new PSNotImplementedException("The version parameter cannot be used without the name parameter.");
                }
            } else { 
                if (string.IsNullOrWhiteSpace(this.Version))
                {
                    this.WriteObject(LogicAppClient.GetWorkflow(this.ResourceGroupName, this.Name), true);
                }
                else
                {
                    this.WriteObject(LogicAppClient.GetWorkflowVersion(this.ResourceGroupName, this.Name, this.Version), true);
                }
            }
        }
    }
}
