using GraphQL.Types;
using GraphQL_API.Models.GraphQLModels;
using GraphQL_API.Models.GraphQLResolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraphQL_API.Models.GraphQLSchema
{
    public class AuthorsSchema : Schema
    {
        public AuthorsSchema()
        {
            Query = new AuthorsQuery();
            RegisterType<AuthorsType>();
            //RegisterType<BookType>();
            //RegisterType<GenreType>();
        }
    }
}