using Microsoft.AspNetCore.Mvc;
using Botanic_Project.Web.Data;
using Botanic_Project.Web.Models;
using Botanic_Project.Web.Models.Entities;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace Botanic_Project.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        public LoginController(ApplicationDbContext dbContext) //Dependency Injection
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Kullanıcıyı veritabanında arayın
                var user = await dbContext.User
                    .FirstOrDefaultAsync(u => u.Email == viewModel.Email);
                if (user != null)
                {
                    // Şifre doğrulama
                    var hashedPassword = user.Password;
                    var isPasswordCorrect = VerifyPassword(viewModel.Password, hashedPassword);

                    if (isPasswordCorrect)
                    {
                        var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, user.Email)
                    };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        var authProperties = new AuthenticationProperties
                        {
                        };

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                                      new ClaimsPrincipal(claimsIdentity),
                                                      authProperties);
                        // Başarılı giriş
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User not found.");
                }
            }

            return View(viewModel);
        }
        private bool VerifyPassword(string enteredPassword, string storedPassword)
        {
            // Şifreyi doğrulamak için, veritabanındaki hashed şifre ile karşılastırma
            try
            {
                byte[] storedSalt = Convert.FromBase64String(storedPassword.Substring(0, 24)); // Salt kısmını alir
                byte[] storedHash = Convert.FromBase64String(storedPassword.Substring(24)); // Hash kısmını alir
                byte[] enteredPasswordHash = KeyDerivation.Pbkdf2(
                    password: enteredPassword,
                    salt: storedSalt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 10000,
                    numBytesRequested: 32);

                return enteredPasswordHash.SequenceEqual(storedHash);
            }
            catch
            {
                return false;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout(User viewModel)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        //REGISTER
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {   if (ModelState.IsValid) {
                string hashedPassword = HashPassword(viewModel.Password);
                var user = new User
                {
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    Email = viewModel.Email,
                    Password = hashedPassword,
                    UserName = viewModel.UserName,
                };
                await dbContext.User.AddAsync(user);
                await dbContext.SaveChangesAsync();
            }

            return View();
        }
        private string HashPassword(string password)
        {
            // Yeni bir salt olusturur
            byte[] salt = new byte[16];
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            // sifreyi hash'ler
            byte[] hash = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 32);

            // Salt ve hash'i birleştirir
            string saltBase64 = Convert.ToBase64String(salt);
            string hashBase64 = Convert.ToBase64String(hash);

            // Salt ve hash birlestirilerek geri döner
            return $"{saltBase64}{hashBase64}";
        }
    }
}
