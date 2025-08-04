using System.ComponentModel.DataAnnotations;
using Tamro.Madam.Repository.Context.Madam;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Repository.Entities.Users.UserSettings;

public class UserSetting : IMadamEntity<string>, IAuditable
{
    [Key]
    public string Id { get; set; }
    public int? RowsPerPage { get; set; }
}
