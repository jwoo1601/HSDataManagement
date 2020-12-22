using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using HyosungManagement.Data;
using HyosungManagement.Filters;
using HyosungManagement.InputModels;
using HyosungManagement.Logging;
using HyosungManagement.Models;
using HyosungManagement.Models.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace HyosungManagement.Controllers.Apis
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [RoleRequirement("Admin", "Master")]
    [Route("api/customers")]
    [ApiController]
    public class CustomersApiController : ApiControllerBase
    {
        AppDbContext Context { get; }
        ILogger<Customer> Logger { get; }
        IStringLocalizer<CustomersApiController> Localizer { get; }
        UserManager<HSMUser> UserManager { get; }

        public CustomersApiController(
            AppDbContext context,
            ILogger<Customer> logger,
            IStringLocalizer<CustomersApiController> localizer,
            UserManager<HSMUser> userManager
        )
        {
            Context = context;
            Logger = logger;
            Localizer = localizer;
            UserManager = userManager;
        }

        // GET /api/customers
        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCustomersAsync()
        {
            var customers = await Context.Customers
                                                    .ToListAsync();

            Logger.LogInformation(
                CustomersApiLogEvents.GetAllCustomers,
                "Successfully retrieved customer list - numCustomers: {numCustomers}.",
                customers.Count
            );

            return Ok(customers);
        }

        // GET /api/customers/{id}
        [HttpGet("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCustomerByIDAsync(int id)
        {
            var foundCustomer = await Context.Customers
                                                        .SingleOrDefaultAsync(c => c.ID == id);
            if (foundCustomer == null)
            {
                Logger.LogError(
                    CustomersApiLogEvents.CustomerNotFound,
                    "Failed to retrieve customer with id {ID}: customer not found.",
                    id
                );

                return NotFound(Localizer["customer.notFound"]);
            }

            Logger.LogInformation(
                CustomersApiLogEvents.GetCustomerByID,
                "Successfully retrieved customer {Customer}",
                foundCustomer
            );

            return Ok(foundCustomer);
        }

        // POST /api/customers
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddCustomerAsync(
            [FromBody] CustomerInputModel inputModel
        )
        {
            try
            {
                if (inputModel.Discharged && !inputModel.DischargeDate.HasValue)
                {
                    ModelState.AddModelError("discharge_date", Localizer["dischargeDate.required"]);

                    return BadRequest(ModelState);
                }

                var customer = await inputModel.SaveAsEntityAsync(null, Context, HttpContext.RequestServices);

                Logger.LogInformation(
                    CustomersApiLogEvents.AddCustomer,
                    "Successfully added customer {Customer} with inputModel {InputModel}.",
                    customer,
                    inputModel
                );

                return Created(customer);
            }
            catch (DbUpdateException e)
            {
                Logger.LogError(
                    CustomersApiLogEvents.AddCustomerError,
                    e,
                    "Failed to add customer with inputModel {InputModel}.",
                    inputModel
                );

                return ServerError(Localizer["customer.dbFailure"]);
            }
        }

        // PUT /api/customers/{id}
        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditCustomerAsync(
            int id,
            [FromBody] CustomerInputModel inputModel
        )
        {
            try
            {
                if (inputModel.Discharged && !inputModel.DischargeDate.HasValue)
                {
                    ModelState.AddModelError("discharge_date", Localizer["dischargeDate.required"]);

                    return BadRequest(ModelState);
                }

                var foundCustomer = await Context.Customers
                                .SingleOrDefaultAsync(c => c.ID == id);
                if (foundCustomer == null)
                {
                    Logger.LogError(
                        CustomersApiLogEvents.CustomerNotFound,
                        "Failed to edit customer with id {ID} and {InputModel}: customer not found.",
                        id,
                        inputModel
                    );

                    return NotFound(Localizer["customer.notFound"]);
                }

                var customer = await inputModel.SaveAsEntityAsync(
                    foundCustomer.ID,
                    Context,
                    HttpContext.RequestServices
                );

                Logger.LogInformation(
                    CustomersApiLogEvents.EditCustomer,
                    "Successfully edited customer {Customer} with inputModel {InputModel}.",
                    customer,
                    inputModel
                );

                return Ok(customer);
            }
            catch (DbUpdateException e)
            {
                Logger.LogError(
                    CustomersApiLogEvents.EditCustomerError,
                    e,
                    "Failed to edit customer {ID} with inputModel {InputModel}.",
                    id,
                    inputModel
                );

                return ServerError(Localizer["customer.dbFailure"]);
            }
        }

        // PATCH /api/customers/{id}
        [HttpPatch("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SetOptions(
            int id,
            [FromBody] CustomerSetOptionsInputModel inputModel
        )
        {
            try
            {
                var foundCustomer = await Context.Customers.SingleOrDefaultAsync(c => c.ID == id);
                if (foundCustomer == null)
                {
                    return NotFound(Localizer["customer.notFound"]);
                }

                if (inputModel.Visible.HasValue)
                {
                    foundCustomer.IsHidden = !inputModel.Visible.Value;
                }
                if (inputModel.Discharged.HasValue)
                {
                    if (inputModel.Discharged.Value && inputModel.DischargeDate.HasValue)
                    {
                        foundCustomer.IsDischarged = inputModel.Discharged.Value;
                        foundCustomer.DischargeDate = inputModel.DischargeDate.Value;
                    }
                    else if (!inputModel.Discharged.Value)
                    {
                        foundCustomer.IsDischarged = inputModel.Discharged.Value;
                        foundCustomer.DischargeDate = null;
                    }
                    else
                    {
                        ModelState.AddModelError("discharge_date", Localizer["dischargeDate.required"]);
                    }
                }

                if (ModelState.IsValid)
                {
                    var saved = Context.Update(foundCustomer);
                    await Context.SaveChangesAsync();

                    Logger.LogInformation(
                        CustomersApiLogEvents.SetOptions,
                        "Successfully set options of customer {Customer} with inputModel {InputModel}.",
                        foundCustomer,
                        inputModel
                    );

                    return Ok(saved.Entity);
                }

                return BadRequest(ModelState);
            }
            catch (DbUpdateException e)
            {
                Logger.LogError(
                    CustomersApiLogEvents.SetOptionsError,
                    e,
                    "Failed to set options of customer {ID} with inputModel {InputModel}.",
                    id,
                    inputModel
                );

                return ServerError(Localizer["customer.dbFailure"]);
            }
        }

        // PATCH /api/customers/{id}/services/
        [HttpPatch("{id}/services")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AssignServicesAsync(
            int id,
            [FromBody] CustomerAssignServicesInputModel inputModel
        )
        {
            try
            {
                var foundCustomer = await Context.Customers

                                                    .SingleOrDefaultAsync(c => c.ID == id);
                if (foundCustomer == null)
                {
                    return NotFound(Localizer["customer.notFound"]);
                }

                var entity = await inputModel.SaveAsEntityAsync(
                    foundCustomer.ID,
                    Context,
                    HttpContext.RequestServices
                );

                Logger.LogInformation(
                    CustomersApiLogEvents.AssignServices,
                    "Successfully assign services customer {Customer} with inputModel {InputModel}.",
                    foundCustomer,
                    inputModel
                );

                return Ok(entity);
            }
            catch (DbUpdateException e)
            {
                Logger.LogError(
                    CustomersApiLogEvents.AssignServicesError,
                    e,
                    "Failed to assign services for customer {ID} with inputModel {InputModel}.",
                    id,
                    inputModel
                );

                return ServerError(Localizer["customer.dbFailure"]);
            }
        }

        // DELETE /api/customers/{id}
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCustomerAsync(int id)
        {
            var foundCustomer = await Context.Customers
                                              .SingleOrDefaultAsync(c => c.ID == id);
            if (foundCustomer == null)
            {
                return NotFound(Localizer["customer.notFound"]);
            }

            try
            {
                foundCustomer.IsDeleted = true; // soft-delete
                //Context.Customers.Remove(foundCustomer);
                await Context.SaveChangesAsync();

                Logger.LogInformation(
                    CustomersApiLogEvents.DeleteCustomer,
                    "Successfully deleted customer {Customer}.",
                    foundCustomer
                );

                return Ok();
            }
            catch (DbUpdateException e)
            {
                Logger.LogError(
                    CustomersApiLogEvents.DeleteCustomerError,
                    e,
                    "Failed to delete customer {ID}.",
                    id
                );

                return ServerError(Localizer["customer.deleteFailure"]);
            }
        }


        private async Task SaveDistinctTags(Customer customer, IEnumerable<string> inputTags)
        {
            foreach (var tag in inputTags.Distinct())
            {
                var formattedTagName = tag.Trim();

                var boundTag = await Context.CustomerTags.FirstOrDefaultAsync(
                    t => t.Name.Equals(formattedTagName)
                );
                if (boundTag == null)
                {
                    var newTag = await Context.CustomerTags.AddAsync(
                        new CustomerTag
                        {
                            Name = formattedTagName
                        }
                    );
                    await Context.SaveChangesAsync();

                    boundTag = newTag.Entity;
                }

                await Context.CustomerTagAssignments.AddAsync(
                    new CustomerTagAssignment
                    {
                        CustomerID = customer.ID,
                        CustomerTagID = boundTag.ID
                    }
                );
                await Context.SaveChangesAsync();
            }
        }
    }
}
