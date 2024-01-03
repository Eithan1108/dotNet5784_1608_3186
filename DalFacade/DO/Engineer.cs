

namespace DO;

public record Engineer
(
  int  Id, 
  string Name ,
  string Email,
  EngineerExperience Level,
  double Cost
)

{
    public Engineer() : this(0, "", "", DO.EngineerExperience.Beginner, 0) { } //empty constructor
}


