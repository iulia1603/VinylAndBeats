using VinylAndBeats.DTOs;
using VinylAndBeats.Models;

namespace VinylAndBeats.Mappings;

public static class TagMappings
{
    public static TagDto ToDto(this Tag tag) => new(tag.Id, tag.Name);

    public static List<TagDto> ToDtoList(this IEnumerable<Tag> tags)
        => tags.Select(t => t.ToDto()).ToList();
}