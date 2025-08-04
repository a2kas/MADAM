using TamroUtilities.Hangfire.Attributes;
using TamroUtilities.Hangfire.Models;

namespace Tamro.Madam.Application.Jobs.Hangfire;

[DisableConcurrentJob()]
public abstract class SendCanceledLineEmailsBase : HangfireJobBase
{
}
