using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using responseexample.DAL;
using responseexample.Models;

namespace responseexample.Controllers
{
    /// <summary>
    /// The UserV1Controller handles operations related to user management in version 1 of the API
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    public class UserV1Controller : ControllerBase
    {
        private readonly DatabaseHelper _databaseHelper;
        /// <summary>
        /// Initializes a new instance of class
        /// </summary>
        /// <param name="configuration"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public UserV1Controller(IConfiguration configuration)
        {
            var connectionString=configuration.GetConnectionString("DefaultConnection")?? throw new ArgumentNullException(nameof(configuration),"Connection string 'DefaultConnection' not found.");
            _databaseHelper = new DatabaseHelper(connectionString);
        }

        // Example of void return type
        /// <summary>
        /// Deletes a user by their Id
        /// </summary>
        /// <param name="id">The Id of User to delete</param>
        [HttpDelete("{id}")]
        public void DeleteUser(int id)
        {
            _databaseHelper.DeleteUser(id);
        }

        // Example of HttpResponseMessage return type
        /// <summary>
        /// Retrieves a custom HTTP response message.
        /// </summary>
        /// <returns>message</returns>
        [HttpGet("message")]
        public HttpResponseMessage GetCustomHttpResponse()
        {
            return _databaseHelper.GetHttpResponseMessage();
        }

        // Example of IActionResult return type
        /// <summary>
        /// Retrieves a user by their Id
        /// </summary>
        /// <param name="id">The id of user</param>
        /// <returns>user with specified Id</returns>
        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid user ID.");
            }

            var user = _databaseHelper.GetUserByIdAsync(id).Result;

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // Example of ActionResult<T> return type
        /// <summary>
        ///  Retrieves a user by their Id
        /// </summary>
        /// <param name="id">user id</param>
        /// <returns></returns>
        [HttpGet("user/{id}")]
        public ActionResult<User> GetUserById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid user ID.");
            }

            var user = _databaseHelper.GetUserByIdAsync(id).Result;

            if (user == null)
            {
                return NotFound();
            }

            return user; // Automatically returns 200 OK
        }

        // Example of Task<IActionResult> return type
        /// <summary>
        /// creates a new user
        /// </summary>
        /// <param name="user">user object to create</param>
        /// <returns>newly created user.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /User
        ///     {
        ///        "id": 1,
        ///        "name": "jane"
        ///        
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns the newly created user</response>
        /// <response code="400">If the user is null</response>
        [HttpPost]
        public async Task<IActionResult> CreateUserAsync([FromBody] User user)
        {
            if (string.IsNullOrEmpty(user.Name))
            {
                return BadRequest("User name is required.");
            }

            var isCreated = await _databaseHelper.CreateUserAsync(user);

            if (!isCreated)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }
    }
}
