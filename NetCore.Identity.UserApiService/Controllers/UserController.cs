using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCore.Identity.Enums;
using NetCore.Identity.UserApiService.Extension;

namespace NetCore.Identity.UserApiService.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        public async Task<object> Get()
        {
            //通过ClaimsPrincipal（证件容器载体）获得某些证件的身份元件
            return new
            {
                name = User.Name(),
                userId = User.UserId(),
                displayName = User.DisplayName(),
                merchantId = User.MerchantId(),
                aud = User.Audience()
            };
        }
        [Authorize(Policy = "ApiV2",Roles = nameof(EnumUserRole.SupperManage))]
        [HttpGet("{id}")]
        public async Task<object> Get(int id)
        {
            return new
            {
                name = User.Name(),
                userId = User.UserId(),
                displayName = User.DisplayName(),
                merchantId = User.MerchantId(),
                roleName = User.Role(),//获得当前登录用户的角色
                aud = User.Audience()
            };
        }

        [Authorize(Roles = nameof(EnumUserRole.Normal))]
        [Route("Post")]
        [HttpPost]
        public async Task<object> Post()
        {
            var userId = User.UserId();
            return new
            {
                name = User.Name(),
                userId = userId,
                displayName = User.DisplayName(),
                merchantId = User.MerchantId(),
                roleName = User.Role()//获得当前登录用户的角色
            };
        }
    }
}