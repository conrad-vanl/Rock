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
    /// BenevolenceRequest GraphQL Type
    /// </summary>
    public partial class BenevolenceRequest : ModelGraphType<Rock.Model.BenevolenceRequest>
    {
       public BenevolenceRequest(): base("BenevolenceRequest")
       {
          Field<Rock.GraphQL.Types.Campus>("Campus", resolve: x => x.Source.Campus);
          Field("CampusId", x => x.CampusId, nullable: true);
          Field<Rock.GraphQL.Types.PersonAlias>("CaseWorkerPersonAlias", resolve: x => x.Source.CaseWorkerPersonAlias);
          Field("CaseWorkerPersonAliasId", x => x.CaseWorkerPersonAliasId, nullable: true);
          Field("CellPhoneNumber", x => x.CellPhoneNumber, nullable: false);
          Field<Rock.GraphQL.Types.DefinedValue>("ConnectionStatusValue", resolve: x => x.Source.ConnectionStatusValue);
          Field("ConnectionStatusValueId", x => x.ConnectionStatusValueId, nullable: true);
          Field("Email", x => x.Email, nullable: false);
          Field("FirstName", x => x.FirstName, nullable: false);
          Field("ForeignGuid", x => x.ForeignGuid.ToStringSafe(), nullable: true);
          Field("ForeignKey", x => x.ForeignKey, nullable: false);
          Field("GovernmentId", x => x.GovernmentId, nullable: false);
          Field("HomePhoneNumber", x => x.HomePhoneNumber, nullable: false);
          Field("LastName", x => x.LastName, nullable: false);
          Field<Rock.GraphQL.Types.Location>("Location", resolve: x => x.Source.Location);
          Field("LocationId", x => x.LocationId, nullable: true);
          Field("ModifiedAuditValuesAlreadyUpdated", x => x.ModifiedAuditValuesAlreadyUpdated, nullable: false);
          Field("ProvidedNextSteps", x => x.ProvidedNextSteps, nullable: false);
          Field("RequestDateTime", x => x.RequestDateTime, nullable: false);
          Field<Rock.GraphQL.Types.PersonAlias>("RequestedByPersonAlias", resolve: x => x.Source.RequestedByPersonAlias);
          Field("RequestedByPersonAliasId", x => x.RequestedByPersonAliasId, nullable: true);
          Field<Rock.GraphQL.Types.DefinedValue>("RequestStatusValue", resolve: x => x.Source.RequestStatusValue);
          Field("RequestStatusValueId", x => x.RequestStatusValueId, nullable: true);
          Field("RequestText", x => x.RequestText, nullable: false);
          Field("ResultSummary", x => x.ResultSummary, nullable: false);
          Field("WorkPhoneNumber", x => x.WorkPhoneNumber, nullable: false);
          Field("CreatedDateTime", x => x.CreatedDateTime, nullable: true);
          Field("ModifiedDateTime", x => x.ModifiedDateTime, nullable: true);
          Field("CreatedByPersonAliasId", x => x.CreatedByPersonAliasId, nullable: true);
          Field("ModifiedByPersonAliasId", x => x.ModifiedByPersonAliasId, nullable: true);
          Field("Guid", x => x.Guid.ToStringSafe(), nullable: false);
          Field("ForeignId", x => x.ForeignId, nullable: true);
       }
       public override Rock.Model.BenevolenceRequest GetById(int id, GraphQLContext context)
       {
           var service = new Rock.Model.BenevolenceRequestService(context.db);
           return service.Get(id);
       }
   }
}
