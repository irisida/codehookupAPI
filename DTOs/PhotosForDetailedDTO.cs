using System;

namespace hookup.API.DTOs
{
  public class PhotosForDetailedDTO
  {
    public int Id { get; set; }
    public string Url { get; set; }
    public string Description { get; set; }
    public DateTime DateAdded { get; set; }
    public bool IsAvatar { get; set; }
  }
}