using TamroUtilities.Abstractions;

namespace Tamro.Madam.Repository.Audit;

public class UserAccessor : IUserAccessor
{
    private string _name;

    public string Name
    {
        get { return _name; }
    }

    public Guid SubjectId => throw new NotImplementedException();

    public bool IsSubjectable => throw new NotImplementedException();

    public void SetUsername(string username)
    {
        _name = username;
    }
}
