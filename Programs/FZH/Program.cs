using FZH;
using FZH.Queries;

Configuration.EnsureDirExists();
var cfg = Configuration.LoadConfig();

var zhClient = new ZhClient(cfg.UserZhGraphQlKey);
zhClient.Start();

var testQuery = new TestQuery {
  IssueByInfo = new(RepositoryGhId: cfg.Repos.First().Id, IssueNumber: 3969) {
    PipelineIssue = new(WorkspaceId: cfg.Workspaces.First().Id)
  }
};
var pee = testQuery.IssueByInfo.ToGraphQlQuery("issueByInfo");

var result = await zhClient.MakeRequest("{\n  "+testQuery.IssueByInfo.ToGraphQlQuery("issueByInfo").Replace("\n", "\n  ")+"\n}");
Console.Write(result);
