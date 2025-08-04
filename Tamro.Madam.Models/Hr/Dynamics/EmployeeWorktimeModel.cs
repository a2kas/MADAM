namespace Tamro.Madam.Models.Hr.Dynamics;

public class EmployeeWorktimeModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string? PrimaryContractNumber =>
                     Contracts?
                  .OrderBy(c => c.ContractNumber)
                  .FirstOrDefault()?.ContractNumber.ToString();
    public List<EmployeeWorktimeContractModel> Contracts { get; set; } = [];
}
