using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Linq;
using System.Text;

namespace MscrmTools.ComponentComparer.AppCode
{
    public static class WebresourceHelper
    {
        public static string GetWebresourceContent(Guid id, string name, IOrganizationService service)
        {
            Entity resource;

            if (id != Guid.Empty)
            {
                resource = service.Retrieve("webresource", id, new ColumnSet("content"));
            }
            else
            {
                resource = service.RetrieveMultiple(new QueryExpression("webresource")
                {
                    NoLock = true,
                    ColumnSet = new ColumnSet("content"),
                    Criteria = new FilterExpression
                    {
                        Conditions =
                    {
                        new ConditionExpression("name", ConditionOperator.Equal, name)
                    }
                    }
                }).Entities.FirstOrDefault();
            }

            byte[] binary = Convert.FromBase64String(resource.GetAttributeValue<string>("content"));
            string resourceContent = Encoding.UTF8.GetString(binary);
            string byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
            if (resourceContent.StartsWith("\""))
            {
                resourceContent = resourceContent.Remove(0, byteOrderMarkUtf8.Length);
            }

            return resourceContent;
        }
    }
}