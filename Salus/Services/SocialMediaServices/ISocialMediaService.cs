using Microsoft.AspNetCore.Mvc;
using Salus.Data;

namespace Salus.Services.SocialMediaServices
{
    public interface ISocialMediaService
    {
        Task<string> StartOrStopFollow(UnFollowFollowRequest request);
        string CheckUnFollowFollowRequest(UnFollowFollowRequest request);
        Task<string> SendComment(WriteCommentRequest request);
        string CheckWriteCommentRequest(WriteCommentRequest request);
        Task<string> DeleteCommentById(int commentId);
        string CheckDeleteCommentRequest(int commentId);
        List<Comment> CreateCommentListByAuthenticatedEmail();
    }
}