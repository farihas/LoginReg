using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LoginReg.Models;
using Microsoft.AspNetCore.Identity;

namespace LoginReg.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyDbContext _dbContext;
        public HomeController(MyDbContext contextService)
        {
            _dbContext = contextService;
        }

        // Other code
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            List<User> AllUsers = _dbContext.Users.ToList();
            return View();
        }

        [HttpPost("register")] 
        public IActionResult Register(User user)  // form method to add a new user
        {
            // When creating a new user, render a view with errors if the submission is invalid,
            if(ModelState.IsValid != true)
            {
                return View("Index", user);
            }
            var test = _dbContext.Users.FirstOrDefault(u => u.Email == user.Email);
            // check if email already in db
            if (test != null)
            {
                ModelState.AddModelError("Email", "Email already exists!");
                return View("Index", user);
            }
            // process the form...   
            PasswordHasher<User> Hasher = new PasswordHasher<User>();
            user.PasswordHash = Hasher.HashPassword(user, user.Password);            
        
            _dbContext.Users.Add(user);  // adding passed in user object to db
            _dbContext.SaveChanges();    // saving changes to db            
            ViewData["Message"] = "You were successfully registered as User ";  
            
            // redirect to login page if the submission is valid                               
            return RedirectToAction("login", user);
        }

        [HttpGet("login")]
        [HttpPost("login")] 
        public IActionResult Login(User user)
        {   // TODO... var passwordHash = Hasher( user.Password);
            
            // check to see if user exists in db
            //_dbContext.Lo //Users.Load();
            var existingUser = _dbContext.Users.FirstOrDefault(u => u.Email == user.Email);

            if (existingUser == null)  // If user not found
            {                
                ModelState.AddModelError("Email", "Email combination doesn't match.");
                return View("Index", user);
            }

            PasswordHasher<User> pwHasher = new PasswordHasher<User>();
            var result = pwHasher.VerifyHashedPassword(user, existingUser.PasswordHash, user.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("Password", "Password combination doesn't match.");
                return View("Index", user);
            }
            
            ViewData["Message"] = "You were successfully logged in as User ";  
            return View("login", existingUser);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
