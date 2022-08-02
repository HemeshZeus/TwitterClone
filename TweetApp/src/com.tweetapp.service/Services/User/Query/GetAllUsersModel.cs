namespace com.tweetapp.service
{
    using MediatR;
    using System.Collections.Generic;

    public class GetAllUsersModel : IRequest<ValidatableResponse<List<UserView>>>
    {
        public bool ActiveOnly { get; set; }
    }
}
