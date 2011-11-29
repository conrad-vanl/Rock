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
	/// REST WCF service for Files
	/// </summary>
    [Export(typeof(IService))]
    [ExportMetadata("RouteName", "api/Cms/File")]
	[AspNetCompatibilityRequirements( RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed )]
    public partial class FileService : IFileService, IService
    {
		/// <summary>
		/// Gets a File object
		/// </summary>
		[WebGet( UriTemplate = "{id}" )]
        public Rock.Models.Cms.FileDTO Get( string id )
        {
            var currentUser = System.Web.Security.Membership.GetUser();
            if ( currentUser == null )
                throw new FaultException( "Must be logged in" );

            using (Rock.Helpers.UnitOfWorkScope uow = new Rock.Helpers.UnitOfWorkScope())
            {
                uow.objectContext.Configuration.ProxyCreationEnabled = false;
				Rock.Services.Cms.FileService FileService = new Rock.Services.Cms.FileService();
                Rock.Models.Cms.File File = FileService.Get( int.Parse( id ) );
                if ( File.Authorized( "View", currentUser ) )
                    return File.DataTransferObject;
                else
                    throw new FaultException( "Unauthorized" );
            }
        }
		
		/// <summary>
		/// Updates a File object
		/// </summary>
		[WebInvoke( Method = "PUT", UriTemplate = "{id}" )]
        public void UpdateFile( string id, Rock.Models.Cms.FileDTO File )
        {
            var currentUser = System.Web.Security.Membership.GetUser();
            if ( currentUser == null )
                throw new FaultException( "Must be logged in" );

            using ( Rock.Helpers.UnitOfWorkScope uow = new Rock.Helpers.UnitOfWorkScope() )
            {
                uow.objectContext.Configuration.ProxyCreationEnabled = false;

                Rock.Services.Cms.FileService FileService = new Rock.Services.Cms.FileService();
                Rock.Models.Cms.File existingFile = FileService.Get( int.Parse( id ) );
                if ( existingFile.Authorized( "Edit", currentUser ) )
                {
                    uow.objectContext.Entry(existingFile).CurrentValues.SetValues(File);
                    FileService.Save( existingFile, currentUser.PersonId() );
                }
                else
                    throw new FaultException( "Unauthorized" );
            }
        }

		/// <summary>
		/// Creates a new File object
		/// </summary>
		[WebInvoke( Method = "POST", UriTemplate = "" )]
        public void CreateFile( Rock.Models.Cms.FileDTO File )
        {
            var currentUser = System.Web.Security.Membership.GetUser();
            if ( currentUser == null )
                throw new FaultException( "Must be logged in" );

            using ( Rock.Helpers.UnitOfWorkScope uow = new Rock.Helpers.UnitOfWorkScope() )
            {
                uow.objectContext.Configuration.ProxyCreationEnabled = false;

                Rock.Services.Cms.FileService FileService = new Rock.Services.Cms.FileService();
                Rock.Models.Cms.File existingFile = new Rock.Models.Cms.File();
				FileService.Add( existingFile, currentUser.PersonId() );
                uow.objectContext.Entry(existingFile).CurrentValues.SetValues(File);
                FileService.Save( existingFile, currentUser.PersonId() );
            }
        }

		/// <summary>
		/// Deletes a File object
		/// </summary>
		[WebInvoke( Method = "DELETE", UriTemplate = "{id}" )]
        public void DeleteFile( string id )
        {
            var currentUser = System.Web.Security.Membership.GetUser();
            if ( currentUser == null )
                throw new FaultException( "Must be logged in" );

            using ( Rock.Helpers.UnitOfWorkScope uow = new Rock.Helpers.UnitOfWorkScope() )
            {
                uow.objectContext.Configuration.ProxyCreationEnabled = false;

                Rock.Services.Cms.FileService FileService = new Rock.Services.Cms.FileService();
                Rock.Models.Cms.File File = FileService.Get( int.Parse( id ) );
                if ( File.Authorized( "Edit", currentUser ) )
                {
                    FileService.Delete( File, currentUser.PersonId() );
                }
                else
                    throw new FaultException( "Unauthorized" );
            }
        }

    }
}
