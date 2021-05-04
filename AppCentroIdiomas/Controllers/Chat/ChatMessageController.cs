using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccess.Models;
using AppCentroIdiomas.Models.Chat.UserChat;
using System.Data.Common;
using System.Reflection;
using Microsoft.Data.SqlClient;
using System.Data;
using AppCentroIdiomas.Models;
using AppCentroIdiomas.Models.Chat;

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


        // GET: api/ChatMessage/5
        [HttpGet("GetMessagesByUserId/{userId}")]
        public async Task<ActionResult<AvailableUsers>> GetMessagesByUserId(int userId)
        {
            var result = new AvailableUsers();

            using (SqlConnection conn = new SqlConnection(_context.Database.GetConnectionString()))
            {
                conn.Open();

                var query =  @$"
                                     SELECT AvailableUsers.UserIdTo, 
                                            AvailableUsers.DisplayNameTo, 
                                            CM.LastMessageContent, 
                                            CM.LastMessageSentAt, 
                                            AvailableUsers.RoleTo
                                    FROM
                                    (
                                        SELECT DISTINCT 
                                                CM.UserIdTo AS UserIdTo, 
                                                concat(UserInformationTo.FirstName, ' ', UserInformationTo.LastName) AS [DisplayNameTo], 
                                                'Docente' AS RoleTo
                                        FROM ChatMessage AS CM
                                                INNER JOIN [User] AS UserFrom ON CM.UserIdFrom = UserFrom.Id
                                                INNER JOIN [UserInformation] AS UserInformationFrom ON UserInformationFrom.Id = UserFrom.UserInformationId
                                                INNER JOIN [User] AS UserTo ON CM.UserIdTo = UserTo.Id
                                                INNER JOIN [UserInformation] AS UserInformationTo ON UserInformationTo.Id = UserTo.UserInformationId
                                        WHERE UserIdFrom = @userId
                                    ) AS AvailableUsers
                                    INNER JOIN
                                    (
                                        SELECT *
                                        FROM
                                        (
                                            SELECT ROW_NUMBER() OVER(PARTITION BY UserIdTo
                                                    ORDER BY SentAt DESC) AS RowNumber, 
                                                    Id, 
                                                    UserIdFrom, 
                                                    UserIdTo, 
                                                    MessageContent AS LastMessageContent, 
                                                    SentAt AS LastMessageSentAt, 
                                                    ReadAt
                                            FROM ChatMessage
                                            WHERE ChatMessage.UserIdFrom = @userId
                                        ) AS tb1
                                        WHERE RowNumber = 1
                                    ) AS CM ON AvailableUsers.UserIdTo = CM.UserIdTo
                        ";
                // 1.  create a command object identifying the stored procedure
                var command = new SqlCommand(query, conn);
                command.Parameters.Add("@userId", SqlDbType.Int);
                command.Parameters["@userId"].Value = userId;


                // 2. set the command object so it knows to execute a stored procedure
                command.CommandType = CommandType.Text;

                // execute the command
                using (var rdr = command.ExecuteReader())
                {
                    //3. Loop through rows
                    while (rdr.Read())
                    {
                        //Get each column
                        result.AvailableUsersList.Add(
                            new AvailableUser { 
                                UserIdTo = rdr.GetInt32(0), 
                                DisplayNameTo = rdr.GetString(1),
                                LastMessageContent = rdr.GetString(2),
                                LastMessageSentAt = rdr.GetDateTimeOffset(3).ToString("yyyy/MM/dd HH:mm:ss"),
                                RoleTo = rdr.GetString(4), 
                            });
                    }
                }
            }

            return Ok(new { availableUsers = result.AvailableUsersList });



            //if (chatMessage == null)
            //{
            //    return NotFound();
            //}

            //return chatMessage;
        }

        public class _HistoryChat {
            public string HistoryMessages { get; set; }
        }

        // GET: api/ChatMessage/5
        [HttpGet("GetHistoryChat/{userIdFrom}/{userIdTo}")]
        public async Task<ActionResult<_HistoryChat>> GetHistoryChat(int userIdFrom, int userIdTo)
        {
            var historyMessages = new List<HistoryChatDTO>();
            var historyChat = new _HistoryChat();

            using (SqlConnection conn = new SqlConnection(_context.Database.GetConnectionString()))
            {
                conn.Open();

                var query = @$"
                            SELECT UserIdTo, 
                                   DisplayNameTo, 
                                   MessageContent, 
                                   SentAt, 
                                   ReadAt, 
                                   CONVERT(BIT, IsRead) AS IsRead, 
                                   CONVERT(BIT, IIF(UserIdTo = @UserIdTo, 1, 0)) AS IsSent
                            FROM
                            (
                                SELECT CM.UserIdTo AS UserIdTo, 
                                       concat(UserInformationTo.FirstName, ' ', UserInformationTo.LastName) AS DisplayNameTo, 
                                       CM.MessageContent AS MessageContent, 
                                       SentAt AS SentAt, 
                                       ReadAt AS ReadAt, 
                                       IIF(ReadAt IS NULL, 0, 1) AS IsRead
                                FROM ChatMessage AS CM
                                     INNER JOIN [User] AS UserFrom ON CM.UserIdFrom = UserFrom.Id
                                     INNER JOIN [UserInformation] AS UserInformationFrom ON UserInformationFrom.Id = UserFrom.UserInformationId
                                     INNER JOIN [User] AS UserTo ON CM.UserIdTo = UserTo.Id
                                     INNER JOIN [UserInformation] AS UserInformationTo ON UserInformationTo.Id = UserTo.UserInformationId
                                WHERE UserIdFrom = @userIdFrom
                                      AND UserIdTo = @userIdTo
                                UNION ALL
                                SELECT CM.UserIdTo AS UserIdTo, 
                                       concat(UserInformationTo.FirstName, ' ', UserInformationTo.LastName) AS [DisplayNameTo], 
                                       CM.MessageContent, 
                                       SentAt, 
                                       ReadAt, 
                                       IIF(ReadAt IS NULL, 0, 1) AS IsRead
                                FROM ChatMessage AS CM
                                     INNER JOIN [User] AS UserFrom ON CM.UserIdFrom = UserFrom.Id
                                     INNER JOIN [UserInformation] AS UserInformationFrom ON UserInformationFrom.Id = UserFrom.UserInformationId
                                     INNER JOIN [User] AS UserTo ON CM.UserIdTo = UserTo.Id
                                     INNER JOIN [UserInformation] AS UserInformationTo ON UserInformationTo.Id = UserTo.UserInformationId
                                WHERE UserIdFrom = @userIdTo
                                      AND UserIdTo = @userIdFrom
                            ) AS CompleteHistoryChat
                            ORDER BY CompleteHistoryChat.SentAt ASC                                     

                        ";
                // 1.  create a command object identifying the stored procedure
                var command = new SqlCommand(query, conn);
                command.Parameters.Add("@userIdFrom", SqlDbType.Int);
                command.Parameters["@userIdFrom"].Value = userIdFrom;

                command.Parameters.Add("@userIdTo", SqlDbType.Int);
                command.Parameters["@userIdTo"].Value = userIdTo ;


                // 2. set the command object so it knows to execute a stored procedure
                command.CommandType = CommandType.Text;

                // execute the command
                using (var rdr = command.ExecuteReader())
                {
                    //3. Loop through rows
                    while (rdr.Read())
                    {
                        //Get each column
                        historyMessages.Add(
                            new HistoryChatDTO
                            {
                                UserIdTo = rdr.GetInt32(0),
                                DisplayNameTo = rdr.GetString(1),
                                MessageContent = rdr.GetString(2),
                                SentAt = rdr.GetDateTimeOffset(3).ToString("yyyy/MM/dd HH:mm:ss"),
                                //ReadAt = rdr.GetDateTimeOffset(4).ToString("yyyy/MM/dd HH:mm:ss"),
                                ReadAt = rdr.GetNullableDateTimeOffset("ReadAt")?.ToString("yyyy/MM/dd HH:mm:ss"),
                                IsRead = rdr.GetBoolean(5),
                                IsSent = rdr.GetBoolean(6)
                            });
                    }
                }
            }

            foreach (var item in historyMessages)
            {
                if (item.IsSent)
                {
                    historyChat.HistoryMessages += @$"Mensaje enviado por mi a las {item.SentAt} {Environment.NewLine}Mensaje: {item.MessageContent}";
                }
                else {
                    historyChat.HistoryMessages += @$"Enviado por {item.DisplayNameTo} a las {item.SentAt} {Environment.NewLine}Mensaje: {item.MessageContent}";
                }
                historyChat.HistoryMessages += @$"{Environment.NewLine}";
                historyChat.HistoryMessages += @$"{Environment.NewLine}";
                historyChat.HistoryMessages += @$"{Environment.NewLine}";
            }

            return historyChat;
        }

        // GET: api/ChatMessage/5
        [HttpGet("GetHistoryChatv2/{userIdFrom}/{userIdTo}")]
        public async Task<ActionResult<AvailableUsers>> GetHistoryChatv2(int userIdFrom, int userIdTo)
        {
            var historyMessages = new List<HistoryChatDTO>();

            using (SqlConnection conn = new SqlConnection(_context.Database.GetConnectionString()))
            {
                conn.Open();

                var query = @$"
                            SELECT UserIdTo, 
                                   DisplayNameTo, 
                                   MessageContent, 
                                   SentAt, 
                                   ReadAt, 
                                   CONVERT(BIT, IsRead) AS IsRead, 
                                   CONVERT(BIT, IIF(UserIdTo = @UserIdTo, 1, 0)) AS IsSent
                            FROM
                            (
                                SELECT CM.UserIdTo AS UserIdTo, 
                                       concat(UserInformationTo.FirstName, ' ', UserInformationTo.LastName) AS DisplayNameTo, 
                                       CM.MessageContent AS MessageContent, 
                                       SentAt AS SentAt, 
                                       ReadAt AS ReadAt, 
                                       IIF(ReadAt IS NULL, 0, 1) AS IsRead
                                FROM ChatMessage AS CM
                                     INNER JOIN [User] AS UserFrom ON CM.UserIdFrom = UserFrom.Id
                                     INNER JOIN [UserInformation] AS UserInformationFrom ON UserInformationFrom.Id = UserFrom.UserInformationId
                                     INNER JOIN [User] AS UserTo ON CM.UserIdTo = UserTo.Id
                                     INNER JOIN [UserInformation] AS UserInformationTo ON UserInformationTo.Id = UserTo.UserInformationId
                                WHERE UserIdFrom = @userIdFrom
                                      AND UserIdTo = @userIdTo
                                UNION ALL
                                SELECT CM.UserIdTo AS UserIdTo, 
                                       concat(UserInformationTo.FirstName, ' ', UserInformationTo.LastName) AS [DisplayNameTo], 
                                       CM.MessageContent, 
                                       SentAt, 
                                       ReadAt, 
                                       IIF(ReadAt IS NULL, 0, 1) AS IsRead
                                FROM ChatMessage AS CM
                                     INNER JOIN [User] AS UserFrom ON CM.UserIdFrom = UserFrom.Id
                                     INNER JOIN [UserInformation] AS UserInformationFrom ON UserInformationFrom.Id = UserFrom.UserInformationId
                                     INNER JOIN [User] AS UserTo ON CM.UserIdTo = UserTo.Id
                                     INNER JOIN [UserInformation] AS UserInformationTo ON UserInformationTo.Id = UserTo.UserInformationId
                                WHERE UserIdFrom = @userIdTo
                                      AND UserIdTo = @userIdFrom
                            ) AS CompleteHistoryChat
                            ORDER BY CompleteHistoryChat.SentAt ASC                                     

                        ";
                // 1.  create a command object identifying the stored procedure
                var command = new SqlCommand(query, conn);
                command.Parameters.Add("@userIdFrom", SqlDbType.Int);
                command.Parameters["@userIdFrom"].Value = userIdFrom;

                command.Parameters.Add("@userIdTo", SqlDbType.Int);
                command.Parameters["@userIdTo"].Value = userIdTo;


                // 2. set the command object so it knows to execute a stored procedure
                command.CommandType = CommandType.Text;

                // execute the command
                using (var rdr = command.ExecuteReader())
                {
                    //3. Loop through rows
                    while (rdr.Read())
                    {
                        //Get each column
                        historyMessages.Add(
                            new HistoryChatDTO
                            {
                                UserIdTo = rdr.GetInt32(0),
                                DisplayNameTo = rdr.GetString(1),
                                MessageContent = rdr.GetString(2),
                                SentAt = rdr.GetDateTimeOffset(3).ToString("yyyy/MM/dd HH:mm:ss"),
                                //ReadAt = rdr.GetDateTimeOffset(4).ToString("yyyy/MM/dd HH:mm:ss"),
                                ReadAt = rdr.GetNullableDateTimeOffset("ReadAt")?.ToString("yyyy/MM/dd HH:mm:ss"),
                                IsRead = rdr.GetBoolean(5),
                                IsSent = rdr.GetBoolean(6)
                            });
                    }
                }
            }

            return Ok(new { historyMessages });
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
