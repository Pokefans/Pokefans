// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Pokefans.Data.Attributes
{
    class RegularExpressionWithOptionsAttribute : RegularExpressionAttribute, IClientValidatable
    {
        public RegularExpressionWithOptionsAttribute(string pattern) : base(pattern) { }

        public RegexOptions RegexOptions { get; set; }

        public override bool IsValid(object value)
        {
            if (string.IsNullOrEmpty((string)value))
                return true;

            return Regex.IsMatch((string)value, Pattern, RegexOptions);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule()
            {
                ErrorMessage = FormatErrorMessage(metadata.DisplayName),
                ValidationType = "regex-with-options"
            };

            rule.ValidationParameters["pattern"] = Pattern;

            string flags = String.Empty;

            if((RegexOptions & RegexOptions.Multiline) == RegexOptions.Multiline)
            {
                flags += "m";
            }
            if((RegexOptions & RegexOptions.IgnoreCase) == RegexOptions.IgnoreCase)
            {
                flags += "i";
            }

            rule.ValidationParameters["flags"] = flags;

            yield return rule;
        }
    }
    /*
     * To use this in client for unobtrusive validation, add this script to the corresponding page (note that this should be land somewhere in the SCRIPTPATH which doesn't exist (yet).
     * 
     (function($) {
        $.validator.unobtrusive.adapters.add("regex-with-options", ["pattern","flags"], function (options) {
            options.messages['regex-with-options'] = options.message;
            options.rules['regex-with-options'] = options.params;
        });
        $.validator.addMethod("regex-with-options", function(value, element, params) {
            var match;
            if(this.optional(element)) {
                return true;
            }
     
            var reg = new RegExp(params.pattern, params.flags);
            var match = reg.exec(value);
            return (match && (match.index === 0) && (match[0].length === value.length));
        });
     })(jQuery);
     */

    // inspired by http://stackoverflow.com/questions/4218836/regularexpressionattribute-how-to-make-it-not-case-sensitive-for-client-side-v
}
