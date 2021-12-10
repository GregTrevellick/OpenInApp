using System.Collections.Generic;

namespace OpenInApp
{
    public class ApplicationToOpenDto
    {
        public IEnumerable<string> ExecutableFilesToBrowseFor { get; set; }
        public bool ProcessWithinProcess { get; set; }
        public bool SeparateProcessPerFileToBeOpened { get; set; }
        public bool UseShellExecute { get; set; }
    }
}
