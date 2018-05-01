using System;
using System.Collections.Generic;
using Core.DTOs;
using Core.Interfaces;
using Core.Model.Extensions;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StudyCase.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ValuesController : Controller
    {
        private readonly IUnityOfWork _uow;

        public ValuesController(IUnityOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        // GET api/values
        public IEnumerable<UserDto> List()
        {
            return _uow.UserRepository.List(new UserWithOrdersSpecification()).GetDtoList();
        }

        [HttpGet]
        // GET api/values/5
        public UserDto Get([FromQuery] string id)
        {
            return _uow.UserRepository.GetById(Guid.Parse(id)).GetDto();
        }

        [HttpPost]
        // POST api/values
        public void Post([FromBody] UserDto dto)
        {
            _uow.UserRepository.Add(dto.GetModelEntity());
            _uow.Save();
        }

        [HttpPut]
        // PUT api/values/5
        public IActionResult Put([FromBody] UserDto dto)
        {
            var user = _uow.UserRepository.GetById(dto.Id).GetDto();
            if (user == null)
            {
                return BadRequest();
            }

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;

            return Ok();
        }

        [HttpDelete]
        // DELETE api/values/5
        public void Delete([FromQuery] string id)
        {
            var user = _uow.UserRepository.GetById(Guid.Parse(id));
            if (user != null)
                _uow.UserRepository.Delete(user);
        }
    }
}