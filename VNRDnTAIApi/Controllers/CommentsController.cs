﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObjectLibrary;
using DataAccessLibrary.Business_Entity;
using DataAccessLibrary.Interfaces;

namespace VNRDnTAIApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]

    public class CommentsController : ControllerBase
    {
        private readonly CommentBusinessEntity _entity;

        public CommentsController(IUnitOfWork work)
        {
            _entity = new CommentBusinessEntity(work);
        }

        // GET: api/Comments
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Comment>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
        {
            try
            {
                return StatusCode(200, await _entity.GetCommentsAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/Comments/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Comment), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Comment>> GetComment(Guid id)
        {
            try
            {
                return StatusCode(200, await _entity.GetCommentAsync(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/Comments/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Comment), 200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PutComment(Guid id, Comment comment)
        {
            if (id != comment.Id)
            {
                return BadRequest();
            }

            try
            {
                return StatusCode(200, await _entity.UpdateComment(comment));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/Comments
        [HttpPost]
        [ProducesResponseType(typeof(Comment), 201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Comment>> PostComment(Comment comment)
        {
            try
            {
                return StatusCode(201, await _entity.AddComment(comment));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/Comments/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            try
            {
                await _entity.RemoveComment(id);
                return StatusCode(204);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
