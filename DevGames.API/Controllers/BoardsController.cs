using AutoMapper;
using DevGames.API.Entities;
using DevGames.API.Models;
using DevGames.API.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace DevGames.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IBoardRepository repository;
        public BoardsController(IMapper mapper, IBoardRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var boards = repository.GetAll();

            Log.Information($"{boards.Count()} boards retrieved.");

            return Ok(boards);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var board = repository.GetById(id);

            if (board == null)
                return NotFound();

            return Ok(board);
        }

        /// <summary>
        /// Post Board
        /// </summary>
        /// <remarks>
        /// Request Body Example:
        /// {
        /// "gameTitle": "Starcraft 2",
        /// "description": "A strategy game",
        /// "rules": "1. No SPAM."
        /// }
        /// </remarks>
        /// <param name="model">Board data</param>
        /// <returns>Created object</returns>
        /// <response code="201">Success</response>
        /// <response code="400">Invalid data</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Post(AddBoardInputModel model)
        {
            var board = mapper.Map<Board>(model);

            repository.Add(board);

            return CreatedAtAction("GetById", new { id = board.Id }, model);
        }

        // PUT api/boards/1
        [HttpPut("{id}")]
        public IActionResult Put(int id, UpdateBoardInputModel model)
        {
            var board = repository.GetById(id);

            if (board == null)
                return NotFound();

            board.Update(model.Description, model.Rules);

            repository.Update(board);

            return NoContent();
        }
    }
}
