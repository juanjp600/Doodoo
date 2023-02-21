using DoodooCoreCsharp;

#pragma warning disable CS0649
namespace FZH.Queries; 

class TestQuery : Query {
    internal sealed record IssueByInfoFragment(ulong RepositoryGhId, ulong IssueNumber) : QueryFragment {
        public ValueString Body;
        public DateTime CreatedAt;

        public sealed record EstimateFragment : QueryFragment {
            public float Value;
        }
        public EstimateFragment Estimate = new EstimateFragment();

        public sealed record PipelineIssueFragment(ValueString WorkspaceId) : QueryFragment {
            public sealed record PipelineFragment : QueryFragment {
                public ValueString Name;
            }
            public PipelineFragment Pipeline = new PipelineFragment();
        }
        public PipelineIssueFragment PipelineIssue = new PipelineIssueFragment(WorkspaceId: default);

        public sealed record AssigneesFragment : QueryFragment {
            public sealed record NodeFragment : QueryFragment {
                public ValueString Login;
            }
            public NodeFragment[] Nodes = Array.Empty<NodeFragment>();
        }
        public AssigneesFragment Assignees = new AssigneesFragment();

        public bool PullRequest;

        public sealed record PullRequestObjectFragment : QueryFragment {
            public ValueString State;
        }
        public PullRequestObjectFragment PullRequestObject = new PullRequestObjectFragment();

        public sealed record PullRequestReviewsFragment : QueryFragment {
            public sealed record NodeFragment : QueryFragment {
                public sealed record UserFragment : QueryFragment {
                    public ValueString Login;
                }
                public UserFragment User = new UserFragment();
                public ValueString State;
            }
            public NodeFragment[] Nodes = Array.Empty<NodeFragment>();
        }
        public PullRequestReviewsFragment PullRequestReviews = new PullRequestReviewsFragment();
        
        public sealed record ReviewRequestsFragment : QueryFragment {
            public sealed record NodeFragment : QueryFragment {
                public sealed record ReviewerFragment : QueryFragment {
                    public sealed record UserFragment : TypenameMatchFragment {
                        public ValueString Login;
                    }
                    public UserFragment User = new UserFragment();
                }
                public ReviewerFragment Reviewer = new ReviewerFragment();
            }
            public NodeFragment[] Nodes = Array.Empty<NodeFragment>();
        }
        public ReviewRequestsFragment ReviewRequests = new ReviewRequestsFragment();
    }
    public IssueByInfoFragment IssueByInfo = new IssueByInfoFragment(RepositoryGhId: 0, IssueNumber: 0);
}
