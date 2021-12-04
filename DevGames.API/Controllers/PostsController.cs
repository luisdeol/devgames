using DevGames.API.Entities;
using DevGames.API.Models;
using DevGames.API.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DevGames.API.Controllers
{
    [Route("api/boards/{id}/posts")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostRepository repository;

        public PostsController(IPostRepository repository)
        {
            this.repository = repository;
        }

        // api/boards/1/posts
        [HttpGet]
        public IActionResult GetAll(int id)
        {
            var posts = repository.GetAllByBoard(id);

            return Ok(posts);
        }

        [HttpGet("{postId}")]
        public IActionResult GetById(int id, int postId)
        {
            var post = repository.GetById(postId);

            if (post == null)
                return NotFound();

            return Ok(post);
        }

        [HttpPost]
        public IActionResult Post(int id, AddPostInputModel model)
        {
            var post = new Post(model.Title, model.Description, id);

            repository.Add(post);

            return CreatedAtAction(nameof(GetById), new { id = post.Id, postId = post.Id }, model);
        }

        // api/boards/1/posts/1/comments POST
        [HttpPost("{postId}/comments")]
        public IActionResult PostComment(int id, int postId, AddCommentInputModel model)
        {
            var postExists = repository.PostExists(postId);

            if (!postExists)
                return NotFound();

            var comment = new Comment(model.Title, model.Description, model.User, postId);

            repository.AddComment(comment);

            return NoContent();
        }
    }
}
