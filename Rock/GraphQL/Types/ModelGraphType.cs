
using GraphQL;
using GraphQL.Types;
using Panic.StringUtils;
using System;
using System.Linq;

using Rock.GraphQL.Interfaces;
using Rock.Model;
using Rock.Attribute;
using System.Linq.Dynamic;

namespace Rock.GraphQL.Types
{
    public class ModelGraphQuery : RockQuery
    {
        public ModelGraphQuery(ObjectGraphType query) : base(query)
        {
            query.AddField(
                Field<ModelInterface>(
                    "model",
                    arguments: new QueryArguments(new QueryArgument<IdGraphType> { Name = "id" }),
                    resolve: ResolveObjectFromGlobalId
                )
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
    public interface IRockModel<out T>
    {
        T GetById(int id, GraphQLContext context);
    }

    public static class Model
    {
        public static ModelGraphType<TSource, TOut> For<TSource, TOut>(Func<int, TOut> getById, string TypeName)
        {
            var type = new DefaultModelGraphType<TSource, TOut>(getById, TypeName);
            return type;
        }

        public static string ToGlobalId(string name, object id)
        {
            return StringUtils.Base64Encode(String.Format("{0}:{1}", name, id));
        }

        public static Tuple<string, int> FromGlobalId(string globalId)
        {
            var parts = StringUtils.Base64Decode(globalId).Split(':');
            string id = string.Join(":", parts.Skip(1));
            int castedId = Convert.ToInt32(id);
            return new Tuple<string, int>(
                parts[0],
                castedId
            );
        }

    }

    public abstract class ModelGraphType<T, TOut> : ObjectGraphType<T>, IRockModel<TOut>
    {
        public abstract TOut GetById(int id, GraphQLContext context);

        public ModelGraphType(string TypeName)
        {
            Name = TypeName;
            Interface<ModelInterface>();
            string name = StringUtils.ToCamelCase(TypeName + "Id");

            Field<NonNullGraphType<IdGraphType>>(
                "id",
                description: $"The Global Id of the {TypeName ?? "node"}",
                resolve: context =>
                {
                    var data = context.Source.As<Data.IModel>();
                    return Model.ToGlobalId(
                        context.ParentType.Name,
                        data.Id
                    );
                }
            );

            Field<NonNullGraphType<IntGraphType>>(
                name,
                description: $"The Id of the {TypeName ?? "node"}",
                resolve: context =>
                {
                    var data = context.Source.As<Data.IModel>();
                    return data.Id;
                }
            );

            Field<Person>(
                "createdByPerson",
                resolve: context =>
                {
                    var data = context.Source.As<Data.IModel>();

                    var userContext = context.UserContext.As<GraphQLContext>();
                    if (data.CreatedByPersonAliasId.HasValue)
                    {
                        var personAliasService = new PersonAliasService(userContext.db);
                        int id = data.CreatedByPersonAliasId ?? 0;
                        return personAliasService.GetPerson(id);
                    }
                    return null;
                }
            );


            Field<Person>(
                "modifiedByPerson",
                resolve: context =>
                {
                    var data = context.Source.As<Data.IModel>();

                    var userContext = context.UserContext.As<GraphQLContext>();
                    if (data.ModifiedByPersonAliasId.HasValue)
                    {
                        var personAliasService = new PersonAliasService(userContext.db);
                        int id = data.ModifiedByPersonAliasId ?? 0;
                        return personAliasService.GetPerson(id);
                    }
                    return null;
                }
            );

            Field<StringGraphType>(
                "attribute",
                arguments: new QueryArguments(new QueryArgument<StringGraphType> { Name = "key" }),
                resolve: context =>
                {
                    var data = context.Source.As<IHasAttributes>();

                    string key = context.GetArgument<string>("key");
                    var userContext = context.UserContext.As<GraphQLContext>();
                    data.LoadAttributes();
                    return data.GetAttributeValue(key);
                }
            );
        }
    }


    public abstract class ModelGraphType<TSource> : ModelGraphType<TSource, TSource>
    {
        public ModelGraphType(string TypeName) : base(TypeName)
        {
        }
    }
    public abstract class ModelGraphType : ModelGraphType<object>
    {
        public ModelGraphType(string TypeName) : base(TypeName)
        {
        }
    }

    public class DefaultModelGraphType<TSource, TOut> : ModelGraphType<TSource, TOut>
    {
        private readonly Func<int, TOut> _getById;
        private Func<int, TOut> getById;

        public DefaultModelGraphType(Func<int, TOut> getById, string TypeName) : base(TypeName)
        {
            _getById = getById;
        }

        public override TOut GetById(int id, GraphQLContext context)
        {
            return _getById(id);
        }

    }

}