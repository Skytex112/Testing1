using System.ComponentModel.DataAnnotations;

namespace ProjectTestingNote.DTO
{
    public record NoteCreateDto( string? Title,
        string? Content);

    public record NoteDTO(Guid Id, string? Title, string? Content, DateTime CreatedAt);

    public record NoteReadDTO(string? Title,string? Content, DateTime CreatedAt);
}
