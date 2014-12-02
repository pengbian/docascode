using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocAsCode
{
    class DdueTemplates
    {
        internal const string template = @"<?xml version=""1.0"" encoding=""utf-8""?>
<codeEntityDocument xsi:schemaLocation=""http://ddue.schemas.microsoft.com/authoring/2003/5 http://clixdevr3.blob.core.windows.net/ddueschema/developer.xsd"" xmlns=""http://ddue.schemas.microsoft.com/authoring/2003/5"" xmlns:xlink=""http://www.w3.org/1999/xlink"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <assembly>
    <assemblyName>{0}</assemblyName>
  </assembly>
  <codeEntities>
    <codeEntity>
      <codeEntityReference>{1}</codeEntityReference>
      {2}
      {3}
      {4}
      {5}
      {6}
      {7}
      {8}
      {9}
    </codeEntity>
  </codeEntities>
</codeEntityDocument>";

        internal const string templateOfSummary = @"<summary>
        <para></para>
      </summary>";

        internal const string templateOfGenericParameters = @"<genericParameters>
        <para></para>
      </genericParameters>";

        internal const string templateOfParameters = @"<parameters>
        <para></para>
      </parameters>";

        internal const string templateOfReturnValue = @"<returnValue>
        <para></para>
      </returnValue>";

        internal const string templateOfExceptions = @"<exceptions>
      </exceptions>";

        internal const string templateOfRemarks = @"<remarks>
      </remarks>";

        internal const string templateOfCodeExamples = @"<codeExamples>
      </codeExamples>";

        internal const string templateOfRelatedTopics = @"<relatedTopics>
      </relatedTopics>";
    }
}
