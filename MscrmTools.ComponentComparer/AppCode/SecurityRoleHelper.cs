using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Linq;

namespace MscrmTools.ComponentComparer.AppCode
{
    public static class SecurityRoleHelper
    {
        public static string GetRolePrivileges(Guid roleId, string roleName, IOrganizationService service)
        {
            if (roleId == Guid.Empty)
            {
                var role = service.RetrieveMultiple(new QueryExpression("role")
                {
                    NoLock = true,
                    Criteria = new FilterExpression
                    {
                        Conditions =
                    {
                        new ConditionExpression("name", ConditionOperator.Equal, roleName),
                        new ConditionExpression("parentroleid", ConditionOperator.Null)
                    }
                    }
                }).Entities.FirstOrDefault();

                if (role == null)
                {
                    throw new Exception($"Role '{roleName}' cannot be found");
                }

                roleId = role.Id;
            }

            var request = new RetrieveRolePrivilegesRoleRequest
            {
                RoleId = roleId
            };

            var response = (RetrieveRolePrivilegesRoleResponse)service.Execute(request);
            var orderedList = response.RolePrivileges.OrderBy(o => o.PrivilegeName);

            var privileges = service.RetrieveMultiple(new QueryExpression("privilege")
            {
                NoLock = true,
                ColumnSet = new ColumnSet("name"),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression("privilegeid", ConditionOperator.In, orderedList.Select(p =>p.PrivilegeId).ToArray()),
                    }
                }
            }).Entities;

            return string.Join(Environment.NewLine, orderedList.Select(p => $"{privileges.First(p2 => p2.Id == p.PrivilegeId)["name"]}:{p.Depth}").OrderBy(p => p));
        }
    }
}