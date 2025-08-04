using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tamro.Madam.Repository.Context.Madam;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings;

[Table("ItemLanguages")]
public class ItemBindingLanguage : IMadamEntity<int>, IAuditable, IBaseEntity
{
    [Key]
    public int Id { get; set; }
    public int ItemBindingId { get; set; }
    public int LanguageId { get; set; }

    public ItemBinding ItemBinding { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime RowVer { get; set; }
}
