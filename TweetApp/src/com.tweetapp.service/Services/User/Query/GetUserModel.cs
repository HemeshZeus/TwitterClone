namespace com.tweetapp.service
{
    using MediatR;
    using System.Collections.Generic;

    public class GetUserModel : IRequest<ValidatableResponse<List<UserView>>>
    {
        public string Name { get; set; }
    }
}
