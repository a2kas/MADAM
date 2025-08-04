using System.ComponentModel.DataAnnotations;
using Tamro.Madam.Repository.Context.Madam;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Repository.Entities.Atcs;

public class Atc : IMadamEntity<int>, IAuditable
{
    [Key]
    public int Id { get; set; }
    public string Value { get; set; }
    public string Name { get; set; }

    public IEnumerable<Item> Items { get; set; }
}
