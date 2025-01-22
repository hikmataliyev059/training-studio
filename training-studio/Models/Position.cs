using training_studio.Models.Common;

namespace training_studio.Models;

public class Position : BaseEntity
{
    public string Name { get; set; }
    public ICollection<Agent> Agents { get; set; }
}
