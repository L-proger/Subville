//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v13.6.1.0 (NJsonSchema v10.1.21.0 (Newtonsoft.Json v12.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------

#pragma warning disable 108 // Disable "CS0108 '{derivedDto}.ToJson()' hides inherited member '{dtoBase}.ToJson()'. Use the new keyword if hiding was intended."
#pragma warning disable 114 // Disable "CS0114 '{derivedDto}.RaisePropertyChanged(String)' hides inherited member 'dtoBase.RaisePropertyChanged(String)'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword."
#pragma warning disable 472 // Disable "CS0472 The result of the expression is always 'false' since a value of type 'Int32' is never equal to 'null' of type 'Int32?'
#pragma warning disable 1573 // Disable "CS1573 Parameter '...' has no matching param tag in the XML comment for ...
#pragma warning disable 1591 // Disable "CS1591 Missing XML comment for publicly visible type or member ..."

namespace Server
{
    using System = global::System;
    
    [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.6.1.0 (NJsonSchema v10.1.21.0 (Newtonsoft.Json v12.0.0.0))")]
    public interface ISubvilleApiController
    {
        /// <summary>List existing repositories</summary>
        /// <param name="_limit">Max repositories in response</param>
        /// <param name="_skip">Skip first N repositories</param>
        /// <returns>A list of repositories</returns>
        System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.ActionResult<RepositoryDescriptionArray>> ListRepositoriesAsync(int? _limit, int? _skip);
    
        /// <summary>Login user and return token</summary>
        /// <returns>Auth token</returns>
        System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.ActionResult<TokenResponse>> GetTokenAsync(LoginRequest body);
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.6.1.0 (NJsonSchema v10.1.21.0 (Newtonsoft.Json v12.0.0.0))")]
    [Microsoft.AspNetCore.Mvc.Route("api/v1")]
    public partial class SubvilleApiController : Microsoft.AspNetCore.Mvc.Controller
    {
        private ISubvilleApiController _implementation;
    
        public SubvilleApiController(ISubvilleApiController implementation)
        {
            _implementation = implementation;
        }
    
        /// <summary>List existing repositories</summary>
        /// <param name="_limit">Max repositories in response</param>
        /// <param name="_skip">Skip first N repositories</param>
        /// <returns>A list of repositories</returns>
        [Microsoft.AspNetCore.Mvc.HttpGet, Microsoft.AspNetCore.Mvc.Route("repository/")]
        public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.ActionResult<RepositoryDescriptionArray>> ListRepositories([Microsoft.AspNetCore.Mvc.FromQuery] int? _limit, [Microsoft.AspNetCore.Mvc.FromQuery] int? _skip)
        {
            return _implementation.ListRepositoriesAsync(_limit, _skip);
        }
    
        /// <summary>Login user and return token</summary>
        /// <returns>Auth token</returns>
        [Microsoft.AspNetCore.Mvc.HttpPost, Microsoft.AspNetCore.Mvc.Route("token/")]
        public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.ActionResult<TokenResponse>> GetToken([Microsoft.AspNetCore.Mvc.FromBody] LoginRequest body)
        {
            return _implementation.GetTokenAsync(body);
        }
    
    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.21.0 (Newtonsoft.Json v12.0.0.0)")]
    public partial class LoginRequest 
    {
        [Newtonsoft.Json.JsonProperty("password", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Password { get; set; }
    
        [Newtonsoft.Json.JsonProperty("user_name", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string User_name { get; set; }
    
        private System.Collections.Generic.IDictionary<string, object> _additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [Newtonsoft.Json.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }
    
        public string ToJson() 
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    
        public static LoginRequest FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<LoginRequest>(data);
        }
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.21.0 (Newtonsoft.Json v12.0.0.0)")]
    public partial class TokenResponse 
    {
        [Newtonsoft.Json.JsonProperty("token", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Token { get; set; }
    
        [Newtonsoft.Json.JsonProperty("user_name", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string User_name { get; set; }
    
        private System.Collections.Generic.IDictionary<string, object> _additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [Newtonsoft.Json.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }
    
        public string ToJson() 
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    
        public static TokenResponse FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<TokenResponse>(data);
        }
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.21.0 (Newtonsoft.Json v12.0.0.0)")]
    public partial class RepositoryDescription 
    {
        [Newtonsoft.Json.JsonProperty("name", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Name { get; set; }
    
        [Newtonsoft.Json.JsonProperty("url", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Url { get; set; }
    
        private System.Collections.Generic.IDictionary<string, object> _additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [Newtonsoft.Json.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }
    
        public string ToJson() 
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    
        public static RepositoryDescription FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<RepositoryDescription>(data);
        }
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.21.0 (Newtonsoft.Json v12.0.0.0)")]
    public partial class RepositoryDescriptionArray 
    {
        [Newtonsoft.Json.JsonProperty("has_more", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool Has_more { get; set; }
    
        [Newtonsoft.Json.JsonProperty("total_results", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int Total_results { get; set; }
    
        [Newtonsoft.Json.JsonProperty("data", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.List<RepositoryDescription> Data { get; set; }
    
        private System.Collections.Generic.IDictionary<string, object> _additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [Newtonsoft.Json.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }
    
        public string ToJson() 
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    
        public static RepositoryDescriptionArray FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<RepositoryDescriptionArray>(data);
        }
    
    }

}

#pragma warning restore 1591
#pragma warning restore 1573
#pragma warning restore  472
#pragma warning restore  114
#pragma warning restore  108