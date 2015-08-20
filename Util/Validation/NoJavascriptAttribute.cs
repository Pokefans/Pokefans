// Copyright 2015 the pokefans-core authors. See copying.md for legal info.

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Pokefans.Util.Validation
{
    // TODO: Better check

    /// <summary>
    /// Attribute to check for javascript code in contents
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    sealed public class NoJavascriptAttribute : ValidationAttribute
    {
        private readonly string[] _invalidTokens = new string[]
        {
            "javascript", "<script", ".js", "onabort", "onafterprintNew", "onbeforeprintNew", "onbeforeunloadNew", "onblur", "oncanplayNew",
            "oncanplaythroughNew", "onchange", "onclick", "oncontextmenuNew", "ondblclick", "ondragendNew", "ondragenterNew", "ondragleaveNew",
            "ondragNew", "ondragoverNew", "ondragstartNew", "ondropNew", "ondurationchangeNew", "onemptiedNew", "onendedNew", "onerror",
            "onerrorNew", "onerrorNew", "onfocus", "onformchangeNew", "onforminputNew", "onhaschangeNew", "oninputNew", "oninvalidNew",
            "onkeydown", "onkeypress", "onkeyup", "onload", "onloadeddataNew", "onloadedmetadataNew", "onloadstartNew", "onmessageNew",
            "onmousedown", "onmousemove", "onmouseout", "onmouseover", "onmouseup", "onmousewheelNew", "onofflineNew", "ononlineNew",
            "onpagehideNew", "onpageshowNew", "onpauseNew", "onplayingNew", "onplayNew", "onpopstateNew", "onprogressNew", "onratechangeNew",
            "onreadystatechangeNew", "onredoNew", "onreset", "onresizeNew", "onscrollNew", "onseekedNew", "onseekingNew", "onselect",
            "onstalledNew", "onstorageNew", "onsubmit", "onsuspendNew", "ontimeupdateNew", "onundoNew", "onunload", "onvolumechangeNew",
            "onwaitingNew", "xmlhttp"
        };

        public override bool IsValid(object value)
        {
            var content = value as string;

            if (content == null)
            {
                return true;
            }

            return !_invalidTokens.Any(content.Contains);
        }
    }
}
