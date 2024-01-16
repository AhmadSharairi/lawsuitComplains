using API.DTOs;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{    
    
    
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IComplaintRepository _complainRepo;
        private readonly ComplaintContext _complaintContext;

        public AdminController(IComplaintRepository complainRepo, ComplaintContext complaintContext)
        {
            _complainRepo = complainRepo;
            _complaintContext = complaintContext;
        }


        //8-making a login functionality and returning all current users complaint (is a huge plus).
        [Authorize]
        [HttpGet("AllComplains")]
        public async Task<ActionResult<Complaint>> GetAllComplains()
        {
            if (await CheckUserRoleAsync("Admin"))
            {
                var complaints = await _complainRepo.GetComplaintAsync();

                if (complaints == null || complaints.Count == 0)
                {
                    return NotFound("No complaints found.");
                }

                return Ok(complaints);
            }
            else
            {

                return BadRequest("You Are Not the Admin");
            }
        }


        // 7- Admin approve/reject status depend on the compliantId.
        [Authorize]
        [HttpPut("UpdateStatus")]
        public async Task<IActionResult> UpdateComplaintStatus(int complaintId, [FromBody] StatusDto statusDto)
        {
            var complaints = await _complainRepo.GetComplaintAsync();

            if (statusDto.Status == "Accepted" || statusDto.Status == "Rejected" && (await CheckUserRoleAsync("Admin")))
            {
                var result = await _complainRepo.UpdateComplaintStatusAsync(complaintId, statusDto.Status);

                if (result)
                {
                    return Ok(new { Message = "Status updated successfully." });
                }


                return NotFound(new { Message = "Status update failed." });
            }


            return BadRequest(new { Message = "Invalid status value. Accepted or Rejected expected. Or You Are Not the Admin!" });
        }


        private async Task<bool> CheckUserRoleAsync(string roleName)
        {
            return await _complaintContext.UsersAuthentication.AnyAsync(u => u.Role == roleName);
        }

    }
}