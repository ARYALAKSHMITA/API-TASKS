using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using responseexample.DAL;
using responseexample.Models;

namespace responseexample.Controllers
{
    /// <summary>
    /// The UserV2Controller handles operations related to user management in version 2 of the API
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    [Produces("application/json")]

    public class UserV2Controller : ControllerBase
    {
        private readonly DatabaseHelper _databaseHelper;

        public UserV2Controller(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration), "Connection string 'DefaultConnection' not found.");
            _databaseHelper = new DatabaseHelper(connectionString);
        }

        //[HttpDelete("{id}")]
        //public void DeleteUser(int id)
        //{
        //    _databaseHelper.DeleteUser(id);
        //}

        //[HttpGet("message")]
        //public HttpResponseMessage GetCustomHttpResponse()
        //{
        //    return _databaseHelper.GetHttpResponseMessage();
        //}

        //[HttpGet("{id}")]
        //public IActionResult GetUser(int id)
        //{
        //    if (id <= 0)
        //    {
        //        return BadRequest("Invalid user ID.");
        //    }

        //    var user = _databaseHelper.GetUserByIdAsync(id).Result;

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(user);
        //}

        //[HttpGet("user/{id}")]
        //public ActionResult<User> GetUserById(int id)
        //{
        //    if (id <= 0)
        //    {
        //        return BadRequest("Invalid user ID.");
        //    }

        //    var user = _databaseHelper.GetUserByIdAsync(id).Result;

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return user; // Automatically returns 200 OK
        //}
        /// <summary>
        /// Retrieves all users whose names start with a specific letter
        /// </summary>
        /// <param name="letter">starting letter</param>
        /// <returns>list of users whose names start with specified letter.</returns>
        [HttpGet("startsWith/{letter}")]
        public async Task<IActionResult> GetUsersStartingWith(char letter)
        {
            var users = await _databaseHelper.GetAllUsersAsync();
            var filteredUsers = users.Where(u => u.Name.StartsWith(letter.ToString(), StringComparison.OrdinalIgnoreCase)).ToList();

            return Ok(filteredUsers);
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateUserAsync([FromBody] User user)
        //{
        //    if (string.IsNullOrEmpty(user.Name))
        //    {
        //        return BadRequest("User name is required.");
        //    }

        //    var isCreated = await _databaseHelper.CreateUserAsync(user);

        //    if (!isCreated)
        //    {
        //        return StatusCode(500, "A problem happened while handling your request.");
        //    }

        //    return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        //}
    }
}

