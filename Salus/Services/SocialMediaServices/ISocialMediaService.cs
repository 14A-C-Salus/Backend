using Microsoft.AspNetCore.Mvc;
using Salus.Data;

namespace Salus.Services.SocialMediaServices
{
    public interface ISocialMediaService
    {
        Task<string> StartOrStopFollow(UnFollowFollowRequest request);
        Task<string> SendComment(WriteCommentRequest request);
        Task<string> DeleteCommentById(int commentId);
        List<Comment> CreateCommentListByAuthenticatedEmail();
        Task<string> ModifyComment(ModifyCommentRequest request);
    }
}