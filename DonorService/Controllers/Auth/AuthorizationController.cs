using Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;
using Models.Models;
using System.Security.Cryptography;
using System.Text;

namespace DonorService.Controllers.Auth
{
    [ApiController]
    public class AuthorizationController(DiliveryContext diliveryContext) : ControllerBase
    {
        [HttpPost("/registration")]
        public async Task<ActionResult> Registration(AuthDTO authDTO)
        {
            if (diliveryContext.Donors.Contains(diliveryContext.Donors.FirstOrDefault(x => x.Login == authDTO.Login)))
                return BadRequest("The login is already being recalled");

            SHA256 sha256 = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(authDTO.Password);
            string hash = Convert.ToHexString(sha256.ComputeHash(bytes));

            Donor donor = new Donor() { Login = authDTO.Login, Password = hash };

            diliveryContext.Donors.Add(donor);
            await diliveryContext.SaveChangesAsync();

            return Ok(donor);
        }

        [HttpPost("/authorization")]
        public async Task<ActionResult> Authorization(AuthDTO authDTO)
        {
            SHA256 sha256 = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(authDTO.Password);
            string hash = Convert.ToHexString(sha256.ComputeHash(bytes));

            if (!diliveryContext.Donors.Contains(diliveryContext.Donors.FirstOrDefault(x => x.Login == authDTO.Login && x.Password == hash)))
                return BadRequest("The login is not already being recalled");

            var donor = diliveryContext.Donors.FirstOrDefault(x => x.Login == authDTO.Login && x.Password == hash);

            return Ok(donor);
        }
    }
}
