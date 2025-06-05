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
        public async Task<ActionResult> RecordDilivery([FromHeader] int donorid, [FromHeader] int diliveryPointId)
        {
            var donor = diliveryContext.Donors.FirstOrDefault(x => x.Id == donorid);
            var point = diliveryContext.DiliveryPoints.FirstOrDefault(x => x.Id == diliveryPointId);
            Record record = new Record() { DiliveryPoint = point, Donor = donor };
            diliveryContext.Records.Add(record);
            await diliveryContext.SaveChangesAsync();

            return Ok("successful recording");
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

        [HttpPost("/GetDonorRecords")]
        [EndpointSummary("получение записей донора")]
        [EndpointDescription("получение всех записей определенного донора")]
        public async Task<IEnumerable<Record>> GetDonorRecords([FromHeader] int donorId)
        {
            var records = diliveryContext.Records.Include(x => x.Donor).Include(x => x.DiliveryPoint).Where(x => x.DonorId == donorId);

            return records;
        }
    }
}
