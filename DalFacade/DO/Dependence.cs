

namespace DO;

public record Dependence
(
    int Id,
    int? DependentTask=null,
    int? DependsOnTask=null
)

{
public Dependence(): this (0) { } //empty constructor

}
