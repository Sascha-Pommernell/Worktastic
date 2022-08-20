﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Worktastic.Data;
using Worktastic.Models;

namespace Worktastic.Controllers
{
    [Authorize]
    public class JobPostingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JobPostingController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult JobPosting()
        {
            if (User.IsInRole("Admin"))
            {
               var allPostings = _context.JobPostings.ToList();
               return View(allPostings);
            }

            var jobPostingsFromDb = _context.JobPostings.Where(x => User.Identity != null && x.OwnerUsername == User.Identity.Name).ToList();
                        
            return View(jobPostingsFromDb);
        }

        public IActionResult CreateEditJobPosting(int id)
        {
            if(id != 0)
            {
                var jobPostingFromDb = _context.JobPostings.SingleOrDefault(x => x.Id == id);

                if(User.Identity != null && jobPostingFromDb != null && (jobPostingFromDb.OwnerUsername != User.Identity.Name) && !User.IsInRole("Admin"))
                {
                    return Unauthorized();
                }

                if(jobPostingFromDb != null)
                {
                    return View(jobPostingFromDb);
                } else
                {
                    return NotFound();
                }
            }

            return View();
        }

        public IActionResult CreateEditJob(JobPosting jobPosting, IFormFile file)
        {
            if (User.Identity != null) jobPosting.OwnerUsername = User.Identity.Name;

            if(file != null)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var bytes = ms.ToArray();
                    jobPosting.CompanyImage = bytes;
                }
            }

            if(jobPosting.Id == 0)
            {
                // Add new job if not editing
                _context.JobPostings.Add(jobPosting);
            } else
            {
                var jobFromDb = _context.JobPostings.SingleOrDefault(x => x.Id == jobPosting.Id);

                if(jobFromDb == null)
                {
                    return NotFound();
                }

                jobFromDb.JobTitle = jobPosting.JobTitle;
                jobFromDb.JobLocation = jobPosting.JobLocation;
                jobFromDb.Description = jobPosting.Description;
                jobFromDb.Tasks = jobPosting.Tasks;
                jobFromDb.EmployeeProfile = jobPosting.EmployeeProfile;
                jobFromDb.Benefits = jobPosting.Benefits;
                jobFromDb.Salary = jobPosting.Salary;
                jobFromDb.StartDate = jobPosting.StartDate;
                jobFromDb.CompanyName = jobPosting.CompanyName;
                jobFromDb.ContactPhone = jobPosting.ContactPhone;
                jobFromDb.ConntactPerson = jobPosting.ConntactPerson;
                jobFromDb.ContactMail = jobPosting.ContactMail;
                jobFromDb.ContactWebsite = jobPosting.ContactWebsite;
                jobFromDb.CompanyImage = jobPosting.CompanyImage;
                jobFromDb.OwnerUsername = jobPosting.OwnerUsername;
            }

            _context.SaveChanges();

            return RedirectToAction("JobPosting");
        }

        [HttpPost]
        public IActionResult DeleteJobPostingById(int id)
        {
            if (id == 0)
                return BadRequest();

            var jobPostingFromDb = _context.JobPostings.SingleOrDefault(x => x.Id == id);

            if (jobPostingFromDb == null)
                return NotFound();

            _context.JobPostings.Remove(jobPostingFromDb);
            _context.SaveChanges();

            return Ok();
        }
    }
}