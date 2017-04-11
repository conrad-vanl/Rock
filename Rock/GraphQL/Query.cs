
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

        }

    }

    public abstract class RockQuery : ObjectGraphType
    {
        public RockQuery(ObjectGraphType query)
        {
       
        }
    }
}