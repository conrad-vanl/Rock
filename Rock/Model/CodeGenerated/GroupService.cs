//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Rock.CodeGeneration project
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
// <copyright>
// Copyright by the Spark Development Network
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//
using System;
using System.Linq;

using Rock.Data;

namespace Rock.Model
{
    /// <summary>
    /// Group Service class
    /// </summary>
    public partial class GroupService : Service<Group>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GroupService"/> class
        /// </summary>
        /// <param name="context">The context.</param>
        public GroupService(RockContext context) : base(context)
        {
        }

        /// <summary>
        /// Determines whether this instance can delete the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>
        ///   <c>true</c> if this instance can delete the specified item; otherwise, <c>false</c>.
        /// </returns>
        public bool CanDelete( Group item, out string errorMessage )
        {
            errorMessage = string.Empty;
 
            if ( new Service<ConnectionRequest>( Context ).Queryable().Any( a => a.AssignedGroupId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", Group.FriendlyTypeName, ConnectionRequest.FriendlyTypeName );
                return false;
            }  
 
            if ( new Service<FinancialPersonSavedAccount>( Context ).Queryable().Any( a => a.GroupId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", Group.FriendlyTypeName, FinancialPersonSavedAccount.FriendlyTypeName );
                return false;
            }  
 
            if ( new Service<Group>( Context ).Queryable().Any( a => a.ParentGroupId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} contains one or more child {1}.", Group.FriendlyTypeName, Group.FriendlyTypeName.Pluralize().ToLower() );
                return false;
            }  
            
            // ignoring GroupRequirement,GroupId 
 
            if ( new Service<Person>( Context ).Queryable().Any( a => a.GivingGroupId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", Group.FriendlyTypeName, Person.FriendlyTypeName );
                return false;
            }  
 
            if ( new Service<Registration>( Context ).Queryable().Any( a => a.GroupId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", Group.FriendlyTypeName, Registration.FriendlyTypeName );
                return false;
            }  
 
            if ( new Service<WorkflowActivity>( Context ).Queryable().Any( a => a.AssignedGroupId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", Group.FriendlyTypeName, WorkflowActivity.FriendlyTypeName );
                return false;
            }  
            return true;
        }
    }

    /// <summary>
    /// Generated Extension Methods
    /// </summary>
    public static partial class GroupExtensionMethods
    {
        /// <summary>
        /// Clones this Group object to a new Group object
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="deepCopy">if set to <c>true</c> a deep copy is made. If false, only the basic entity properties are copied.</param>
        /// <returns></returns>
        public static Group Clone( this Group source, bool deepCopy )
        {
            if (deepCopy)
            {
                return source.Clone() as Group;
            }
            else
            {
                var target = new Group();
                target.CopyPropertiesFrom( source );
                return target;
            }
        }

        /// <summary>
        /// Copies the properties from another Group object to this Group object
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        public static void CopyPropertiesFrom( this Group target, Group source )
        {
            target.Id = source.Id;
            target.AddUserAccountsDuringSync = source.AddUserAccountsDuringSync;
            target.AllowGuests = source.AllowGuests;
            target.CampusId = source.CampusId;
            target.Description = source.Description;
            target.ExitSystemEmailId = source.ExitSystemEmailId;
            target.ForeignGuid = source.ForeignGuid;
            target.ForeignKey = source.ForeignKey;
            target.GroupTypeId = source.GroupTypeId;
            target.IsActive = source.IsActive;
            target.IsPublic = source.IsPublic;
            target.IsSecurityRole = source.IsSecurityRole;
            target.IsSystem = source.IsSystem;
            target.MustMeetRequirementsToAddMember = source.MustMeetRequirementsToAddMember;
            target.Name = source.Name;
            target.Order = source.Order;
            target.ParentGroupId = source.ParentGroupId;
            target.ScheduleId = source.ScheduleId;
            target.SyncDataViewId = source.SyncDataViewId;
            target.WelcomeSystemEmailId = source.WelcomeSystemEmailId;
            target.CreatedDateTime = source.CreatedDateTime;
            target.ModifiedDateTime = source.ModifiedDateTime;
            target.CreatedByPersonAliasId = source.CreatedByPersonAliasId;
            target.ModifiedByPersonAliasId = source.ModifiedByPersonAliasId;
            target.Guid = source.Guid;
            target.ForeignId = source.ForeignId;

        }
    }
}
