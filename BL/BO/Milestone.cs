

namespace BO;

public class Milestone
{
    public int? Id { get; init; }
    public string Description { get; set; }
    public string Alias { get; set; }
    public DateTime CreatedAtDate { get; init; }
    public BO.Status? Status { get; set; }
    public DateTime? ForecastDate { get; set; }
    public DateTime? DeadLineDate { get; set; }
    public DateTime? CompleteDate { get; set; }
    public double? CompletionPercentage { get; set; }
    public string? Reamarks { get; set; }
    public IEnumerable<TaskInList>? Deoendencies { get; set; }
}
