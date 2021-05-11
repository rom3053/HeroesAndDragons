using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Concurrent;

namespace HeroesAndDragons.JWT
{
    //public class JwtAuthManager
    //{
    //    public IImmutableDictionary<string, RefreshToken> UsersRefreshTokensReadOnlyDictionary => _usersRefreshTokens.ToImmutableDictionary();
    //    private readonly ConcurrentDictionary<string, RefreshToken> _usersRefreshTokens;
    //    private readonly JwtTokenConfig _jwtTokenConfig;
    //    private readonly byte[] _secret;

    //    public JwtAuthManager(JwtTokenConfig jwtTokenConfig)
    //    {
    //        _jwtTokenConfig = jwtTokenConfig;
    //        _usersRefreshTokens = new ConcurrentDictionary<string, RefreshToken>();
    //        _secret = Encoding.ASCII.GetBytes(jwtTokenConfig.Secret);
    //    }
    //    public void RemoveExpiredRefreshToken(DateTime now)
    //    {
    //        var expiredTokens = _usersRefreshTokens.Where(x => x.Value.ExpireAt < now).ToList();
    //        foreach (var expiredToken in expiredTokens)
    //        {
    //            _usersRefreshTokens.TryRemove(expiredToken.Key, out _);
    //        }
    //    }
    //    public void RemoveExpiredRefreshTokenByUserName(string userName)
    //    {
    //        var expiredTokens = _usersRefreshTokens.Where(x => x.Value.UserName == userName).ToList();
    //        foreach (var expiredToken in expiredTokens)
    //        {
    //            _usersRefreshTokens.TryRemove(expiredToken.Key, out _);
    //        }
    //    }
    //    public JwtAuthResult GenerateTokens(string username, Claim[] claims, DateTime now)
    //    {
    //        var shouldAddAudienceClaim = string.IsNullOrWhiteSpace(claims?.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Aud)?.Value);
    //        var jwtToken = new JwtSecurityToken(
    //            _jwtTokenConfig.Issuer,
    //            shouldAddAudienceClaim ? _jwtTokenConfig.Audience : string.Empty,
    //            claims,
    //            expires: now.AddMinutes(_jwtTokenConfig.AccessTokenExpiration),
    //            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(_secret), SecurityAlgorithms.HmacSha256Signature));
    //        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

    //        var refreshToken = new RefreshToken
    //        {
    //            UserName = username,
    //            TokenString = GenerateRefreshTokenString(),
    //            ExpireAt = now.AddMinutes(_jwtTokenConfig.AccessTokenExpiration)
    //        };
    //        _usersRefreshTokens.AddOrUpdate(refreshToken.TokenString, refreshToken, (_, _) => refreshToken);
    //    }
    //}
    public class JwtAuthResult
    {
        public string AccessToken { get; set; }
        public RefreshToken RefreshToken { get; set; }
    }
    public class RefreshToken
    {
        public string UserName { get; set; }
        public string TokenString { get; set; }
        public DateTime ExpireAt { get; set; }

    }
}
