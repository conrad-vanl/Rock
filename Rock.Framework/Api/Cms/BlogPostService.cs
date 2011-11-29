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
	/// REST WCF service for BlogPosts
	/// </summary>
    [Export(typeof(IService))]
    [ExportMetadata("RouteName", "api/Cms/BlogPost")]
	[AspNetCompatibilityRequirements( RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed )]
    public partial class BlogPostService : IBlogPostService, IService
    {
		/// <summary>
		/// Gets a BlogPost object
		/// </summary>
		[WebGet( UriTemplate = "{id}" )]
        public Rock.Models.Cms.BlogPostDTO Get( string id )
        {
            var currentUser = System.Web.Security.Membership.GetUser();
            if ( currentUser == null )
                throw new FaultException( "Must be logged in" );

            using (Rock.Helpers.UnitOfWorkScope uow = new Rock.Helpers.UnitOfWorkScope())
            {
                uow.objectContext.Configuration.ProxyCreationEnabled = false;
				Rock.Services.Cms.BlogPostService BlogPostService = new Rock.Services.Cms.BlogPostService();
                Rock.Models.Cms.BlogPost BlogPost = BlogPostService.Get( int.Parse( id ) );
                if ( BlogPost.Authorized( "View", currentUser ) )
                    return BlogPost.DataTransferObject;
                else
                    throw new FaultException( "Unauthorized" );
            }
        }
		
		/// <summary>
		/// Updates a BlogPost object
		/// </summary>
		[WebInvoke( Method = "PUT", UriTemplate = "{id}" )]
        public void UpdateBlogPost( string id, Rock.Models.Cms.BlogPostDTO BlogPost )
        {
            var currentUser = System.Web.Security.Membership.GetUser();
            if ( currentUser == null )
                throw new FaultException( "Must be logged in" );

            using ( Rock.Helpers.UnitOfWorkScope uow = new Rock.Helpers.UnitOfWorkScope() )
            {
                uow.objectContext.Configuration.ProxyCreationEnabled = false;

                Rock.Services.Cms.BlogPostService BlogPostService = new Rock.Services.Cms.BlogPostService();
                Rock.Models.Cms.BlogPost existingBlogPost = BlogPostService.Get( int.Parse( id ) );
                if ( existingBlogPost.Authorized( "Edit", currentUser ) )
                {
                    uow.objectContext.Entry(existingBlogPost).CurrentValues.SetValues(BlogPost);
                    BlogPostService.Save( existingBlogPost, currentUser.PersonId() );
                }
                else
                    throw new FaultException( "Unauthorized" );
            }
        }

		/// <summary>
		/// Creates a new BlogPost object
		/// </summary>
		[WebInvoke( Method = "POST", UriTemplate = "" )]
        public void CreateBlogPost( Rock.Models.Cms.BlogPostDTO BlogPost )
        {
            var currentUser = System.Web.Security.Membership.GetUser();
            if ( currentUser == null )
                throw new FaultException( "Must be logged in" );

            using ( Rock.Helpers.UnitOfWorkScope uow = new Rock.Helpers.UnitOfWorkScope() )
            {
                uow.objectContext.Configuration.ProxyCreationEnabled = false;

                Rock.Services.Cms.BlogPostService BlogPostService = new Rock.Services.Cms.BlogPostService();
                Rock.Models.Cms.BlogPost existingBlogPost = new Rock.Models.Cms.BlogPost();
				BlogPostService.Add( existingBlogPost, currentUser.PersonId() );
                uow.objectContext.Entry(existingBlogPost).CurrentValues.SetValues(BlogPost);
                BlogPostService.Save( existingBlogPost, currentUser.PersonId() );
            }
        }

		/// <summary>
		/// Deletes a BlogPost object
		/// </summary>
		[WebInvoke( Method = "DELETE", UriTemplate = "{id}" )]
        public void DeleteBlogPost( string id )
        {
            var currentUser = System.Web.Security.Membership.GetUser();
            if ( currentUser == null )
                throw new FaultException( "Must be logged in" );

            using ( Rock.Helpers.UnitOfWorkScope uow = new Rock.Helpers.UnitOfWorkScope() )
            {
                uow.objectContext.Configuration.ProxyCreationEnabled = false;

                Rock.Services.Cms.BlogPostService BlogPostService = new Rock.Services.Cms.BlogPostService();
                Rock.Models.Cms.BlogPost BlogPost = BlogPostService.Get( int.Parse( id ) );
                if ( BlogPost.Authorized( "Edit", currentUser ) )
                {
                    BlogPostService.Delete( BlogPost, currentUser.PersonId() );
                }
                else
                    throw new FaultException( "Unauthorized" );
            }
        }

    }
}
