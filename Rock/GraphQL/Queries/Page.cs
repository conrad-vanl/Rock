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
using GraphQL.Types;
using Rock;
using Rock.Data;
using Rock.Model;
using System;
using System.Linq;
using System.Linq.Dynamic;

namespace Rock.GraphQL.Queries
{
   public class PageQuery : RockQuery
    {
        public PageQuery(ObjectGraphType query) : base(query)
        {
            query.AddField(
                Field<Types.Page>(
                    "route",
                    arguments: new QueryArguments(new QueryArgument<StringGraphType> { Name = "url" }),
                    resolve: context =>
                    {
                        RockContext db = context.UserContext.As<GraphQLContext>().db;
                        string url = context.GetArgument<string>("url");
                        Uri parseUrl = new Uri(url);

                        PageRouteService pages = new PageRouteService(db);
                        
                        return pages.Queryable().Where(p => p.Id == 12);
                    }
                )
            );
        }
        
    }

}
