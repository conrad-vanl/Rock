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

namespace Rock.Api.Cms
{
	/// <summary>
	/// REST WCF service for SiteDomains
	/// </summary>
    [Export(typeof(IService))]
    [ExportMetadata("RouteName", "api/Cms/SiteDomain")]
	[AspNetCompatibilityRequirements( RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed )]
    public partial class SiteDomainService : ISiteDomainService, IService
    {
		/// <summary>
		/// Gets a SiteDomain object
		/// </summary>
		[WebGet( UriTemplate = "{id}" )]
        public Rock.Models.Cms.SiteDomainDTO Get( string id )
        {
            var currentUser = System.Web.Security.Membership.GetUser();
            if ( currentUser == null )
                throw new FaultException( "Must be logged in" );

            using (Rock.Helpers.UnitOfWorkScope uow = new Rock.Helpers.UnitOfWorkScope())
            {
                uow.objectContext.Configuration.ProxyCreationEnabled = false;
				Rock.Services.Cms.SiteDomainService SiteDomainService = new Rock.Services.Cms.SiteDomainService();
                Rock.Models.Cms.SiteDomain SiteDomain = SiteDomainService.Get( int.Parse( id ) );
                if ( SiteDomain.Authorized( "View", currentUser ) )
                    return SiteDomain.DataTransferObject;
                else
                    throw new FaultException( "Unauthorized" );
            }
        }
		
		/// <summary>
		/// Updates a SiteDomain object
		/// </summary>
		[WebInvoke( Method = "PUT", UriTemplate = "{id}" )]
        public void UpdateSiteDomain( string id, Rock.Models.Cms.SiteDomainDTO SiteDomain )
        {
            var currentUser = System.Web.Security.Membership.GetUser();
            if ( currentUser == null )
                throw new FaultException( "Must be logged in" );

            using ( Rock.Helpers.UnitOfWorkScope uow = new Rock.Helpers.UnitOfWorkScope() )
            {
                uow.objectContext.Configuration.ProxyCreationEnabled = false;

                Rock.Services.Cms.SiteDomainService SiteDomainService = new Rock.Services.Cms.SiteDomainService();
                Rock.Models.Cms.SiteDomain existingSiteDomain = SiteDomainService.Get( int.Parse( id ) );
                if ( existingSiteDomain.Authorized( "Edit", currentUser ) )
                {
                    uow.objectContext.Entry(existingSiteDomain).CurrentValues.SetValues(SiteDomain);
                    SiteDomainService.Save( existingSiteDomain, currentUser.PersonId() );
                }
                else
                    throw new FaultException( "Unauthorized" );
            }
        }

		/// <summary>
		/// Creates a new SiteDomain object
		/// </summary>
		[WebInvoke( Method = "POST", UriTemplate = "" )]
        public void CreateSiteDomain( Rock.Models.Cms.SiteDomainDTO SiteDomain )
        {
            var currentUser = System.Web.Security.Membership.GetUser();
            if ( currentUser == null )
                throw new FaultException( "Must be logged in" );

            using ( Rock.Helpers.UnitOfWorkScope uow = new Rock.Helpers.UnitOfWorkScope() )
            {
                uow.objectContext.Configuration.ProxyCreationEnabled = false;

                Rock.Services.Cms.SiteDomainService SiteDomainService = new Rock.Services.Cms.SiteDomainService();
                Rock.Models.Cms.SiteDomain existingSiteDomain = new Rock.Models.Cms.SiteDomain();
				SiteDomainService.Add( existingSiteDomain, currentUser.PersonId() );
                uow.objectContext.Entry(existingSiteDomain).CurrentValues.SetValues(SiteDomain);
                SiteDomainService.Save( existingSiteDomain, currentUser.PersonId() );
            }
        }

		/// <summary>
		/// Deletes a SiteDomain object
		/// </summary>
		[WebInvoke( Method = "DELETE", UriTemplate = "{id}" )]
        public void DeleteSiteDomain( string id )
        {
            var currentUser = System.Web.Security.Membership.GetUser();
            if ( currentUser == null )
                throw new FaultException( "Must be logged in" );

            using ( Rock.Helpers.UnitOfWorkScope uow = new Rock.Helpers.UnitOfWorkScope() )
            {
                uow.objectContext.Configuration.ProxyCreationEnabled = false;

                Rock.Services.Cms.SiteDomainService SiteDomainService = new Rock.Services.Cms.SiteDomainService();
                Rock.Models.Cms.SiteDomain SiteDomain = SiteDomainService.Get( int.Parse( id ) );
                if ( SiteDomain.Authorized( "Edit", currentUser ) )
                {
                    SiteDomainService.Delete( SiteDomain, currentUser.PersonId() );
                }
                else
                    throw new FaultException( "Unauthorized" );
            }
        }

    }
}
