namespace Tamro.Madam.Models.Hr.Dynamics;
public class WorktimeImportModel
{
    public string InstitutionName { get; set; }
    public string DepartmentId { get; set; }
    public DateTime Period { get; set; }
    public List<EmployeeWorktimeModel> Employees { get; set; }
}
