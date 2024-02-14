using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestRegisterAPI.Data;
using TestRegisterAPI.Model;
using TestRegisterAPI.Services;


namespace TestRegisterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ErrorResponseService _errorResponseService;

        public RegisterController(ApplicationDbContext context, ErrorResponseService errorResponseService)
        {
            _context = context;
            _errorResponseService = errorResponseService;
        }

        //POST: api/Register
        [HttpPost]
        public async Task<ActionResult<Register>> userRegister(Register register)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    
                    //If Email already exist
                    var existEmail = await _context.Register.FirstOrDefaultAsync(x => x.Email == register.Email);
                    if (existEmail != null)
                    {
                        var errorResponse = _errorResponseService.CreateErrorResponse(400, "Email Already Exist!");
                        return BadRequest(errorResponse);
                    }

                    _context.Register.Add(register);
                    await _context.SaveChangesAsync();

                    //Success Response
                    var response = new
                    {
                        Status = 200,
                        Message = "Registration Successful",
                        Data = register,
                    };
                    return Created("", response);

                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                var errorResponse = _errorResponseService.CreateErrorResponse(500,"Internal Server Error!!");
                return StatusCode(500, errorResponse);
            }
        }
    }
}
