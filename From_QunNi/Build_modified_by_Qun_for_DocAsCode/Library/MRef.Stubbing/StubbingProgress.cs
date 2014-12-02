namespace Microsoft.Content.BuildEngine.MRef.Stubbing
{
    using Microsoft.Content.BuildEngine.DataAccessorInterface;
    using Microsoft.Content.BuildEngine.ReflectionTables;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Web.Script.Serialization;

    public sealed class StubbingProgress
    {
        #region Fields/Consts
        public const int NotStarted = -1;
        private ITableAccessor<BuildTask> m_accessor;
        private string m_currentVersion;
        private BuildTask m_buildTask;
        private readonly List<string> m_reportContent = new List<string>();
        private readonly Stopwatch m_stopwatch = Stopwatch.StartNew();
        #endregion

        #region Ctors

        public StubbingProgress()
        {
            FileCapacity = NotStarted;
            FileCount = NotStarted;
            TopicCount = NotStarted;
            Preprocessed = NotStarted;
            Submitted = NotStarted;
            Ignored = NotStarted;
            ValidatedCount = NotStarted;
            CommitedCount = NotStarted;
            CleanUpCount = NotStarted;
            TotalOtherTaskCount = NotStarted;
            CompletedOtherTaskCount = NotStarted;
        }

        #endregion

        #region Json Properties

        public int TopicCount { get; set; }

        public int Preprocessed { get; set; }

        public int Submitted { get; set; }

        public int Ignored { get; set; }

        public int FileCount { get; set; }

        public int FileCapacity { get; set; }

        public int ValidatedCount { get; set; }

        public int CommitedCount { get; set; }

        public int CleanUpCount { get; set; }

        public int TotalOtherTaskCount { get; set; }

        public int CompletedOtherTaskCount { get; set; }

        #endregion

        #region Status Properties

        public bool IsChangesGenerated { get { return TopicCount >= 1; } }

        public bool IsPreprocessCompleted { get { return IsChangesGenerated && CompletedOtherTaskCount >= 2; } }

        public bool IsSubmitCompleted { get { return IsChangesGenerated && CompletedOtherTaskCount >= 3; } }

        public bool IsValidationCompleted { get { return IsChangesGenerated && CompletedOtherTaskCount >= 4; } }

        public bool IsCommitCompleted { get { return IsChangesGenerated && CommitedCount == TopicCount; } }

        public bool IsCleanupCompleted { get { return IsChangesGenerated && CompletedOtherTaskCount >= 5; } }

        public bool IsPostprocessCompleted { get { return CompletedOtherTaskCount == TotalOtherTaskCount; } }

        internal string Status { get { return m_buildTask.TaskStatus; } set { m_buildTask.TaskStatus = value; } }

        #endregion

        #region Report Properties
        internal string ReportStepName { get; set; }
        internal Func<int> GetTotal { get; set; }
        internal Func<int> GetSuccessed { get; set; }
        internal Func<int> GetIgnored { get; set; }
        internal int FailedCount { get; set; }
        internal int PendingCount { get { return GetTotal() - GetSuccessed() - GetIgnored() - FailedCount; } }
        #endregion

        #region Methods

        public void Save()
        {
            var js = new JavaScriptSerializer();
            m_buildTask.Comment = js.Serialize(this);
            m_accessor.InsertOrUpdate(m_buildTask);
        }

        public static StubbingProgress Load(IAccessorFactory factory, string currentVersion)
        {
            var accessor = factory.CreateTableAccessor<BuildTask>();
            var buildTask = accessor.Get(currentVersion, SR.TaskKey_ChangesItem);
            StubbingProgress result;
            if (buildTask == null)
            {
                result = new StubbingProgress();
                buildTask = new BuildTask
                {
                    TaskIndex = SR.TaskKey_ChangesItem,
                    VersionNumber = currentVersion,
                    Phase = SR.TaskPhase_Stubbing,
                    TaskStatus = SR.TaskStatus_Pending,
                    Comment = "{}",
                };
            }
            else
            {
                var js = new JavaScriptSerializer();
                result = js.Deserialize<StubbingProgress>(buildTask.Comment);
            }
            result.m_accessor = accessor;
            result.m_buildTask = buildTask;
            result.m_currentVersion = currentVersion;
            return result;
        }

        public void AddReportLine()
        {
            AddReportLine(string.Format(
                "{0}: Total {1}, Succeeded {2}, Ignored {3}, Failed {4}, Pending {5}.",
                ReportStepName,
                GetTotal(),
                GetSuccessed(),
                GetIgnored(),
                FailedCount,
                PendingCount >= 0 ? PendingCount : 0), 1);
        }

        public void AddReportLines(string content, int indent = 1)
        {
            AddReportLines(content.Split(new[] { Environment.NewLine }, StringSplitOptions.None), indent);
        }

        public void AddReportLines(IEnumerable<string> lines, int indent = 1)
        {
            foreach (var text in lines)
            {
                AddReportLine(text, indent);
            }
        }

        private void AddReportLine(string text, int indent)
        {
            string line;
            if (indent == 0)
            {
                line = text;
            }
            else
            {
                line = new string(' ', indent * 2);
                line += text;
            }
            m_reportContent.Add(line);
        }

        public TimeSpan GetTimespan()
        {
            return m_stopwatch.Elapsed;
        }

        public string GetReport()
        {
            return string.Join(Environment.NewLine, m_reportContent);
        }

        #endregion
    }
}
