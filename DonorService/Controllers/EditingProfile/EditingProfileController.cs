using Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Enums;
using Models.Models;

namespace DonorService.Controllers.EditingProfile
{
    [ApiController]
    [Route("[controller]")]
    public class EditingProfileController(DiliveryContext diliveryContext) : ControllerBase
    {
        [HttpPost("/editProfile")]
        [EndpointSummary("редактирование профиля")]
        public async Task<ActionResult> EditProfile([FromHeader] int donorId, Tuple<string, BloodTypeEnum> data)
        {
            var donor = diliveryContext.Donors.FirstOrDefault(x => x.Id == donorId);
            if (donor == null) return BadRequest("not donor");

            donor.FullName = data.Item1;
            donor.BloodTypeEnum = data.Item2;
            donor.IsEdit = true;

            await diliveryContext.SaveChangesAsync();
            return Ok(donor);
        }
        [HttpPost("changeProfileIcon")]
        [EndpointSummary("смена иконки профиля")]
        public async Task<ActionResult<Donor>> ChangeIconProfile([FromHeader] int donorId, [FromBody] string base64)
        {
            var donor = diliveryContext.Donors.FirstOrDefault(x => x.Id == donorId);
            if (donor == null)
                return BadRequest();
            else
            {
                donor.Base64Image = base64;
                await diliveryContext.SaveChangesAsync();
                return Ok(donor);
            }
        }
    }
}
