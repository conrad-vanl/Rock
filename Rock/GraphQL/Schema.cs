
using GraphQL.Types;

namespace Rock.GraphQL
{
    public class RockSchema : Schema
    {
        public RockSchema()
        {
            Query = new RootQuery();
        }
    }
}