﻿namespace Authentication.Controllers.Models.JoiningEntity
{
    public class UserProfileToUserProfile
    {
        public int id { get; set; }
        public int followerId { get; set; }
        public UserProfile? follower { get; set; }
        public string followDate { get; set; } = DateTime.Now.ToString("yyyy.MM.dd");
        public int followedId { get; set; }
        public UserProfile? followed { get; set; }
    }
}
