using System.Collections.Generic;

namespace JFS.Models.TFS.WorkItem
{
    public class WorkItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string AreaPath { get; set; }
        public string TeamProject { get; set; }
        public string IterationPath { get; set; }
        public int Priority { get; set; }
        public List<Link> Links { get; set; }

        public List<WorkItemParameter> ToParameterList()
        {
            List<WorkItemParameter> parameters = new List<WorkItemParameter>
            {
                new WorkItemParameter
                {
                    op = "add",
                    path = "/fields/System.Title",
                    value = Title
                },
                new WorkItemParameter
                {
                    op = "add",
                    path = "/fields/System.AreaPath",
                    value = AreaPath
                },
                new WorkItemParameter
                {
                    op = "add",
                    path = "/fields/System.TeamProject",
                    value = TeamProject
                },
                new WorkItemParameter
                {
                    op = "add",
                    path = "/fields/System.IterationPath",
                    value = IterationPath
                },
                new WorkItemParameter
                {
                    op = "add",
                    path = "/fields/Microsoft.VSTS.Common.Priority",
                    value = Priority
                },
            };

            foreach (var link in Links)
            {
                parameters.Add(new WorkItemParameter { op = "add", path = "/relations/-", value = link});
            }

            return parameters;
        }
    }
}
