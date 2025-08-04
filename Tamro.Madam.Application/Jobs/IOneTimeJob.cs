using Tamro.Madam.Application.Models.Common;

namespace Tamro.Madam.Application.Jobs;

public interface IOneTimeJob
{
    string Name { get; }
    string Description { get; }
    bool Processing { get; set; }
    Task<Result<int>> Execute();
}
