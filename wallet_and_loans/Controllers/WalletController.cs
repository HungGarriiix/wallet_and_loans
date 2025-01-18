using Microsoft.AspNetCore.Mvc;
using wallet_and_loans_api.IServices;
using wallet_and_loans_api.Model.DTO.WalletDTO;
using wallet_and_loans_components.Logics;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace wallet_and_loans_api.Controllers
{
    [Route("api/wallets")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        // GET: api/wallets
        [HttpGet]
        public IActionResult GetWallets()
        {
            try
            {
                var wallets = _walletService.GetWallets();
                return Ok(wallets);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/wallets/:id
        [HttpGet("{id}")]
        public IActionResult GetWalletByID(int id)
        {
            try
            {
                var wallet = _walletService.GetWalletByID(id);
                return Ok(wallet);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/wallets/add
        [HttpPost("add")]
        public IActionResult AddWallet([FromBody] CreateWalletDTO dto)
        {
            try
            {
                _walletService.AddWallet(dto, out Wallet wallet);
                return Created($"/api/wallets/{wallet.ID}", wallet);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PATCH api/wallets/:id
        [HttpPatch("{id}")]
        public IActionResult UpdateWallet(int id, [FromBody] UpdateWalletDTO dto)
        {
            try
            {
                _walletService.UpdateWallet(id, dto, out Wallet result);
                return Ok(result);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<WalletController>/5
        // to be continued
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Unauthorized();
        }
    }
}
