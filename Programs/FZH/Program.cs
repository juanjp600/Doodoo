using FZH;

Configuration.EnsureDirExists();
var cfg = Configuration.LoadConfig();

var zhClient = new ZhClient(cfg.UserZhGraphQlKey);
zhClient.Start();
var result = await zhClient.MakeRequest(@"
{
  issueByInfo(repositoryGhId: [RepoId], issueNumber: 3969) {
    body
    createdAt
    estimate {
      value
    }
    pipelineIssue(workspaceId: ""[WorkspaceId]"") {
      pipeline {
        name
      }
    }
    assignees {
      nodes {
        login
      }
    }
    pullRequest
    pullRequestObject {
      state
    }
    pullRequestReviews {
      nodes {
        user {
          login
        }
        state
      }
    }
    reviewRequests {
      nodes {
        reviewer {
          __typename ... on User {
            login
          }
        }
      }
    }
  }
}"
    .Replace("[RepoId]", cfg.Repos.First().Id.ToString())
    .Replace("[WorkspaceId]", cfg.Workspaces.First().Id));
Console.Write(result);
