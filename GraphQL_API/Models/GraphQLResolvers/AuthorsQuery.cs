using GraphQL.Types;
using GraphQL_API.Models.DatabaseEntity;
using GraphQL_API.Models.GraphQLModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraphQL_API.Models.GraphQLResolvers
{
    public class AuthorsQuery : ObjectGraphType
    {
        public AuthorsQuery()
        {
            Field<AuthorsType>(
              "authors",
              //arguments: new QueryArguments(new QueryArgument<StringGraphType> { Name = "name" }),
              resolve: context =>
              {

                  //var name = context.GetArgument<string>("name");
                  using (var db = new BooksEntities())
                  {
                      var list = (from x in db.Authors select x).ToList().FirstOrDefault();
                      list.Books.ToList();
                      var type = list.Books.GetType();
                      foreach(var props in list.Books)
                      {
                          props.Genres.ToList();
                      }
                      
                      return list;
                  }
              }
            );
        }
    }
}