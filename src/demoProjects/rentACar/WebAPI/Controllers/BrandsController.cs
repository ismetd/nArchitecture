using Application.Features.Brands.Commands.CreateBrand;
using Application.Features.Brands.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BrandsController : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateBrandCommand brandCommand)
    {
        CreatedBrandDto result = await Mediator.Send(brandCommand);
        return Created("", result);
    }

}
