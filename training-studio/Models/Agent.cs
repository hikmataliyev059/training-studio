using training_studio.Models.Common;

namespace training_studio.Models;

public class Agent : BaseEntity
{
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public IFormFile File { get; set; }
    public int PositionId { get; set; }
    public Position Position { get; set; }  
}
