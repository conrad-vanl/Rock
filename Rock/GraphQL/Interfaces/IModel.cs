
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GraphQL.Types;
using Rock.GraphQL.Types;

namespace Rock.GraphQL.Interfaces
{
    public class ModelInterface : InterfaceGraphType
    {
        public string FriendlyTypeName;

        public ModelInterface()
        {
            Name = "Model";

            Field<NonNullGraphType<IdGraphType>>("id", "global node id");
            //Field<Person>("createdByPerson", "the person who created the model");
            //Field<Person>("modifiedByPerson", "the person who last modified the model");
            //Field<DateGraphType>("createdDateTime", "the creation time of the model");
            //Field<DateGraphType>("modifiedDateTime", "the last time the model was updated");
        }
    }
}