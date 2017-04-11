
using System.Collections.Generic;
using GraphQL.Types;
using GraphQL;

using Rock.GraphQL.Interfaces;
using Rock.GraphQL.Types;
using System.Linq;
using System;
using Rock;
using Rock.Model;

namespace Rock.GraphQL
{

    public class RootQuery : ObjectGraphType
    {
        public RootQuery()
        {
            Name = "Query";

            foreach (var type in Reflection.FindTypes(typeof(RockQuery)))
            {
                var instance = (RockQuery)Activator.CreateInstance(type.Value, this);
            }

            Field<ModelInterface>(
                "model",
                arguments: new QueryArguments(new QueryArgument<IdGraphType> { Name = "id" }),
                resolve: ResolveObjectFromGlobalId
            );

        }

        private object ResolveObjectFromGlobalId(ResolveFieldContext<object> context)
        {
            string globalId = context.GetArgument<string>("id");
            var parts = Types.Model.FromGlobalId(globalId);
            var node = (IRockModel<object>)context.Schema.FindType(parts.Item1);

            var userContext = context.UserContext.As<GraphQLContext>();

            return node.GetById(parts.Item2, userContext);
        }

    }

    public abstract class RockQuery : ObjectGraphType
    {
        public RockQuery(ObjectGraphType query)
        {

        }
    }
}