using bePatientRegistration.Application.Patients.Dtos;
using bePatientRegistration.Application.Patients.Services;
using bePatientRegistration.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace bePatientRegistration.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientAppService _patientAppService;

        public PatientsController(IPatientAppService patientAppService)
        {
            _patientAppService = patientAppService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientDto>>> GetAll(CancellationToken cancellationToken)
        {
            var result = await _patientAppService.GetAllAsync(cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PatientDto>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var patient = await _patientAppService.GetByIdAsync(id, cancellationToken);
            if (patient is null)
                return NotFound();

            return Ok(patient);
        }

        [HttpPost]
        public async Task<ActionResult<PatientDto>> Create(
            [FromBody] CreatePatientRequest request,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var created = await _patientAppService.CreateAsync(request, cancellationToken);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (DomainException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<PatientDto>> Update(
            Guid id,
            [FromBody] UpdatePatientRequest request,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updated = await _patientAppService.UpdateAsync(id, request, cancellationToken);
                if (updated is null)
                    return NotFound();

                return Ok(updated);
            }
            catch (DomainException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _patientAppService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
