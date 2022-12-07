using Microsoft.AspNetCore.Mvc;
using Salus.Data;

namespace Salus.Services.SocialMediaServices
{
    public interface ISocialMediaService
    {
        string CheckUnFollowFollowRequest(UnFollowFollowRequest request);
        Task<string> StartOrStopFollow(UnFollowFollowRequest request);
        string CheckWriteCommentRequest(WriteCommentRequest request);
        Task<string> SendComment(WriteCommentRequest request);
        string CheckDeleteCommentRequest(int commentId);
        Task<string> DeleteCommentById(int commentId);
    }
}