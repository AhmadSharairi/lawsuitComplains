using API.DTOs;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace API.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IComplaintRepository _complainRepo;
        private readonly ComplaintContext _complaintContext;
        private readonly IStringLocalizer<AdminController> _localizer;

        public AdminController(IComplaintRepository complainRepo, ComplaintContext complaintContext, IStringLocalizer<AdminController> localizer)
        {
            _complainRepo = complainRepo;
            _complaintContext = complaintContext;
            _localizer = localizer;
        }


        //8-making a login functionality and returning all current users complaint (is a huge plus).
        [Authorize]
        [HttpGet("AllComplains")]
        public async Task<ActionResult<Complaint>> GetAllComplains()
        {
            if (await CheckUserRoleAsync("Admin"))
            {
                var complaints = await _complainRepo.GetComplaintsAsync();

                if (complaints == null || complaints.Count == 0)
                {
                    var notFoundMessage = _localizer["NoComplaintsFound"].Value;
                    return NotFound(notFoundMessage);
                }

                return Ok(complaints);
            }
            else
            {

                var notAdminMessage = _localizer["NotAdminError"].Value;
                return BadRequest(notAdminMessage);
            }
        }


        // 7- Admin approve/reject status depend on the compliantId.
        [Authorize]
        [HttpPut("UpdateStatus")]
        public async Task<IActionResult> UpdateComplaintStatus(int complaintId, [FromBody] StatusDto statusDto)
        {
            var complaints = await _complainRepo.GetComplaintsAsync();

            if ((statusDto.Status == "Accepted" || statusDto.Status == "Rejected") && (await CheckUserRoleAsync("Admin")))
            {
                var result = await _complainRepo.UpdateComplaintStatusAsync(complaintId, statusDto.Status);

                if (result)
                {
                    var successMessage = _localizer[statusDto.MessageKey].Value;
                    return Ok(new { Message = successMessage });
                }

                var failureMessage = _localizer[statusDto.MessageKey].Value;
                return NotFound(new { Message = failureMessage });
            }

            var invalidStatusMessage = _localizer[statusDto.MessageKey].Value;
            return BadRequest(new { Message = invalidStatusMessage });
        }


        private async Task<bool> CheckUserRoleAsync(string roleName)
        {
            return await _complaintContext.UsersAuthentication.AnyAsync(u => u.Role == roleName);
        }

    }
}