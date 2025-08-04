namespace Tamro.Madam.Models.Hr.Dynamics;
public class EmployeeWorktimeContractModel
{
    public int ContractNumber { get; set; }
    public List<EmployeeWorkedHoursModel> WorkedHours { get; set; }
}
