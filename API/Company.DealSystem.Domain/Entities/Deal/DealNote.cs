using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Company.DealSystem.Domain.Models.Enum;

namespace Company.DealSystem.Domain.Entities
{
    public class DealNote : BaseEntity
    {
        public Deal Deal { get; set; }
        public int DealId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }

        public int CreationUserId { get; set; }
        public User CreationUser { get; set; }
        public string Content { get; set; }
        /// <summary>
        /// This determine whether this note can be deleted/changed. It becomes 'locked' whenever the deal transitions from one state to another
        /// </summary>
        public bool IsLocked { get; set; }
        public NoteReminderTypeEnum? ReminderType { get; set; }
        public int? ReminderUserId { get; set; }
        public User ReminderUser { get; set; }
        public string ReminderEmailAccounts { get; set; }
        public DateTimeOffset? ReminderDateTime { get; set; }
    }
}
