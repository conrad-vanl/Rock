using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rock.Data;
using Rock.Model;
using System.Net.Http;

namespace Rock.GraphQL
{
    public class GraphQLContext
    {
        public RockContext db = new RockContext();
        public Person person { get; set; }
        public HttpRequestMessage request { get; set; }

        public GraphQLContext()
        {

        }
    }
}