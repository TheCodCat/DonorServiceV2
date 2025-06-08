using Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Models;

namespace DonorService.Controllers.Records
{
    [ApiController]
    [Route("[Controller]")]
    public class RecordersController(DiliveryContext diliveryContext) : ControllerBase
    {
        [HttpPost("/Addrecods")]
        [EndpointSummary("Запись на сдачу")]
        public async Task<ActionResult> RecordDilivery([FromHeader] int donorid, [FromHeader] int diliveryPointId, [FromBody] DateTime dateOnly)
        {
            try
            {
                var donor = diliveryContext.Donors.FirstOrDefault(x => x.Id == donorid);
                var point = diliveryContext.DiliveryPoints.FirstOrDefault(x => x.Id == diliveryPointId);
                Record record = new Record() { DiliveryPoint = point, Donor = donor, DateOnly = dateOnly };
                diliveryContext.Records.Add(record);
                await diliveryContext.SaveChangesAsync();

                return Ok("successful recording");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("/GetRecords")]
        [EndpointSummary("Получение всех записей")]
        public async Task<IEnumerable<Record>> GetAllRecords()
        {
            var records = diliveryContext.Records.
                Include(x => x.Donor)
                .Include(x => x.DiliveryPoint)
                .OrderBy(x => x.Id)
                .ToList();

            return records;
        }

        [HttpGet("/GetDonorRecords")]
        [EndpointSummary("получение записей донора")]
        [EndpointDescription("получение всех записей определенного донора")]
        public async Task<IEnumerable<Record>> GetDonorRecords([FromHeader] int donorId)
        {
            var records = diliveryContext.Records.Include(x => x.Donor).Include(x => x.DiliveryPoint).Where(x => x.DonorId == donorId);

            return records;
        }

        [HttpGet("/GetAllDilivery")]
        [EndpointSummary("Получение всех точек сбора крови")]
        public async Task<IEnumerable<DiliveryPoint>> GetAllDilivery()
        {
            try
            {
                return diliveryContext.DiliveryPoints.ToList();
            }
            catch
            {
                return new List<DiliveryPoint>();
            }
        }
    }
}
