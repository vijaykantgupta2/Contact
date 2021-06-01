using ContactAPI.Helpers;
using ContactAPI.Models;
using ContactAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
//using logger

namespace ContactAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : BaseController
    {
        private readonly ILoggerManager Logger;
        private readonly IUnitOfWork UoW;
        public ContactController(IUnitOfWork unitOfWork, ILoggerManager logger)
        {
            this.UoW = unitOfWork;
            this.Logger = logger;
        }

        // GET: api/Contact/GetContacts
        [Route("GetContacts")]
        [HttpGet]
        [ActionName("GetContacts")]
        public async Task<IEnumerable<Contact>> Get()
        {
            try
            {
                Logger.LogInfo("Fetching all contacts from the database");
                return await UoW.Contacts.GetAll();
            }
            catch(Exception ex)
            {
                Logger.LogError($"Something went wrong: {ex}");
                return (IEnumerable<Contact>)StatusCode(500, "Internal server error");
            }
        }

        // GET api/Contact/GetContactById/{id}
        [Route("GetContactById")]
        [HttpGet]
        [ActionName("GetContactById")]
        public async Task<Contact> Get(int id)
        {
            try
            {
                Logger.LogInfo("Fetching contact from the database");
                return await UoW.Contacts.Get(id);
            }
            catch(Exception ex)
            {
                Logger.LogError($"Something went wrong: {ex}");
                return (Contact)StatusCode(500, "Internal server error");
            }
        }

        // POST api/Contact/AddContact
        [Route("AddContact")]
        [HttpPost]
        [ActionName("AddContact")]
        public IActionResult Post([Bind("Id,FirstName,LastName,Email,Phone,Status")] Contact contact)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Logger.LogInfo("Adding new contact to database");
                    UoW.Contacts.Add(contact);
                    UoW.Complete();
                    return Ok();
                }
                else
                {
                    Logger.LogError("Something went wrong");
                    return BadRequest();
                }
            }
            catch(Exception ex)
            {
                Logger.LogError($"Something went wrong: {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT api/Contact/UpdateContact
        [Route("UpdateContact")]
        [HttpPut]
        [ActionName("UpdateContact")]
        public void Put(Contact contact)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Logger.LogInfo("Updating an existing contact");
                    UoW.Contacts.Update(contact);
                    UoW.Complete();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"Something went wrong: {ex}");
            }
        }

        // DELETE api/Contact/DeleteContact/{id}
        [Route("DeleteContact")]
        [HttpDelete]
        [ActionName("DeleteContact")]
        public void Delete(int id)
        {
            try
            {
                Logger.LogInfo("Deleting an existing contact");
                UoW.Contacts.Delete(id);
                UoW.Complete();
            }
            catch (Exception ex)
            {
                Logger.LogError($"Something went wrong: {ex}");
            }
        }

    }
}
