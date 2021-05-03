using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccess.Models;
using AppCentroIdiomas.Models.Chat.UserChat;

namespace AppCentroIdiomas.Controllers.Chat
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatMessageController : ControllerBase
    {
        private readonly AppCentroEstudiosDBContext _context;

        public ChatMessageController(AppCentroEstudiosDBContext context)
        {
            _context = context;
        }

        // GET: api/ChatMessage
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChatMessage>>> GetChatMessages()
        {
            return await _context.ChatMessages.ToListAsync();
        }

        // GET: api/ChatMessage/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ChatMessage>> GetChatMessage(int id)
        {
            var chatMessage = await _context.ChatMessages.FindAsync(id);

            if (chatMessage == null)
            {
                return NotFound();
            }

            return chatMessage;
        }

        // PUT: api/ChatMessage/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChatMessage(int id, ChatMessage chatMessage)
        {
            if (id != chatMessage.Id)
            {
                return BadRequest();
            }

            _context.Entry(chatMessage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChatMessageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ChatMessage
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ChatMessage>> PostChatMessage(ChatMessageInDto chatMessage)
        {
            var _chatMessage = new ChatMessage
            {
                UserIdFrom = chatMessage.UserIdFrom,
                UserIdTo = chatMessage.UserIdTo,
                MessageContent = chatMessage.MessageContent,
                SentAt = DateTimeOffset.UtcNow,
                ReadAt = null
            };
            _context.ChatMessages.Add(_chatMessage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChatMessage", new { id = _chatMessage.Id }, _chatMessage);
        }

        //// POST: api/ChatMessage
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for
        //// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPost]
        //public async Task<ActionResult<ChatMessage>> PostChatMessage(ChatMessage chatMessage)
        //{
        //    _context.ChatMessages.Add(chatMessage);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetChatMessage", new { id = chatMessage.Id }, chatMessage);
        //}

        // DELETE: api/ChatMessage/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ChatMessage>> DeleteChatMessage(int id)
        {
            var chatMessage = await _context.ChatMessages.FindAsync(id);
            if (chatMessage == null)
            {
                return NotFound();
            }

            _context.ChatMessages.Remove(chatMessage);
            await _context.SaveChangesAsync();

            return chatMessage;
        }

        private bool ChatMessageExists(int id)
        {
            return _context.ChatMessages.Any(e => e.Id == id);
        }
    }
}
