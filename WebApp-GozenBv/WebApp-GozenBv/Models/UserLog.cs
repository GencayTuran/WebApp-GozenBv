using System;

namespace WebApp_GozenBv.Models
{
    public class UserLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Action { get; set; }
        public int Controller { get; set; }
        public string EntityId { get; set; }
        public DateTime LogDate { get; set; }
        public User User { get; set; }
    }
}
