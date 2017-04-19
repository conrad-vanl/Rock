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
    /// GroupRequirement GraphQL Type
    /// </summary>
    public partial class GroupRequirement : ModelGraphType<Rock.Model.GroupRequirement>
    {
       public GroupRequirement(): base("GroupRequirement")
       {
          Field("ForeignGuid", x => x.ForeignGuid.ToStringSafe(), nullable: true);
          Field("ForeignKey", x => x.ForeignKey, nullable: false);
          Field("GroupId", x => x.GroupId, nullable: false);
          Field<Rock.GraphQL.Types.GroupRequirementType>("GroupRequirementType", resolve: x => x.Source.GroupRequirementType);
          Field("GroupRequirementTypeId", x => x.GroupRequirementTypeId, nullable: false);
          Field("GroupRoleId", x => x.GroupRoleId, nullable: true);
          Field("ModifiedAuditValuesAlreadyUpdated", x => x.ModifiedAuditValuesAlreadyUpdated, nullable: false);
          Field("CreatedDateTime", x => x.CreatedDateTime, nullable: true);
          Field("ModifiedDateTime", x => x.ModifiedDateTime, nullable: true);
          Field("CreatedByPersonAliasId", x => x.CreatedByPersonAliasId, nullable: true);
          Field("ModifiedByPersonAliasId", x => x.ModifiedByPersonAliasId, nullable: true);
          Field("Guid", x => x.Guid.ToStringSafe(), nullable: false);
          Field("ForeignId", x => x.ForeignId, nullable: true);
       }
       public override Rock.Model.GroupRequirement GetById(int id, GraphQLContext context)
       {
           var service = new Rock.Model.GroupRequirementService(context.db);
           return service.Get(id);
       }
   }
}
