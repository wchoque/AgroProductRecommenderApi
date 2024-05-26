using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AgroProductRecommenderApi.Models.Chat;
using AgroProductRecommenderApi.Models.Chat.ProductChat;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace AgroProductRecommenderApi.Controllers.Chat
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductChatMessageController : ControllerBase
    {
        private readonly AgroProductRecommenderDBContext _context;

        public ProductChatMessageController(AgroProductRecommenderDBContext context)
        {
            _context = context;
        }

        // GET: api/ProductChatMessage/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductChatMessage>> GetProductChatMessage(int id)
        {
            var chatMessage = await _context.ProductChatMessages.FindAsync(id);

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
            var result = GetAvailableUsers(userId);

            foreach (var availableUser in result.AvailableUsersList)
            {
                var imageUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{Url.Action("GetProfilePicture", "User", new { id = availableUser.UserIdTo })}";
                availableUser.ImageUrl = imageUrl;
            }
            return Ok(new { availableUsers = result.AvailableUsersList });
        }

        public class _HistoryChat
        {
            public string HistoryMessages { get; set; }
        }

        // GET: api/ChatMessage/5
        [HttpGet("GetHistoryChat/{userIdFrom}/{userIdTo}")]
        public async Task<ActionResult<_HistoryChat>> GetHistoryChat(int userIdFrom, int userIdTo)
        {
            var historyChat = new _HistoryChat();
            var historyMessages = GetFullHistoryChat(userIdFrom, userIdTo);

            foreach (var item in historyMessages)
            {
                if (item.IsSent)
                {
                    historyChat.HistoryMessages += @$"Mensaje enviado por mi a las {item.SentAt} {Environment.NewLine}Mensaje: {item.MessageContent}";
                }
                else
                {
                    historyChat.HistoryMessages += @$"Enviado por {item.DisplayNameTo} a las {item.SentAt} {Environment.NewLine}Mensaje: {item.MessageContent}";
                }
                historyChat.HistoryMessages += @$"{Environment.NewLine}";
                historyChat.HistoryMessages += @$"{Environment.NewLine}";
                historyChat.HistoryMessages += @$"{Environment.NewLine}";
            }

            return historyChat;
        }

        private List<HistoryChatDTO> GetFullHistoryChat(int userIdFrom, int userIdTo)
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
                                 FROM ProductChatMessage AS CM
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
                                 FROM ProductChatMessage AS CM
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

            return historyMessages;
        }

        // GET: api/ChatMessage/5
        [HttpGet("GetMessageByUser/{userId}/{productChatMessageId}")]
        public async Task<ActionResult<AvailableUser>> GetMessageByUser(int userId, int productChatMessageId)
        {
            var productChatMessage = await _context.ProductChatMessages.FindAsync(productChatMessageId);
            if (productChatMessage == null)
            {
                return NotFound();
            }

            var result = GetAvailableUsers(userId);
            var availableUserResult = result.FirstOrDefault(x => x.UserIdTo == productChatMessage.UserIdFrom);

            var imageUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{Url.Action("GetProfilePicture", "User", new { id = availableUserResult.UserIdTo })}";
            availableUserResult.ImageUrl = imageUrl;

            return Ok(availableUserResult);
        }


        [HttpGet("GetMessages/{userIdFrom}/{userIdTo}")]
        public async Task<IActionResult> GetMessages(int userIdFrom, int userIdTo)
        {
            var fromUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == userIdFrom);
            if (fromUser == null)
            {
                return NotFound("From User not found");
            }

            var toUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == userIdTo);
            if (toUser == null)
            {
                return NotFound("To User not found");
            }

            // Obtener los mensajes entre los usuarios
            var chats = await _context.ProductChatMessages
                .Where(x => (x.UserIdFrom == userIdFrom && x.UserIdTo == userIdTo) || (x.UserIdFrom == userIdTo && x.UserIdTo == userIdFrom))
                .OrderByDescending(x => x.SentAt)
                .ToListAsync();

            // Formatear los mensajes para la respuesta
            var response = chats.Select(chat => new
            {
                message = chat.MessageContent,
                senderId = chat.UserIdFrom,
                timestamp = chat.SentAt.ToString("yyyy-MM-ddTHH:mm:ss.fffffffzzz")
            }).ToList();

            return Ok(response);
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
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutChatMessage(int id, ChatMessage chatMessage)
        //{
        //    if (id != chatMessage.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(chatMessage).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ChatMessageExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/ChatMessage
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ChatMessage>> PostChatMessage(ProductChatMessageInDto message)
        {
            var productChatMessage = new ProductChatMessage
            {
                UserIdFrom = message.UserIdFrom,
                UserIdTo = message.UserIdTo,
                ProductId = message.ProductId,
                MessageContent = message.MessageContent,
                SentAt = DateTimeOffset.UtcNow,
                ReadAt = null
            };

            await _context.ProductChatMessages.AddAsync(productChatMessage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductChatMessage", new { id = productChatMessage.Id }, productChatMessage);
        }

        private AvailableUsers GetAvailableUsers(int userId)
        {
            var result = new AvailableUsers();
            using (SqlConnection conn = new SqlConnection(_context.Database.GetConnectionString()))
            {
                conn.Open();

                var query = @$"
                                SELECT DISTINCT
                                    COALESCE(Sent.UserIdTo, Received.UserIdFrom, SentResponded.UserIdTo, ReceivedResponded.UserIdFrom) AS UserIdTo,
                                    COALESCE(Sent.DisplayName, Received.DisplayName, SentResponded.DisplayName, ReceivedResponded.DisplayName) AS DisplayNameTo,
                                    COALESCE(Sent.LastMessage, Received.LastMessage, SentResponded.LastMessage, ReceivedResponded.LastMessage) AS LastMessageContent,
                                    COALESCE(Sent.LastMessageSentAt, Received.LastMessageSentAt, SentResponded.LastMessageSentAt, ReceivedResponded.LastMessageSentAt) AS LastMessageSentAt,
                                    COALESCE(Sent.Role, Received.Role, SentResponded.Role, ReceivedResponded.Role) AS RoleTo
                                FROM
                                    -- Sent, not responded
                                    (SELECT 
                                        UserIdTo, 
                                        CONCAT(UserInformationTo.FirstName, ' ', UserInformationTo.LastName) AS DisplayName,
                                        UserTypeTo.Name AS Role,
                                        MessageContent AS LastMessage, 
                                        SentAt AS LastMessageSentAt
                                    FROM ProductChatMessage
                                    INNER JOIN [User] AS UserTo ON UserIdTo = UserTo.Id
                                    INNER JOIN [UserInformation] AS UserInformationTo ON UserTo.UserInformationId = UserInformationTo.Id
	                                INNER JOIN [UserByType] AS UseByTypeTo ON UseByTypeTo.UserId = UserTo.Id
                                    INNER JOIN [UserType] AS UserTypeTo ON UserTypeTo.Id = UseByTypeTo.UserTypeId
                                    WHERE UserIdFrom = @UserIdFrom
                                    AND NOT EXISTS (
                                        SELECT 1 FROM ProductChatMessage WHERE UserIdFrom = UserIdTo AND UserIdTo = @UserIdFrom
                                    )
                                    ) AS Sent
                                FULL OUTER JOIN
                                    -- Received, not responded
                                    (SELECT 
                                        UserIdFrom, 
                                        CONCAT(UserInformationFrom.FirstName, ' ', UserInformationFrom.LastName) AS DisplayName,
                                        UserTypeFrom.Name AS Role,
                                        MessageContent AS LastMessage, 
                                        SentAt AS LastMessageSentAt
                                    FROM ProductChatMessage
                                    INNER JOIN [User] AS UserFrom ON UserIdFrom = UserFrom.Id
                                    INNER JOIN [UserInformation] AS UserInformationFrom ON UserFrom.UserInformationId = UserInformationFrom.Id
                                    INNER JOIN [UserByType] AS UseByTypeFrom ON UseByTypeFrom.UserId = UserFrom.Id
                                    INNER JOIN [UserType] AS UserTypeFrom ON UserTypeFrom.Id = UseByTypeFrom.UserTypeId
                                    WHERE UserIdTo = @UserIdFrom
                                    AND NOT EXISTS (
                                        SELECT 1 FROM ProductChatMessage WHERE UserIdFrom = @UserIdFrom AND UserIdTo = UserIdFrom
                                    )
                                    ) AS Received ON Sent.UserIdTo = Received.UserIdFrom
                                FULL OUTER JOIN
                                    -- Sent, responded
                                    (SELECT 
                                        UserIdTo, 
                                        CONCAT(UserInformationTo.FirstName, ' ', UserInformationTo.LastName) AS DisplayName,
                                        UserTypeTo.Name AS Role,
                                        MessageContent AS LastMessage, 
                                        SentAt AS LastMessageSentAt
                                    FROM ProductChatMessage
                                    INNER JOIN [User] AS UserTo ON UserIdTo = UserTo.Id
                                    INNER JOIN [UserInformation] AS UserInformationTo ON UserTo.UserInformationId = UserInformationTo.Id
	                                INNER JOIN [UserByType] AS UseByTypeTo ON UseByTypeTo.UserId = UserTo.Id
                                    INNER JOIN [UserType] AS UserTypeTo ON UserTypeTo.Id = UseByTypeTo.UserTypeId
                                    WHERE UserIdFrom = @UserIdFrom
                                    AND EXISTS (
                                        SELECT 1 FROM ProductChatMessage WHERE UserIdFrom = UserIdTo AND UserIdTo = @UserIdFrom
                                    )
                                    ) AS SentResponded ON Sent.UserIdTo = SentResponded.UserIdTo
                                FULL OUTER JOIN
                                    -- Received, responded
                                    (SELECT 
                                        UserIdFrom, 
                                        CONCAT(UserInformationFrom.FirstName, ' ', UserInformationFrom.LastName) AS DisplayName,
                                        UserTypeFrom.Name AS Role,
                                        MessageContent AS LastMessage, 
                                        SentAt AS LastMessageSentAt
                                    FROM ProductChatMessage
                                    INNER JOIN [User] AS UserFrom ON UserIdFrom = UserFrom.Id
                                    INNER JOIN [UserInformation] AS UserInformationFrom ON UserFrom.UserInformationId = UserInformationFrom.Id
                                    INNER JOIN [UserByType] AS UseByTypeFrom ON UseByTypeFrom.UserId = UserFrom.Id
                                    INNER JOIN [UserType] AS UserTypeFrom ON UserTypeFrom.Id = UseByTypeFrom.UserTypeId
                                    WHERE UserIdTo = @UserIdFrom
                                    AND EXISTS (
                                        SELECT 1 FROM ProductChatMessage WHERE UserIdFrom = @UserIdFrom AND UserIdTo = UserIdFrom
                                    )
                                    ) AS ReceivedResponded ON Received.UserIdFrom = ReceivedResponded.UserIdFrom

                        ";
                // 1.  create a command object identifying the stored procedure
                var command = new SqlCommand(query, conn);
                command.Parameters.Add("@userIdFrom", SqlDbType.Int);
                command.Parameters["@userIdFrom"].Value = userId;

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
                            new AvailableUser
                            {
                                UserIdTo = rdr.GetInt32(0),
                                DisplayNameTo = rdr.GetString(1),
                                LastMessageContent = rdr.GetString(2),
                                LastMessageSentAt = rdr.GetDateTimeOffset(3).ToString("yyyy/MM/dd HH:mm:ss"),
                                RoleTo = rdr.GetString(4),
                            });
                    }
                }
            }
            return result;
        }
    }
}
