using System.Globalization;
using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Models.ItemMasterdata.Brands;
using Tamro.Madam.Models.ItemMasterdata.Forms;
using Tamro.Madam.Models.ItemMasterdata.Items;
using Tamro.Madam.Models.ItemMasterdata.MeasurementUnits;
using Tamro.Madam.Models.ItemMasterdata.Nicks;
using Tamro.Madam.Models.ItemMasterdata.Producers;

namespace Tamro.Madam.Models.Tests.ItemMasterdata.Items;

[TestFixture]
public class ItemModelTests
{
    [TestCase(0, false)]
    [TestCase(4, true)]
    public void IsParallel_IsSetCorrectly(int parallelParentItemId, bool expectedIsParallel)
    {
        // Act
        var itemModel = new ItemModel()
        {
            ParallelParentItemId = parallelParentItemId,
        };

        // Assert
        itemModel.IsParallel.ShouldBe(expectedIsParallel);
    }


    [TestCase("TANTUM", "VERDE", "1,5MG ", "SOL", "240", "ML", 1, "ACRAF", null, "LIVORNO", "TANTUM VERDE 1,5MG  SOL 240 ML N1 (ACRAF) ")]
    [TestCase("NATEJA", "GRIPUS STRONG TEA", null, null, null, null, 20, "ACORUS CALAMUS", 0, "ACORUS CALAMUS", "NATEJA GRIPUS STRONG TEA N20 (ACORUS CALAMUS) ")]
    [TestCase("DECUBAL", "BASIC", null, "LIP BALM", "30", "ML", 1, "ACTAVIS", 0, "TEVA", "DECUBAL BASIC LIP BALM 30 ML N1 (ACTAVIS) ")]
    [TestCase("PROLACTON", "PLUS", "5MG/5 MG ", "CAPS", null, null, 15, "AGETIS SUPPLEMENTS", 0, "MEDOCHEMIE", "PROLACTON PLUS 5MG/5 MG  CAPS N15 (AGETIS SUPPLEMENTS) ")]
    [TestCase("RHESONATIV", null, "625IU/ML", "INJ", "1", "ML", 1, "OCTAPHARMA", 10006519, "ABC PHARMA", "RHESONATIV 625IU/ML INJ 1 ML N1 (ABC PHARMA) P")]
    [TestCase("LOCERYL", null, "50MG/ML", "NAIL LACQUER", "2.500", "ML", 1, "GALDERMA", 10005750, "GLOBALEX", "LOCERYL 50MG/ML NAIL LACQUER 2.5 ML N1 (GLOBALEX) P")]
    public void ItemName_GetsCorrectly(
        string? brandName,
        string? description,
        string? strength,
        string? formName,
        string? measure,
        string? measureName,
        int? numero,
        string? producerName,
        int? parallelParentItemId,
        string? supplierNick,
        string expectedItemName)
    {
        // Arrange + act
        decimal? measureValue = string.IsNullOrEmpty(measure) ? null : decimal.Parse(measure, CultureInfo.InvariantCulture);

        var model = new ItemModel
        {
            Description = description,
            Strength = strength,
            Measure = measureValue,
            Numero = numero,
            ParallelParentItemId = parallelParentItemId ?? 0,
        };

        if (!string.IsNullOrEmpty(brandName))
        {
            model.Brand = new BrandClsfModel
            {
                Id = 1,
                Name = brandName
            };
        }

        if (!string.IsNullOrEmpty(formName))
        {
            model.Form = new FormClsfModel
            {
                Id = 2,
                Name = formName
            };
        }

        if (!string.IsNullOrEmpty(measureName))
        {
            model.MeasurementUnit = new MeasurementUnitClsfModel
            {
                Id = 3,
                Name = measureName
            };
        }

        if (!string.IsNullOrEmpty(producerName))
        {
            model.Producer = new ProducerClsfModel
            {
                Id = 4,
                Name = producerName
            };
        }

        if (!string.IsNullOrEmpty(supplierNick))
        {
            model.SupplierNick = new NickClsfModel
            {
                Id = 5,
                Name = supplierNick
            };
        }

        // Assert
        model.ItemName.ShouldBe(expectedItemName);
    }
}
