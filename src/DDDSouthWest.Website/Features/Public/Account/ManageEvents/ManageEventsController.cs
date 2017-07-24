﻿using System;
using System.Linq;
using System.Threading.Tasks;
using DDDSouthWest.Domain.Features.Account.ManageEvents.CreateEvent;
using DDDSouthWest.Website.Framework;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DDDSouthWest.Website.Features.Public.Account.ManageEvents
{
    [Authorize(Policy = AccessPolicies.OrganiserAccessPolicy)]
    public class ManageEventsController : Controller
    {
        private readonly IMediator _mediator;

        public ManageEventsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [Route("/account/events/", Name = RouteNames.EventsManage)]
        public IActionResult Index()
        {
            return View();
        }
        
        [Route("/account/events/create", Name = RouteNames.EventCreate)]
        public IActionResult Create()
        {
            return View(new ManageEventsViewModel());
        }
        
        [HttpPost]
        [Route("/account/events/create")]
        public async Task<IActionResult> Create(CreateEvent.Command command)
        {
            CreateEvent.Response result;

            try
            {
                result = await _mediator.Send(command);
            }
            catch (ValidationException e)
            {
                return View(new ManageEventsViewModel
                {
                    Errors = e.Errors.ToList(),
                    EventDate = command.EventDate,
                    EventFilename = command.EventFilename,
                    EventName = command.EventName
                });
            }
            
            /*return RedirectToAction("Edit", new BlogPostEdit.Query { Id = id.Id });*/
            return RedirectToAction("Edit", result.Id);
        }

        [Route("/account/events/edit/{id}", Name = RouteNames.EventEdit)]
        public IActionResult Edit(int id)
        {
            // TODO: Pull data from database

            return View(new ManageEventsViewModel
            {
                Id = id
            });
        }

        [HttpPost]
        [Route("/account/events/edit")]
        public async Task<IActionResult> Edit(CreateEvent.Command command)
        {
            var result = await _mediator.Send(command);
            
            /*return RedirectToAction("Edit", new BlogPostEdit.Query { Id = id.Id });*/
            return RedirectToAction("Edit", result.Id);
        }
    }
}