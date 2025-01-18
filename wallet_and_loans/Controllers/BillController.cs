using Microsoft.AspNetCore.Mvc;
using wallet_and_loans_api.IServices;
using wallet_and_loans_api.Model.DTO.BillDTO;
using wallet_and_loans_api.Services;
using wallet_and_loans_components.Logics;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace wallet_and_loans_api.Controllers
{
    [Route("api/bills")]
    [ApiController]
    public class BillController : ControllerBase
    {
        private IBillService _billService;

        public BillController(IBillService billService)
        {
            _billService = billService;
        }

        // GET: api/bills
        [HttpGet]
        public IActionResult GetBills()
        {
            try
            {
                var bills = _billService.GetBills();
                return Ok(bills);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/bills/{billId}
        [HttpGet("{id}")]
        public IActionResult GetBillByID(int id)
        {
            try
            {
                var bill = _billService.GetBillByID(id);
                return Ok(bill);
            } 
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<BillController>
        [HttpPost("create")]
        public IActionResult CreateBill([FromBody] CreateBillDTO dto)
        {
            try
            {
                _billService.CreateBill(dto, out Bill bill);
                return Created($"/api/bills/{bill.ID}", bill);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
/*
        // PUT api/<BillController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<BillController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }*/
    }
}
