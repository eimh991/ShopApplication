using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.DTO;
using Shop.Interfaces;

namespace Shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopUpController : ControllerBase
    {
        private readonly ITopUpService _topUpService;
        public TopUpController(ITopUpService topUpService)
        {
            _topUpService = topUpService;
        }

        [HttpPost]
        public async Task<ActionResult<string>> CreateNewCode([FromBody] decimal amountValue, CancellationToken cancellationToken) 
        {
            var newCode = await _topUpService.CreateTopUpCodeAsync(amountValue, cancellationToken);
            if (newCode == null)
            {
                return BadRequest("Не получилось создать новый код");
            }
            
            return Ok(newCode);
        }

        [HttpPost("applyCode")]
        public async Task<ActionResult<bool>> ApplyTopUpCodeAsync([FromBody]TopUpCodeDTO dto, CancellationToken cancellationToken)
        {
            var applycode = await _topUpService.ApplyTopUpCodeAsync(dto.Code, dto.UserId, cancellationToken);

            if (!applycode)
            {
                return Ok("Данный код уже активирован, попробуйте ввести другой код");
            }
            return Ok("Ваш код активирован, средства зачислены на счет");
        }
        
    }
}
