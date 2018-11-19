using System.Collections.Generic;

namespace JFS.Models.TFS.WorkItem
{
    public class WorkItem
    {
        public string Title { get; set; }
        public string ReproSteps { get; set; }
        public string CreatedDate { get; set; }
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
                    path = "/fields/Microsoft.VSTS.TCM.ReproSteps", //"/fields/System.Description",
                    value = ReproSteps // $"{ReproSteps.Replace("\n", "<div></div>")}"
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

            if (Links != null)
            {
                foreach (var link in Links)
                {
                    parameters.Add(new WorkItemParameter { op = "add", path = "/relations/-", value = link });
                }
            }

            return parameters;
        }

        public List<WorkItemParameter> ToParameterListNotEmptyFields(int rev)
        {
            List<WorkItemParameter> parameters = new List<WorkItemParameter>();

            if (!(string.IsNullOrWhiteSpace(Title) || string.IsNullOrEmpty(Title)))
            {
                parameters.Add(new WorkItemParameter
                {
                    op = "add",
                    path = "/fields/System.Title",
                    value = Title
                });
            }

            if (!(string.IsNullOrWhiteSpace(ReproSteps) || string.IsNullOrEmpty(ReproSteps)))
            {
                parameters.Add(new WorkItemParameter
                {
                    op = "add",
                    path = "/fields/Microsoft.VSTS.TCM.ReproSteps",
                    value = ReproSteps //$"<div>{ReproSteps.Replace("\n", "</div><div>")}</div>"
                });
            }

            if (Priority != 0)
            {
                parameters.Add(new WorkItemParameter
                {
                    op = "add",
                    path = "/fields/Microsoft.VSTS.Common.Priority",
                    value = Priority
                });
            }

            parameters.Add(new WorkItemParameter
            {
                op = "test",
                path = "/rev",
                value = rev
            });

            if (Links != null)
            {
                foreach (var link in Links)
                {
                    parameters.Add(new WorkItemParameter { op = "add", path = "/relations/-", value = link });
                }
            }

            return parameters;
        }


    }
}
