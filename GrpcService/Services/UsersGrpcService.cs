using AutoMapper;
using FluentValidation;
using Grpc.Core;
using GrpcService.Protos.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.Server.IIS.Core;
using Services.Interfaces;

namespace GrpcService.Services
{
    public class UsersGrpcService : GrpcService.Protos.Users.GrpcUsers.GrpcUsersBase
    {
        private IUsersService _service;
        private IMapper _mapper;
        private IValidator<User> _validator;

        public UsersGrpcService(IUsersService service, IMapper mapper, IValidator<User> validator)
        {
            _service = service;
            _mapper = mapper;
            _validator = validator;
        }

        public override async Task<User> Create(User request, ServerCallContext context)
        {

            //Models.User modelsUser = await _service.CreateAsync(new Models.User { Id = request.Id, Password = request.Password, Username = request.Username });
            var result = await _service.CreateAsync(_mapper.Map<Models.User>(request));

            //throw new Exception("oppsss");

            return _mapper.Map<User>(result);
        }

        public override async Task<Users> Read(None request, ServerCallContext context)
        {
            var result = await _service.ReadAsync();

            var users = new Users();
            users.Collection.AddRange(_mapper.Map<IEnumerable<User>>(result));

            return users;
        }

        public override async Task<User> ReadById(User request, ServerCallContext context)
        {
            if (_validator.Validate(request).IsValid)
                return new User();

            var result = await _service.ReadAsync(request.Id);

            return _mapper.Map<User>(result);
        }

        public override async Task<None> Update(User request, ServerCallContext context)
        {
            /*context.GetHttpContext().Response.StatusCode = StatusCodes.Status400BadRequest;
            return Task.FromResult(new None());*/
            
            await _service.UpdateAsync(request.Id, _mapper.Map<Models.User>(request));

            return new None();

        }

        public override async Task<None> Delete(User request, ServerCallContext context)
        {
            await _service.DeleteAsync(request.Id);

            return new None();
        }
    }
}
