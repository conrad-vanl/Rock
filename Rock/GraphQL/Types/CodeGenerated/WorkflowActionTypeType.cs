//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Rock.CodeGeneration project
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
// <copyright>
// Copyright by the Spark Development Network
//
// Licensed under the Rock Community License (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.rockrms.com/license
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//

using GraphQL;
using Rock.Model;

namespace Rock.GraphQL.Types
{
    /// <summary>
    /// WorkflowActionType GraphQL Type
    /// </summary>
    public partial class WorkflowActionType : ModelGraphType<Rock.Model.WorkflowActionType>
    {
       public WorkflowActionType(): base("WorkflowActionType")
       {
          Field("ActivityTypeId", x => x.ActivityTypeId, nullable: false);
          Field("CriteriaAttributeGuid", x => x.CriteriaAttributeGuid.ToStringSafe(), nullable: true);
          Field("CriteriaComparisonType", x => x.CriteriaComparisonType.ConvertToInt(), nullable: false);
          Field("CriteriaValue", x => x.CriteriaValue, nullable: false);
          Field<Rock.GraphQL.Types.EntityType>("EntityType", resolve: x => x.Source.EntityType);
          Field("EntityTypeId", x => x.EntityTypeId, nullable: false);
          Field("ForeignGuid", x => x.ForeignGuid.ToStringSafe(), nullable: true);
          Field("ForeignKey", x => x.ForeignKey, nullable: false);
          Field("IsActionCompletedOnSuccess", x => x.IsActionCompletedOnSuccess, nullable: false);
          Field("IsActivityCompletedOnSuccess", x => x.IsActivityCompletedOnSuccess, nullable: false);
          Field("ModifiedAuditValuesAlreadyUpdated", x => x.ModifiedAuditValuesAlreadyUpdated, nullable: false);
          Field("Name", x => x.Name, nullable: false);
          Field("Order", x => x.Order, nullable: false);
          Field<Rock.GraphQL.Types.WorkflowActionForm>("WorkflowForm", resolve: x => x.Source.WorkflowForm);
          Field("WorkflowFormId", x => x.WorkflowFormId, nullable: true);
          Field("CreatedDateTime", x => x.CreatedDateTime, nullable: true);
          Field("ModifiedDateTime", x => x.ModifiedDateTime, nullable: true);
          Field("CreatedByPersonAliasId", x => x.CreatedByPersonAliasId, nullable: true);
          Field("ModifiedByPersonAliasId", x => x.ModifiedByPersonAliasId, nullable: true);
          Field("Guid", x => x.Guid.ToStringSafe(), nullable: false);
          Field("ForeignId", x => x.ForeignId, nullable: true);
       }
       public override Rock.Model.WorkflowActionType GetById(int id, GraphQLContext context)
       {
           var service = new Rock.Model.WorkflowActionTypeService(context.db);
           return service.Get(id);
       }
   }
}
