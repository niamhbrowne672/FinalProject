﻿using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace FinalProject.Web;

/// <summary>
/// Condition tag helper
/// 
/// </summary>
[HtmlTargetElement(Attributes = "asp-condition")] //nameof(Condition))]
public class ConditionTagHelper : TagHelper
{
    [HtmlAttributeName("asp-condition")]
    public bool Condition { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (!Condition)
        {
            output.SuppressOutput();
        }
    }
}


