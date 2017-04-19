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
    /// PrayerRequest GraphQL Type
    /// </summary>
    public partial class PrayerRequest : ModelGraphType<Rock.Model.PrayerRequest>
    {
       public PrayerRequest(): base("PrayerRequest")
       {
          Field("AllowComments", x => x.AllowComments, nullable: true);
          Field("Answer", x => x.Answer, nullable: false);
          Field<Rock.GraphQL.Types.PersonAlias>("ApprovedByPersonAlias", resolve: x => x.Source.ApprovedByPersonAlias);
          Field("ApprovedByPersonAliasId", x => x.ApprovedByPersonAliasId, nullable: true);
          Field("ApprovedOnDateTime", x => x.ApprovedOnDateTime, nullable: true);
          Field<Rock.GraphQL.Types.Category>("Category", resolve: x => x.Source.Category);
          Field("CategoryId", x => x.CategoryId, nullable: true);
          Field("Email", x => x.Email, nullable: false);
          Field("EnteredDateTime", x => x.EnteredDateTime, nullable: false);
          Field("ExpirationDate", x => x.ExpirationDate, nullable: true);
          Field("FirstName", x => x.FirstName, nullable: false);
          Field("FlagCount", x => x.FlagCount, nullable: true);
          Field("ForeignGuid", x => x.ForeignGuid.ToStringSafe(), nullable: true);
          Field("ForeignKey", x => x.ForeignKey, nullable: false);
          Field("GroupId", x => x.GroupId, nullable: true);
          Field("IsActive", x => x.IsActive, nullable: true);
          Field("IsApproved", x => x.IsApproved, nullable: true);
          Field("IsPublic", x => x.IsPublic, nullable: true);
          Field("IsUrgent", x => x.IsUrgent, nullable: true);
          Field("LastName", x => x.LastName, nullable: false);
          Field("ModifiedAuditValuesAlreadyUpdated", x => x.ModifiedAuditValuesAlreadyUpdated, nullable: false);
          Field("PrayerCount", x => x.PrayerCount, nullable: true);
          Field<Rock.GraphQL.Types.PersonAlias>("RequestedByPersonAlias", resolve: x => x.Source.RequestedByPersonAlias);
          Field("RequestedByPersonAliasId", x => x.RequestedByPersonAliasId, nullable: true);
          Field("Text", x => x.Text, nullable: false);
          Field("CreatedDateTime", x => x.CreatedDateTime, nullable: true);
          Field("ModifiedDateTime", x => x.ModifiedDateTime, nullable: true);
          Field("CreatedByPersonAliasId", x => x.CreatedByPersonAliasId, nullable: true);
          Field("ModifiedByPersonAliasId", x => x.ModifiedByPersonAliasId, nullable: true);
          Field("Guid", x => x.Guid.ToStringSafe(), nullable: false);
          Field("ForeignId", x => x.ForeignId, nullable: true);
       }
       public override Rock.Model.PrayerRequest GetById(int id, GraphQLContext context)
       {
           var service = new Rock.Model.PrayerRequestService(context.db);
           return service.Get(id);
       }
   }
}
