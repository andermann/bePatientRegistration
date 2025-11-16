using bePatientRegistration.Application.Patients.Dtos;
using bePatientRegistration.Application.Patients.Services;
using bePatientRegistration.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace bePatientRegistration.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthPlansController : ControllerBase
    {
        private readonly IHealthPlanAppService _healthPlanAppService;

        public HealthPlansController(IHealthPlanAppService healthPlanAppService)
        {
            _healthPlanAppService = healthPlanAppService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<HealthPlanDto>>> GetAll(CancellationToken cancellationToken)
        {
            var result = await _healthPlanAppService.GetAllAsync(cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<HealthPlanDto>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var item = await _healthPlanAppService.GetByIdAsync(id, cancellationToken);
            if (item is null)
                return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<HealthPlanDto>> Create(
            [FromBody] CreateHealthPlanRequest request,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var created = await _healthPlanAppService.CreateAsync(request, cancellationToken);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (DomainException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<HealthPlanDto>> Update(
            Guid id,
            [FromBody] UpdateHealthPlanRequest request,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updated = await _healthPlanAppService.UpdateAsync(id, request, cancellationToken);
                if (updated is null)
                    return NotFound();

                return Ok(updated);
            }
            catch (DomainException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
