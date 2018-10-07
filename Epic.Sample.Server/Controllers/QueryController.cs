using System;
using System.Threading.Tasks;
using Epic.Sample.Application.Queries;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;

namespace Epic.Sample.Server.Controllers
{
    [Route("[controller]")] 
    public class QueryController : Controller
    {
        private readonly IDocumentExecuter _document;
        private readonly ISchema _schema;

        public QueryController(ISchema schema, IDocumentExecuter document)
        {
            _schema = schema;
            _document = document;
        }
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GraphQuery query)
        {
            if (query == null) { throw new ArgumentNullException(nameof(query)); }
            var inputs = query.Variables.ToInputs();
            var executionOptions = new ExecutionOptions
            {
                Schema = _schema,
                Query = query.Query,
                Inputs = inputs
            };

            var result = await _document.ExecuteAsync(executionOptions).ConfigureAwait(false);

            if (result.Errors?.Count > 0)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}