using System;
using System.Collections.Generic;
using Epic.Sample.Application.Commands;
using Epic.Sample.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Epic.Sample.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController: Controller
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public void Add([FromBody] NewProduct product)
        {
            _mediator.Send(product);
        }
        
        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(Guid id, [FromBody] EditProduct product)
        {
            product.ProductId = id;
            _mediator.Send(product);
        }

        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            _mediator.Send(new DeleteProduct {Id = id});
        }
    }
}