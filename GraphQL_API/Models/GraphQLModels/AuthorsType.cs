using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraphQL_API.Models.GraphQLModels
{
    public class GenreType : ObjectGraphType<GraphQL_API.Models.DatabaseEntity.Genre>
    {
        public GenreType()
        {
            Name = "Genre";
            Field(x => x.Id).Description("ID");
            Field(x => x.Name, nullable: true).Description("genre");
        }
    }

    public class BookType : ObjectGraphType<GraphQL_API.Models.DatabaseEntity.Book>
    {
        public BookType()
        {
            Name = "Book";
            Field(d => d.Id).Description("ID");
            Field(d => d.Name).Description("Name");
            Field<ListGraphType<GenreType>>(
                            "genres", "list of genre");
        }
    }

    public class AuthorsType : ObjectGraphType<GraphQL_API.Models.DatabaseEntity.Author>
    {
        public AuthorsType()
        {
            Name = "Author";
            Field(x => x.Id).Description("ID");
            Field(x => x.Name, nullable: true).Description("Name of author");
            Field(x => x.Country, nullable: true).Description("Country");
            Field<ListGraphType<BookType>>().Name("books")
                   .Description("list of books");
                   //.Argument<NonNullGraphType<IntGraphType>>("id", "ids")
                   //.Argument<NonNullGraphType<StringGraphType>>("name", "name")
                   //.Argument<NonNullGraphType<ListGraphType<StringGraphType>>>("genres", "genres")
                   //.Resolve(context =>
                   //{
                   //var name = context.GetArgument<string>("name");
                   //var genresList = context.GetArgument<List<String>>("genres");
                   //});
        }
    }
}