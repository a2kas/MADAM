using System.Text;
using Tamro.Madam.Application.Clients;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.QualityCheck;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items;

namespace Tamro.Madam.Application.Services.Items.QualityCheck;

public class QualityCheckAiConsumerService : IQualityCheckAiConsumerService
{
    private readonly IMinioHttpClient _minioHttpClient;

    public QualityCheckAiConsumerService(IMinioHttpClient minioHttpClient)
    {
        _minioHttpClient = minioHttpClient;       
    }

    public string ConstructPrompt()
    {
        return @"You are an AI assistant specializing in pharmaceutical product information quality control. Your task is to meticulously analyze the provided product data in the later 'JSON Schema' section. You are going to compare the product data to the supplied reference values ('referenceValue' values), which come from our database, so we want be informed of any errors in these reference values. The input product data will contain information structured in JSON that also outlines the format of your response.
            Your response must be a JSON array (with nothing in addition such as markdown) that closely follows the structure of the JSON for the input product data, except you will omit 'language', 'dataType', 'description', and 'referenceValue' subfields. Follow these procedures for filling out 'value', 'issuesFlagged' and 'issuesType':
            A. Do these steps for the 'activeIngredient' and 'strength' fields:
             1. Carefully review the product information (the 'referenceValue' value for each 'field' throughout the JSON including other sections) and in the 'value' subfield fill in the active ingredient (in 'activeIngredient' field) and the strength or concentration of that active ingredient in the 'strength' field. Obey the 'dataType', but use null if there is no active ingredient or strength found.
             2. Compare your found 'value' to the 'referenceValue' for the 'activeIngredient' and 'strength' fields.
             3. If your value is non-null, then in the 'issuesFlagged' subfield explain why a revision was needed and categorize the 'issuesType' as in the later 'issuesType definitions' section.
            B. Do these steps for the rest of the fields, i.e., 'name', 'shortDescription', 'fullDescription' and 'usage':
             1. Review the 'referenceValue' subfield to identify major discrepancies, factual errors, grammatical problems, and HTML tags that are not syntactically correct.
             2. Place your proposed revision of this text (if any) into the 'value' subfield, else null to suggest no change to 'referenceValue'.
             3. If your value is non-null, then note any major differences in the 'issuesFlagged' subfield and categorize the 'issuesType' as in the later 'issuesType definitions' section.
            IMPORTANT: To note a difference, it must be ""major"", in that it could mislead or confuse a customer (e.g., incorrect data) or could give us a bad company image (e.g., grammar mistake). Do not nitpick about HTML so long as it is syntactically correct.
            issuesType definitions:
            Types of issues (for 'issuesTypes' subfield):
                - ""missingData"" for when you have found a non-null 'value' but the 'referenceValue' is null (for 'activeIngredient' or 'strength' fields). However, the strength should be null if the activeIngredient is null, and it can be okay for there to be no active ingredient.
                - ""incorrectData"" for when you have a 'value' that differs from the 'referenceValue' for that field (for 'activeIngredient' or 'strength' fields).
                - ""grammarMistake"" for incorrect grammar in the 'referenceValue' for that field.
                - ""erroneousHtmlTagging"" for HTML tags that are not set up properly.
                - ""textStyle"" for writing issues in the 'referenceValue' for that field.
                - ""confusingText"" for text in the 'referenceValue' that could mislead or confuse a customer.
                - ""inconsistentText"" for text that says different things across countries or fields for the same product.
                - ""uncategorized"" for issues that cannot be easily fit into the above categories.
                - null for no issues (i.e., when 'issuesFlagged' is null)
            Note: when filling in 'value', use the language specified in the 'language' subfield: English for ""EN"", Lithuanian for ""LT"", ""Latvian"" for ""LV"", and ""Estonian"" for ""EE"". However, 'issuesFlagged' should be English and 'issuesType' should be the camelcase English as in the definitions table.";
    }

    public async Task<ItemQualityCheckReferenceModel> ConstructReference(Item item)
    {
        var reference = new ItemQualityCheckReferenceModel();
        var sb = new StringBuilder();

        sb.AppendLine($"Product ID: {item.Id}");
        reference.Id = item.Id;
        sb.AppendLine($"Product name: {item.ItemName}");
        reference.ItemName = item.ItemName;
        sb.AppendLine($"ATC: {item.Atc?.Name}");
        reference.Atc = item.Atc?.Name;
        sb.AppendLine($"Strength: {item.Strength}");
        reference.Strength = item.Strength;
        sb.AppendLine();

        var groupedBindings = item.Bindings
            .Where(x => x.ItemBindingInfo != null && !string.IsNullOrEmpty(x.Company))
            .GroupBy(x => x.Company.Substring(0, 3))
            .Select(g => g.First());

        foreach (var itemBinding in groupedBindings)
        {
            var bindingReference = new ItemBindingQualityCheckReferenceModel();
            sb.AppendLine("-----------");
            sb.AppendLine($"Country: {Classifiers.Companies.SingleOrDefault(x => x.Value == itemBinding.Company)?.Country.ToString()}");
            sb.AppendLine($"Local item code: {itemBinding.LocalId}");
            bindingReference.LocalId = itemBinding.LocalId;
            bindingReference.Id = itemBinding.Id;
            sb.AppendLine($"Short description: {itemBinding.ItemBindingInfo?.ShortDescription}");
            bindingReference.ShortDescription = itemBinding.ItemBindingInfo?.ShortDescription;

            try
            {
                if (!string.IsNullOrEmpty(itemBinding.ItemBindingInfo?.DescriptionReference))
                {
                    var description = await _minioHttpClient.FetchHtml(itemBinding.ItemBindingInfo.DescriptionReference);
                    bindingReference.Description = description;
                    sb.AppendLine($"Description: {description}");
                }
            }
            catch (Exception ex)
            {
               //TODO:
            }
            sb.AppendLine($"Usage: {itemBinding.ItemBindingInfo?.Usage}");
            bindingReference.Usage = itemBinding.ItemBindingInfo?.Usage;
            sb.AppendLine("-----------");
            reference.Bindings.Add(bindingReference);
        }

        sb.AppendLine(@"
            Please return the information as a JSON array with ""section"" tags at the top level, ""section"" should follow format {country}_{Local item code} and within each ""section"" 
            there is an array of ""fields"".
            The keys are as follows:
            * field - The name of the field.
            * language - The language to be used for this section. (See prompt above for a list.)
            * dataType - the data type of this field (string, integer, decimal, or date - defaults to string).
            * description - An explanation of this field for the LLM.
            * referenceValue - The value in the database.
            * issuesFlagged - A placeholder for the LLM to raise issues with the reference value.
            * issuesType - A placeholder for the LLM to make a general summary of the types of issues raised. (See prompt above for a list.)
            * value - A placeholder for the LLM to supply a suggested revision (if any). Do not remove HTML tags when proposing the new value.
            ");

        reference.Text = sb.ToString();

        return reference;
    }

    public string ConstructSchema()
    {
        return string.Empty;
    }
}
