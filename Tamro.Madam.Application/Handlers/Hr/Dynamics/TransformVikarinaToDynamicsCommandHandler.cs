using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using MediatR;
using OfficeOpenXml;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Commands.Hr.Dynamics;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.Hr.Dynamics;

namespace Tamro.Madam.Application.Handlers.Hr.Dynamics;

[RequiresPermission(Permissions.CanGenerateDynamicsImport)]
public class TransformVikarinaToDynamicsCommandHandler : IRequestHandler<TransformVikarinaToDynamicsCommand, Result<byte[]>>
{
    public TransformVikarinaToDynamicsCommandHandler()
    {

    }

    public async Task<Result<byte[]>> Handle(TransformVikarinaToDynamicsCommand request, CancellationToken cancellationToken)
    {
        if (request.VikarinaFiles.Count == 1)
        {
            var csvString = Encoding.GetEncoding("ISO-8859-13").GetString(request.VikarinaFiles[0]);
            var model = ExtractDataFromVikarina(csvString);
            var fileBytes = GenerateDynamicsFoImport(model);
            return Result<byte[]>.Success(fileBytes);
        }
        else
        {
            await using var memoryStream = new MemoryStream();
            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                for (int i = 0; i < request.VikarinaFiles.Count; i++)
                {
                    var csvString = Encoding.GetEncoding("ISO-8859-13").GetString(request.VikarinaFiles[i]);
                    var model = ExtractDataFromVikarina(csvString);
                    var fileBytes = GenerateDynamicsFoImport(model);

                    var zipEntry = archive.CreateEntry($"DynamicsImport_{i + 1}.xlsx", System.IO.Compression.CompressionLevel.Fastest);
                    await using var zipStream = zipEntry.Open();
                    await zipStream.WriteAsync(fileBytes, 0, fileBytes.Length, cancellationToken);
                }
            }
            return Result<byte[]>.Success(memoryStream.ToArray());
        }
    }

    private WorktimeImportModel ExtractDataFromVikarina(string vikarinaFileContent)
    {
        var model = new WorktimeImportModel
        {
            Employees = []
        };
        using var reader = new StringReader(vikarinaFileContent);

        int lineNo = 0;
        string[] headerCells = [];
        int daysInMonth = 0;
        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            lineNo++;
            if (lineNo == 1)
            {
                model.InstitutionName = line.Split(';')[0].Replace("\"", "");
            }
            else if (lineNo == 3)
            {
                var department = line.Split(';')[0];
                if (department.Contains("BENU"))
                {
                    model.DepartmentId = "7" + new string([.. department.Replace("BENU", "").Trim().Take(4)]);
                }
            }
            else if (line.StartsWith("Eilės numeris"))
            {
                var cells = line.Split(";");
                for (int i = 0; i < 4; i++)
                {
                    if (i == 3)
                    {
                        headerCells = line.Split(";");
                        daysInMonth = headerCells.Select(cell => int.TryParse(cell, out int num) ? num : (int?)null).Max() ?? int.MinValue;
                    }
                    lineNo++;
                    line = reader.ReadLine();
                }
            }
            else
            {
                var firstCell = line.Split(";")[0];
                if (int.TryParse(firstCell, out int intValue))
                {
                    var cells = line.Split(";");
                    var employeeName = Regex.Replace(cells[2], @"\d", "").Trim();
                    if (string.IsNullOrEmpty(employeeName))
                    {
                        employeeName = Regex.Replace(cells[3], @"\d", "").Trim();
                    }
                    var employee = model.Employees.FirstOrDefault(x => x.Name == employeeName);
                    bool isNewEmployee = false;
                    if (employee == null)
                    {
                        isNewEmployee = true;
                        employee = new EmployeeWorktimeModel();
                    }
                    employee.Name = employeeName;
                    var contract = new EmployeeWorktimeContractModel()
                    {
                        ContractNumber = int.TryParse(cells[1], out var contractNumber) ? contractNumber : 0,
                        WorkedHours = [],
                    };
                    var workedHours = new EmployeeWorkedHoursModel
                    {
                        Type = "DD",
                        Workdays = GetWorkdays(cells, headerCells, daysInMonth)
                    };
                    contract.WorkedHours.Add(workedHours);
                    while (true)
                    {
                        line = reader.ReadLine();
                        if (line == null)
                        {
                            break;
                        }

                        cells = line.Split(";")
                            .Where(cell => !string.IsNullOrWhiteSpace(cell))
                            .ToArray();
                        if (!cells.Any())
                        {
                            break;
                        }
                        var typeCell = cells[0];

                        string[] extraHourTypeCells = { "DN", "VD", "VN", "VS", "VSN", "SN", "DS", "BN", "PA" };
                        if (extraHourTypeCells.Contains(typeCell))
                        {
                            var extraHours = new EmployeeWorkedHoursModel
                            {
                                Type = typeCell,
                                Workdays = GetWorkdays(line.Split(";"), headerCells, daysInMonth),
                            };
                            contract.WorkedHours.Add(extraHours);
                        }
                        else
                        {
                            break;
                        }
                    }

                    employee.Contracts.Add(contract);
                    if (isNewEmployee)
                    {
                        model.Employees.Add(employee);
                    }
                }
            }
        }

        return model;
    }

    private byte[] GenerateDynamicsFoImport(WorktimeImportModel model)
    {
        var stream = new MemoryStream();
        using (var package = new ExcelPackage(stream))
        {
            var sheet = package.Workbook.Worksheets.Add("Import");

            sheet.Cells[1, 1].Value = "Tabelis";

            sheet.Cells[2, 1].Value = "Įmonės kodas:";
            string institutionCode = "LT01";
            if (model.InstitutionName.Contains("benu", StringComparison.CurrentCultureIgnoreCase))
            {
                institutionCode = "LT02";
            }
            sheet.Cells[2, 2].Value = institutionCode;

            sheet.Cells[3, 1].Value = "Įmonės pavadinimas:";
            sheet.Cells[3, 2].Value = model.InstitutionName;
            sheet.Cells[3, 4].Value = "Patvirtinta:";

            var lastDayOfPreviousMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddDays(-1);
            string formattedDate = lastDayOfPreviousMonth.ToString("yyyy-MM-dd");
            sheet.Cells[4, 1].Value = "Periodas:";
            sheet.Cells[4, 2].Value = formattedDate;
            sheet.Cells[4, 4].Value = formattedDate;

            sheet.Cells[5, 1].Value = "Užpildė:";
            sheet.Cells[5, 2].Value = "1001388";
            sheet.Cells[5, 3].Value = "";

            sheet.Cells[6, 1].Value = "Patvirtino:";
            sheet.Cells[6, 2].Value = "1001388";
            sheet.Cells[6, 3].Value = "";

            sheet.Cells[9, 1].Value = "Darbuotojas";
            sheet.Cells[9, 2].Value = "Sutarties ID";
            sheet.Cells[9, 3].Value = "Sutarties eilutė";
            sheet.Cells[9, 4].Value = "Pareigos";
            sheet.Cells[9, 5].Value = "Koeficientas";
            sheet.Cells[9, 6].Value = "Darbo laiko tipas";
            sheet.Cells[9, 7].Value = "Datos";

            const int days = 31;
            for (int i = 1; i <= days; i++)
            {
                sheet.Cells[11, i + 6].Value = i;
            }

            sheet.Cells[9, 7 + days].Value = "Viso darbo valandų";
            sheet.Cells[9, 8 + days].Value = "Viso darbo dienų";
            if (!string.IsNullOrEmpty(model.DepartmentId))
            {
                sheet.Cells[9, 9 + days].Value = "Department";
            }

            int rowIndex = 12;
            foreach (var employee in model.Employees)
            {
                RemoveDuplicateHours(employee.Contracts);
                foreach (var contract in employee.Contracts)
                {
                    foreach (var worktimeType in contract.WorkedHours)
                    {
                        sheet.Cells[rowIndex, 1].Value = employee.Name;
                        sheet.Cells[rowIndex, 2].Value = $"{institutionCode}10{employee.PrimaryContractNumber}";
                        sheet.Cells[rowIndex, 3].Value = contract.ContractNumber;
                        sheet.Cells[rowIndex, 4].Value = "";
                        sheet.Cells[rowIndex, 5].Value = "";
                        sheet.Cells[rowIndex, 6].Value = worktimeType.Type;
                        foreach (var workday in worktimeType.Workdays)
                        {
                            if (double.TryParse(workday.Hours, out double numericHours) && numericHours != default)
                            {
                                sheet.Cells[rowIndex, 6 + workday.Date].Value = numericHours;
                            }
                        }
                        sheet.Cells[rowIndex, 9 + days].Value = model.DepartmentId;
                        rowIndex++;
                    }
                }
            }

            package.Save();
        }
        return stream.ToArray();
    }

    private List<WorkdayModel> GetWorkdays(string[] cells, string[] headerCells, int daysInMonth)
    {
        var workdays = new List<WorkdayModel>();

        var validHeaderIndicesAndValues = headerCells
            .Select((headerCell, index) => new { headerCell, index })
            .Where(x => int.TryParse(x.headerCell, out _))
            .Select(x => new { x.index, x.headerCell })
            .ToList();

        for (int i = 0; i < daysInMonth; i++)
        {
            var headerCellIndex = validHeaderIndicesAndValues.Find(x => x.headerCell == $"{i + 1}")?.index;
            if (headerCellIndex != null)
            {
                var cell = cells[headerCellIndex ?? 0];

                workdays.Add(new WorkdayModel()
                {
                    Date = i + 1,
                    Hours = cell,
                });
            }
        }

        return workdays;
    }

    public void RemoveDuplicateHours(List<EmployeeWorktimeContractModel> contracts)
    {
        foreach (var contract in contracts)
        {
            var ddEntries = contract.WorkedHours.Where(wh => wh.Type == "DD").ToList();
            var dsEntries = contract.WorkedHours.Where(wh => wh.Type == "DS").ToList();

            foreach (var ddEntry in ddEntries)
            {
                foreach (var ddWorkday in ddEntry.Workdays.ToList())
                {
                    var matchingDsEntry = dsEntries
                        .SelectMany(dsEntry => dsEntry.Workdays, (dsEntry, dsWorkday) => new { dsEntry, dsWorkday })
                        .FirstOrDefault(x => x.dsWorkday.Date == ddWorkday.Date && x.dsWorkday.Hours == ddWorkday.Hours);

                    if (matchingDsEntry != null)
                    {
                        ddEntry.Workdays.Remove(ddWorkday);
                    }
                }
            }
        }
    }
}
