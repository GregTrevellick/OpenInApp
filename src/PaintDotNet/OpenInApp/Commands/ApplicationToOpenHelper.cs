using System.Collections.Generic;

namespace OpenInApp
{
    public class ApplicationToOpenHelper
    {
        public IEnumerable<string> GetExecutableFilesToBrowseFor()
        {
            return GetApplicationToOpenDto().ExecutableFilesToBrowseFor;
        }

        public ApplicationToOpenDto GetApplicationToOpenDto()
        {
            var applicationToOpenDto = new ApplicationToOpenDto
            {
                ExecutableFilesToBrowseFor = new List<string> { "PaintDotNet.exe" },
                ProcessWithinProcess = false,
                SeparateProcessPerFileToBeOpened = true,
                UseShellExecute = true,
            };

            return applicationToOpenDto;
        }
    }
}
