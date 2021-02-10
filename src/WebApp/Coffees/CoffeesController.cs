using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Coffees;
using Domain;
using Domain.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Coffees
{
    [ApiController]
    [Route("api/v1/coffees")]
    public sealed class CoffeesController : Controller
    {
        private readonly IMediator _mediator;

        public CoffeesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IReadOnlyCollection<Dto.Coffee>> GetCoffees(CancellationToken cancellationToken)
        {
            var coffees = await _mediator.Send(new RetrieveAllCoffeesCommand(), cancellationToken);

            return coffees.Select(c => new Dto.Coffee
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                PriceInUsd = c.Price,
                WeightPerBagInOz = c.NetWeightPerBag,
            }).ToArray();
        }

        [HttpPost]
        public async Task<ActionResult<Dto.Coffee>> CreateCoffee([FromBody] Dto.CoffeePostRequest request, CancellationToken cancellationToken)
        {
            var cmd = new AddCoffeeCommand
            {
                Name = Name<Coffee>.Create(request.Name!),
                Description = Description.Create(request.Description!),
                Price = UsdPrice.Create(request.PriceInUsd!.Value),
                NetWeightPerBag = Ounces.Create(request.WeightPerBagInOz!.Value),
            };

            var coffee = await _mediator.Send(cmd, cancellationToken);

            return Created($"/api/v1/coffees/{coffee.Id}", new Dto.Coffee
            {
                Id = coffee.Id,
                Name = coffee.Name,
                Description = coffee.Description,
                PriceInUsd = coffee.Price,
                WeightPerBagInOz = coffee.NetWeightPerBag,
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Dto.Coffee>> UpdateCoffee(Guid id, [FromBody] Dto.CoffeePostRequest request, CancellationToken cancellationToken)
        {
            var cmd = new UpdateCoffeeCommand
            {
                CoffeeId = Id<Coffee>.From(id),
                Name = request.Name != null ? Name<Coffee>.Create(request.Name) : null,
                Description = request.Description != null ? Description.Create(request.Description) : null,
                Price = request.PriceInUsd.HasValue ? UsdPrice.Create(request.PriceInUsd.Value) : null,
                WeightPerBag = request.WeightPerBagInOz.HasValue ? Ounces.Create(request.WeightPerBagInOz.Value) : null,
            };

            var coffee = await _mediator.Send(cmd, cancellationToken);

            return Accepted($"/api/v1/coffees/{coffee.Id}", new Dto.Coffee
            {
                Id = coffee.Id,
                Name = coffee.Name,
                Description = coffee.Description,
                PriceInUsd = coffee.Price,
                WeightPerBagInOz = coffee.NetWeightPerBag,
            });
        }
    }
}
