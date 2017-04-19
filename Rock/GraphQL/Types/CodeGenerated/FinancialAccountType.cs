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
    /// FinancialAccount GraphQL Type
    /// </summary>
    public partial class FinancialAccount : ModelGraphType<Rock.Model.FinancialAccount>
    {
       public FinancialAccount(): base("FinancialAccount")
       {
          Field<Rock.GraphQL.Types.DefinedValue>("AccountTypeValue", resolve: x => x.Source.AccountTypeValue);
          Field("AccountTypeValueId", x => x.AccountTypeValueId, nullable: true);
          Field("CampusId", x => x.CampusId, nullable: true);
          Field("Description", x => x.Description, nullable: false);
          Field("EndDate", x => x.EndDate, nullable: true);
          Field("ForeignGuid", x => x.ForeignGuid.ToStringSafe(), nullable: true);
          Field("ForeignKey", x => x.ForeignKey, nullable: false);
          Field("GlCode", x => x.GlCode, nullable: false);
          Field<Rock.GraphQL.Types.BinaryFile>("ImageBinaryFile", resolve: x => x.Source.ImageBinaryFile);
          Field("ImageBinaryFileId", x => x.ImageBinaryFileId, nullable: true);
          Field("IsActive", x => x.IsActive, nullable: false);
          Field("IsPublic", x => x.IsPublic, nullable: true);
          Field("IsTaxDeductible", x => x.IsTaxDeductible, nullable: false);
          Field("ModifiedAuditValuesAlreadyUpdated", x => x.ModifiedAuditValuesAlreadyUpdated, nullable: false);
          Field("Name", x => x.Name, nullable: false);
          Field("Order", x => x.Order, nullable: false);
          Field("ParentAccountId", x => x.ParentAccountId, nullable: true);
          Field("PublicDescription", x => x.PublicDescription, nullable: false);
          Field("PublicName", x => x.PublicName, nullable: false);
          Field("StartDate", x => x.StartDate, nullable: true);
          Field("Url", x => x.Url, nullable: false);
          Field("CreatedDateTime", x => x.CreatedDateTime, nullable: true);
          Field("ModifiedDateTime", x => x.ModifiedDateTime, nullable: true);
          Field("CreatedByPersonAliasId", x => x.CreatedByPersonAliasId, nullable: true);
          Field("ModifiedByPersonAliasId", x => x.ModifiedByPersonAliasId, nullable: true);
          Field("Guid", x => x.Guid.ToStringSafe(), nullable: false);
          Field("ForeignId", x => x.ForeignId, nullable: true);
       }
       public override Rock.Model.FinancialAccount GetById(int id, GraphQLContext context)
       {
           var service = new Rock.Model.FinancialAccountService(context.db);
           return service.Get(id);
       }
   }
}
