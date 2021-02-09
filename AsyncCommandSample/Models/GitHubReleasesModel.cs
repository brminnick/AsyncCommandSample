using System;
using System.ComponentModel;

namespace AsyncCommandSample
{
    record GitHubReleasesModel(long Id, Uri Url, string Tag_Name);
}

// C# 9.0 Workaround https://stackoverflow.com/a/62656145/5953643
namespace System.Runtime.CompilerServices
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class IsExternalInit { }
}
