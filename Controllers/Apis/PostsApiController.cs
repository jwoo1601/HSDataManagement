using HyosungManagement.Data;
using HyosungManagement.Filters;
using HyosungManagement.InputModels;
using HyosungManagement.Logging;
using HyosungManagement.Models;
using HyosungManagement.Models.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace HyosungManagement.Controllers.Apis
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/posts")]
    [ApiController]
    public class PostsApiController : ApiControllerBase
    {
        AppDbContext Context { get; }
        ILogger<Post> Logger { get; }
        UserManager<HSMUser> UserManager { get; }
        RoleManager<HSMRole> RoleManager { get; }
        IStringLocalizer<PostsApiController> Localizer { get; }

        public PostsApiController(
            AppDbContext context,
            ILogger<Post> logger,
            UserManager<HSMUser> userManager,
            RoleManager<HSMRole> roleManager,
            IStringLocalizer<PostsApiController> localizer
        )
        {
            Context = context;
            Logger = logger;
            UserManager = userManager;
            RoleManager = roleManager;
            Localizer = localizer;
        }

        // GET /api/posts
        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPostsAsync()
        {
            var list = await Context.Posts.ToListAsync();

            Logger.LogInformation(
                PostsApiLogEvents.GetAllPosts,
                "Successfully retrieved posts - num: {num}.",
                list.Count
            );

            return Ok(list);
        }


        // GET /api/posts/{id}
        [HttpGet("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPostByIDAsync(
            int id
        )
        {
            var post = await Context.Posts.SingleOrDefaultAsync(p => p.ID.Equals(id));
            if (post == null)
            {
                Logger.LogError(
                    PostsApiLogEvents.PostNotFound,
                    "Failed to retrieve post {ID}: post not found.",
                    id
                );

                return NotFound(Localizer["post.notFound"]);
            }

            Logger.LogInformation(
                PostsApiLogEvents.GetPostByID,
                "Successfully retrieved post {@Post}",
                post
            );

            return Ok(post);
        }
    }
}
