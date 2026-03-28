using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using BonProfCa.Models;
using BonProfCa.Services;

namespace BonProfCa.Controllers
{

    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("[controller]")]
    [ApiController]
    [EnableCors]
    public class AddressesController(AddressesService addressesService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<Response<List<AddressDetails>>>> GetAddressesByUserId()
        {
            var response = await addressesService.GetAddressesByUserIdAsync(User);

            if (response.Status == 200)
            {
                return Ok(response);
            }

            return StatusCode(response.Status, response);
        }

        [HttpPost]
        public async Task<ActionResult<Response<AddressDetails>>> CreateAddress(
            [FromBody] AddressCreate addressDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response<object>
                {
                    Status = 400,
                    Message = "Données de validation invalides",
                    Data = ModelState
                });
            }

            var response = await addressesService.CreateAddressAsync(addressDto, User);

            return StatusCode(response.Status, response);
        }

        [HttpPut]
        public async Task<ActionResult<Response<AddressDetails>>> UpdateAddress(
            [FromBody] AddressUpdate addressDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response<object>
                {
                    Status = 400,
                    Message = "Données de validation invalides",
                    Data = ModelState
                });
            }

            var response = await addressesService.UpdateAddressAsync(addressDto, User);

            return StatusCode(response.Status, response);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<Response<object>>> DeleteAddress(
            [FromRoute] Guid id)
        {
            var response = await addressesService.DeleteAddressAsync(id, User);

            return StatusCode(response.Status, response);
        }
    }
}
