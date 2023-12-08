using Menchul.Base.Constants;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace Menchul.MCode.Infrastructure.Security
{
    public class PolicyKeeper
    {
        private readonly Dictionary<string, AuthorizationPolicy> _policies = new();

        public static string ClientRoleName(string appName, string roleName) => $"{appName}::{roleName}";

        public PolicyKeeper(string appName)
        {
            string ClientRole(string roleName) => ClientRoleName(appName, roleName);

            AddMRolePolicy(Policies.RoleCreateProduct, ClientRole("create_product"));
            AddMRolePolicy(Policies.RoleUpdateProduct, ClientRole("update_product"));
            AddMRolePolicy(Policies.RoleDeleteProduct, ClientRole("delete_product"));

            AddMRolePolicy(Policies.RoleCreatePassport, ClientRole("create_passport"));

            AddMRolePolicy(Policies.RoleCreatePackage, ClientRole("create_package"));

            AddScopePolicy(Policies.ScopeMCode, ApiScopes.MCodeAccess);

            AddMRolePolicy(Policies.RoleAdmin, "admin");
            AddMRolePolicy(Policies.RoleResourceAdmin, ClientRole("admin"));
            AddMRolePolicy(Policies.RoleCreateClientCompany, ClientRole("create_client-company"));
        }

        private void AddMRolePolicy(string policyName, string roleName)
        {
            var policy = new AuthorizationPolicyBuilder()
                .RequireRole(roleName).Build();

            _policies.Add(policyName, policy);
        }

        private void AddScopePolicy(string policyName, string scopeName)
        {
            var policy = new AuthorizationPolicyBuilder()
                .RequireClaim(UserClaimTypes.Scopes, scopeName).Build();

            _policies.Add(policyName, policy);
        }

        public void RegisterPolicies(AuthorizationOptions options)
        {
            foreach (var policyPair in _policies)
            {
                options.AddPolicy(policyPair.Key, policyPair.Value);
            }
        }
    }
}