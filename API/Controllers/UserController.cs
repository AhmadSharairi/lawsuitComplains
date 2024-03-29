using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IComplaintRepository _complainRepo;
        private readonly IDemandRepository _demandRepository;
        private readonly IAttachmentRepository _attachmentRepo;
        private readonly IStringLocalizer<UserController> _localizer;
           private readonly ILocalizationService _localizationService;
        private readonly IMapper _mapper;


        public UserController(IUserRepository userRepository,
        IComplaintRepository complaintRepository,
         IDemandRepository demandRepository,
         IAttachmentRepository attachmentRepository,
         IStringLocalizer<UserController> localizer,
         ILocalizationService localizationService,
         IMapper mapper
         )
        {
            _userRepository = userRepository;
            _complainRepo = complaintRepository;
            _demandRepository = demandRepository;
            _attachmentRepo = attachmentRepository;
             _localizer = localizer;
              _localizationService = localizationService;
            _mapper = mapper;

        }

        [HttpGet("{{userId}}")]
        public IActionResult GetUser(int userId)
        {
            var user = _userRepository.GetUserById(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(user);
        }



        //1-This application allows a user to submit a complaint with demands as a free text. 
        //4-user insert his basic information like (name, number)
[HttpPost("submitComplaint")]
public async Task<IActionResult> submitComplaint([FromBody] ComplaintViewModel complaintViewModel)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    var user = new User
    {
        Name = complaintViewModel.Name,
        Number = complaintViewModel.Number
    };

    var complaint = new Complaint
    {
        Status = "Pending",
        SubmissionDate = DateTime.Now,
        User = user
    };

    foreach (var localizedText in complaintViewModel.ComplaintTexts)
    {
        var localizedComplaintText = new LocalizedText
        {
            LanguageCode = localizedText.LanguageCode,
            Text = localizedText.Text,
            Complaint = complaint
        };

        complaint.ComplaintTexts.Add(localizedComplaintText);
    }

    var demands = new List<Demand>();

    foreach (var demandDescription in complaintViewModel.DemandDescriptions)
    {
        var demand = new Demand
        {
            Description = demandDescription.Description,
            Complaint = complaint
        };
        demands.Add(demand);
    }

    await _userRepository.AddUser(user);
    await _complainRepo.CreateComplaintAsync(complaint);
    await _demandRepository.CreateDemandsAsync(demands);

    var successMessage = _localizationService.GetLocalizedString("ComplaintSubmitted");

    return Ok(new { Message = successMessage });
}



        //6-user can update edit complaints at any time.
        [HttpPut("updateComplaints")]
        public async Task<IActionResult> UpdateComplaint(int complaintId, ComplaintDto updatedComplaint)
        {
            var complaintToUpdate = _mapper.Map<Complaint>(updatedComplaint);
            var result = await _complainRepo.UpdateComplaintByIdAsync(complaintId, complaintToUpdate);

           var successMessage = _localizationService.GetLocalizedString("DemandUpdated");

            return result
                ? Ok(successMessage)
                : BadRequest("Failed to update complaint");
        }



        //3-user can attach a file (pdf) for official papers like Id
        [HttpPost("uploadPdf")]
        public async Task<IActionResult> UploadFile([FromForm] AttachmentDto fileToUploadId)
        {
            if (fileToUploadId.File == null || fileToUploadId.File.Length == 0)
                return BadRequest("Invalid file");


            using (var memoryStream = new MemoryStream())
            {
                await fileToUploadId.File.CopyToAsync(memoryStream);
                var fileBytes = memoryStream.ToArray();

                var attachment = new Attachment
                {
                    FileName = fileToUploadId.File.FileName,
                    FileContent = fileBytes,
                    UserId = fileToUploadId.UserId
                };

                await _attachmentRepo.AddAttachmentAsync(attachment);

                return Ok("Uploaded Succesfully");
            }
        }


      



    }
}