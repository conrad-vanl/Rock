//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the T4\Model.tt template.
//
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System.ComponentModel.Composition;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;

using Rock.Cms.Security;

namespace Rock.Api.Core
{
	/// <summary>
	/// REST WCF service for DefinedValues
	/// </summary>
    [Export(typeof(IService))]
    [ExportMetadata("RouteName", "api/Core/DefinedValue")]
	[AspNetCompatibilityRequirements( RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed )]
    public partial class DefinedValueService : IDefinedValueService, IService
    {
		/// <summary>
		/// Gets a DefinedValue object
		/// </summary>
		[WebGet( UriTemplate = "{id}" )]
        public Rock.Models.Core.DefinedValueDTO Get( string id )
        {
            var currentUser = System.Web.Security.Membership.GetUser();
            if ( currentUser == null )
                throw new FaultException( "Must be logged in" );

            using (Rock.Helpers.UnitOfWorkScope uow = new Rock.Helpers.UnitOfWorkScope())
            {
                uow.objectContext.Configuration.ProxyCreationEnabled = false;
				Rock.Services.Core.DefinedValueService DefinedValueService = new Rock.Services.Core.DefinedValueService();
                Rock.Models.Core.DefinedValue DefinedValue = DefinedValueService.Get( int.Parse( id ) );
                if ( DefinedValue.Authorized( "View", currentUser ) )
                    return DefinedValue.DataTransferObject;
                else
                    throw new FaultException( "Unauthorized" );
            }
        }
		
		/// <summary>
		/// Updates a DefinedValue object
		/// </summary>
		[WebInvoke( Method = "PUT", UriTemplate = "{id}" )]
        public void UpdateDefinedValue( string id, Rock.Models.Core.DefinedValueDTO DefinedValue )
        {
            var currentUser = System.Web.Security.Membership.GetUser();
            if ( currentUser == null )
                throw new FaultException( "Must be logged in" );

            using ( Rock.Helpers.UnitOfWorkScope uow = new Rock.Helpers.UnitOfWorkScope() )
            {
                uow.objectContext.Configuration.ProxyCreationEnabled = false;

                Rock.Services.Core.DefinedValueService DefinedValueService = new Rock.Services.Core.DefinedValueService();
                Rock.Models.Core.DefinedValue existingDefinedValue = DefinedValueService.Get( int.Parse( id ) );
                if ( existingDefinedValue.Authorized( "Edit", currentUser ) )
                {
                    uow.objectContext.Entry(existingDefinedValue).CurrentValues.SetValues(DefinedValue);
                    DefinedValueService.Save( existingDefinedValue, currentUser.PersonId() );
                }
                else
                    throw new FaultException( "Unauthorized" );
            }
        }

		/// <summary>
		/// Creates a new DefinedValue object
		/// </summary>
		[WebInvoke( Method = "POST", UriTemplate = "" )]
        public void CreateDefinedValue( Rock.Models.Core.DefinedValueDTO DefinedValue )
        {
            var currentUser = System.Web.Security.Membership.GetUser();
            if ( currentUser == null )
                throw new FaultException( "Must be logged in" );

            using ( Rock.Helpers.UnitOfWorkScope uow = new Rock.Helpers.UnitOfWorkScope() )
            {
                uow.objectContext.Configuration.ProxyCreationEnabled = false;

                Rock.Services.Core.DefinedValueService DefinedValueService = new Rock.Services.Core.DefinedValueService();
                Rock.Models.Core.DefinedValue existingDefinedValue = new Rock.Models.Core.DefinedValue();
				DefinedValueService.Add( existingDefinedValue, currentUser.PersonId() );
                uow.objectContext.Entry(existingDefinedValue).CurrentValues.SetValues(DefinedValue);
                DefinedValueService.Save( existingDefinedValue, currentUser.PersonId() );
            }
        }

		/// <summary>
		/// Deletes a DefinedValue object
		/// </summary>
		[WebInvoke( Method = "DELETE", UriTemplate = "{id}" )]
        public void DeleteDefinedValue( string id )
        {
            var currentUser = System.Web.Security.Membership.GetUser();
            if ( currentUser == null )
                throw new FaultException( "Must be logged in" );

            using ( Rock.Helpers.UnitOfWorkScope uow = new Rock.Helpers.UnitOfWorkScope() )
            {
                uow.objectContext.Configuration.ProxyCreationEnabled = false;

                Rock.Services.Core.DefinedValueService DefinedValueService = new Rock.Services.Core.DefinedValueService();
                Rock.Models.Core.DefinedValue DefinedValue = DefinedValueService.Get( int.Parse( id ) );
                if ( DefinedValue.Authorized( "Edit", currentUser ) )
                {
                    DefinedValueService.Delete( DefinedValue, currentUser.PersonId() );
                }
                else
                    throw new FaultException( "Unauthorized" );
            }
        }

    }
}
