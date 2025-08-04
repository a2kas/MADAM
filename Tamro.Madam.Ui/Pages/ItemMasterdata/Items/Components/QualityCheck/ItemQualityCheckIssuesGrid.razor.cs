using DiffPlex.DiffBuilder.Model;
using DiffPlex.DiffBuilder;
using DiffPlex;
using System.Net;
using System.Text;
using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components;
using Tamro.Madam.Models.ItemMasterdata.Items.QualityCheck;
using Tamro.Madam.Ui.Store.State;

namespace Tamro.Madam.Ui.Pages.ItemMasterdata.Items.Components.QualityCheck;

public partial class ItemQualityCheckIssuesGrid
{
    [EditorRequired]
    [Parameter]
    public List<ItemQualityCheckIssueGridModel> Issues { get; set; }

    [Inject]
    private IMediator _mediator { get; set; }
    [Inject]
    private IState<UserProfileState> _userProfileState { get; set; }


    private string HighlightDiff(string baseText, string compareText, bool isExpected)
    {
        if (string.IsNullOrEmpty(baseText) || string.IsNullOrEmpty(compareText))
        {
            return string.Empty;
        }

        var differ = new Differ();
        var diff = differ.CreateCharacterDiffs(baseText, compareText, false, false);

        var sb = new StringBuilder();
        int baseOffset = 0;
        int compareOffset = 0;

        foreach (var block in diff.DiffBlocks)
        {
            int basePos = block.DeleteStartA;
            int comparePos = block.InsertStartB;

            // Common text before the diff block
            if (isExpected && comparePos > compareOffset)
            {
                sb.Append(WebUtility.HtmlEncode(compareText.Substring(compareOffset, comparePos - compareOffset)));
            }
            else if (!isExpected && basePos > baseOffset)
            {
                sb.Append(WebUtility.HtmlEncode(baseText.Substring(baseOffset, basePos - baseOffset)));
            }

            for (int i = 0; i < Math.Max(block.DeleteCountA, block.InsertCountB); i++)
            {
                if (!isExpected && i < block.DeleteCountA)
                {
                    var deletedChar = baseText[basePos + i];
                    sb.Append($"<span style='color: red; text-decoration: line-through'>{WebUtility.HtmlEncode(deletedChar.ToString())}</span>");
                }

                if (isExpected && i < block.InsertCountB)
                {
                    var insertedChar = compareText[comparePos + i];
                    sb.Append($"<span style='background-color: lightgreen'>{WebUtility.HtmlEncode(insertedChar.ToString())}</span>");
                }
            }

            // Update offsets
            baseOffset = block.DeleteStartA + block.DeleteCountA;
            compareOffset = block.InsertStartB + block.InsertCountB;
        }

        // Append any remaining common text
        if (isExpected && compareOffset < compareText.Length)
        {
            sb.Append(WebUtility.HtmlEncode(compareText.Substring(compareOffset)));
        }
        else if (!isExpected && baseOffset < baseText.Length)
        {
            sb.Append(WebUtility.HtmlEncode(baseText.Substring(baseOffset)));
        }

        return sb.ToString();
    }

}
