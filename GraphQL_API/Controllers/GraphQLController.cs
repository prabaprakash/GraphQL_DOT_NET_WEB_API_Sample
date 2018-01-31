using GraphQL;
using GraphQL.Http;
using GraphQL.Instrumentation;
using GraphQL.Types;
using GraphQL.Validation.Complexity;
using GraphQL_API.Models.GraphQLSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace GraphQL_API.Controllers
{
    public class GraphQLController : ApiController
    {
        private readonly IDictionary<string, string> _namedQueries;
        public GraphQLController()
        {
   

            _namedQueries = new Dictionary<string, string>()
            {
                ["a-query"] = @"query author { name }"
            };
        }
   
        [HttpGet]
        public Task<HttpResponseMessage> GetAsync(HttpRequestMessage request)
        {
            return PostAsync(request, new GraphQLQuery { Query = "query author { name }", Variables = null });
        }

        [HttpPost]
        public async Task<HttpResponseMessage> PostAsync(HttpRequestMessage request, GraphQLQuery query)
        {
            var inputs = query.Variables.ToInputs();
            var queryToExecute = query.Query;

            if (!string.IsNullOrWhiteSpace(query.NamedQuery))
            {
                queryToExecute = _namedQueries[query.NamedQuery];
            }

            var result = await new DocumentExecuter().ExecuteAsync(_ =>
            {
                _.Schema = new AuthorsSchema();
                _.Query = queryToExecute;
                _.OperationName = query.OperationName;
                _.Inputs = inputs;

                _.ComplexityConfiguration = new ComplexityConfiguration { MaxDepth = 15 };
                _.FieldMiddleware.Use<InstrumentFieldsMiddleware>();

            }).ConfigureAwait(false);

            var httpResult = result.Errors?.Count > 0
                ? HttpStatusCode.BadRequest
                : HttpStatusCode.OK;

            var json = new DocumentWriter().Write(result);

            var response = request.CreateResponse(httpResult);
            response.Content = new StringContent(json, Encoding.UTF8, "application/json");

            return response;
        }
        //public void fun()
        //{
        //    var schema = new AuthorsSchema();
        //    var authorsQuery = @"
        //         query {
        //         authors (name: ""nicholas cage"") {
        //            id
        //            name
        //            country
        //            books (id: 1,name:""praba"",genres: [""romance""]) {
        //               name
        //               id
        //               genres {
        //                       name
        //                       id
        //                 }
        //              } } }";
        //    var result = new DocumentExecuter().ExecuteAsync(_ =>
        //    {
        //        _.Schema = schema;
        //        _.Query = authorsQuery;
        //    }).GetAwaiter();

        //    var json = new DocumentWriter(indent: true).Write(result);
        //}
    }
    public class GraphQLQuery
    {
        public string OperationName { get; set; }
        public string NamedQuery { get; set; }
        public string Query { get; set; }
        public Newtonsoft.Json.Linq.JObject Variables { get; set; }
    }
}