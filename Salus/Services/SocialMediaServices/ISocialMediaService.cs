﻿using Microsoft.AspNetCore.Mvc;
using Salus.Data;

namespace Salus.Services.SocialMediaServices
{
    public interface ISocialMediaService
    {
        void StartOrStopFollow(UnFollowFollowRequest request);
        Task<Comment> SendComment(WriteCommentRequest request);
        void DeleteCommentById(int commentId);
        List<Comment> CreateCommentListByAuthenticatedEmail();
        Task<Comment> ModifyComment(ModifyCommentRequest request);
    }
}