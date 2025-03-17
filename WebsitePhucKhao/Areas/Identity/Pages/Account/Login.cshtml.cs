// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using WebsitePhucKhao.Models;
using Microsoft.EntityFrameworkCore;

namespace WebsitePhucKhao.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly ApplicationDbContext _context;

        public LoginModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILogger<LoginModel> logger, ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _context = context;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Required]
            [Display(Name = "Chọn vai trò")]
            public string Role { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);

                if (user == null)
                {
                    switch (Input.Role)
                    {
                        case "SinhVien":
                            var sinhVien = await _context.SinhViens.FirstOrDefaultAsync(sv => sv.Email == Input.Email);
                            if (sinhVien != null)
                            {
                                user = new ApplicationUser
                                {
                                    UserName = sinhVien.Email,
                                    Email = sinhVien.Email,
                                    MaSinhVien = sinhVien.MaSinhVien
                                };

                                var createResult = await _userManager.CreateAsync(user, sinhVien.MaSinhVien.ToString());
                                if (!createResult.Succeeded)
                                {
                                    foreach (var error in createResult.Errors)
                                        ModelState.AddModelError(string.Empty, error.Description);
                                    return Page();
                                }
                            }
                            break;

                        case "GiangVien":
                            var giangVien = await _context.GiangViens.FirstOrDefaultAsync(gv => gv.Email == Input.Email);
                            if (giangVien != null)
                            {
                                user = new ApplicationUser
                                {
                                    UserName = giangVien.Email,
                                    Email = giangVien.Email,
                                    MaGiangVien = giangVien.MaGiangVien
                                };

                                var createResult = await _userManager.CreateAsync(user, giangVien.MaGiangVien.ToString());
                                if (!createResult.Succeeded)
                                {
                                    foreach (var error in createResult.Errors)
                                        ModelState.AddModelError(string.Empty, error.Description);
                                    return Page();
                                }
                            }
                            break;

                        case "NhanVien":
                            var nhanVien = await _context.NhanVienPhongDaoTaos.FirstOrDefaultAsync(nv => nv.Email == Input.Email);
                            if (nhanVien != null)
                            {
                                user = new ApplicationUser
                                {
                                    UserName = nhanVien.Email,
                                    Email = nhanVien.Email,
                                    MaNhanVienPhongDaoTao = nhanVien.MaNhanVienPhongDaoTao
                                };

                                var createResult = await _userManager.CreateAsync(user, nhanVien.MaNhanVienPhongDaoTao.ToString());
                                if (!createResult.Succeeded)
                                {
                                    foreach (var error in createResult.Errors)
                                        ModelState.AddModelError(string.Empty, error.Description);
                                    return Page();
                                }
                            }
                            break;
                    }

                    if (user == null)
                    {
                        ModelState.AddModelError(string.Empty, "Tài khoản không tồn tại. Hãy đăng ký trước.");
                        return Page();
                    }
                }

                // Tiến hành đăng nhập
                var result = await _signInManager.PasswordSignInAsync(user.UserName, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Người dùng đã đăng nhập.");

                    // Điều hướng theo vai trò đã chọn
                    switch (Input.Role)
                    {
                        case "SinhVien":
                            returnUrl = Url.Content("~/SinhVien/Index");
                            break;
                        case "GiangVien":
                            returnUrl = Url.Content("~/GiangVien/Index");
                            break;
                        case "NhanVien":
                            returnUrl = Url.Content("~/NhanVien/Index");
                            break;
                    }

                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("Tài khoản bị khóa.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Đăng nhập không hợp lệ.");
                    return Page();
                }
            }

            return Page();
        }



    }
}
