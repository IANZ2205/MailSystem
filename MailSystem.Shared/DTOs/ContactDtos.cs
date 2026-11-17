using System;

namespace MailSystem.Shared.DTOs
{
    public class ContactDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public DateTime AddedAt { get; set; }
    }

    public class AddContactRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
    }

    public class RemoveContactRequestDto
    {
        public int ContactId { get; set; }
    }
}