﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Bff.Models;
using GraphQL.DataLoader;
using GraphQL.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GraphQL.Bff.Controllers
{
    [Route("graphql")]
    [ApiController]
    public class GraphQLController : ControllerBase
    {
        private readonly IDocumentExecuter _documentExecuter;
        private readonly ISchema _schema;
        private readonly ILogger<GraphQLController> _logger;
        private readonly DataLoaderDocumentListener _dataLoaderDocumentListener;

        public GraphQLController(ISchema schema, IDocumentExecuter documentExecuter, ILogger<GraphQLController> logger, DataLoaderDocumentListener dataLoaderDocumentListener)
        {
            _schema = schema;
            _documentExecuter = documentExecuter;
            _logger = logger;
            _dataLoaderDocumentListener = dataLoaderDocumentListener;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GraphQLQuery query)
        {
            if (query == null) { throw new ArgumentNullException(nameof(query)); }
            var inputs = query.Variables.ToInputs();
            var executionOptions = new ExecutionOptions
            {
                Schema = _schema,
                Query = query.Query,
                Inputs = inputs,
            };
            executionOptions.Listeners.Add(_dataLoaderDocumentListener);

            _logger.Log(LogLevel.Information, "Executing {query.Query}", query.Query);

            var result = await _documentExecuter.ExecuteAsync(executionOptions).ConfigureAwait(false);

            if (result.Errors?.Count > 0)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

    }
}