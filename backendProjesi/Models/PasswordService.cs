﻿using Microsoft.AspNetCore.Identity;

namespace backendProjesi.Models
{
    public class PasswordService
    {
        private readonly PasswordHasher<object> _passwordHasher;
        public PasswordService()
        {
            _passwordHasher = new PasswordHasher<object>();
        }
        public string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(null, password);
        }
        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            var result = _passwordHasher.VerifyHashedPassword(null, hashedPassword, providedPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}
