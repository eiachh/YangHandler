﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using YangHandler.Interpreter;

namespace YangHandlerTool
{
    public static class YangTypes
    {
        private static Dictionary<string,TypeNode> DefinedTypes = new Dictionary<string,TypeNode>();

        static Dictionary<string, string> DisplayNameDict = new Dictionary<string, string>
        {
            {"binary", "binary"},
            {"bits", "bits"},
            {"boolean", "boolean"},
            {"decimal64", "decimal64"},
            {"empty", "empty"},
            {"enumeration", "enumeration"},
            {"identityref", "identityref"},
            {"instance_identifier", "instance-identifier"},
            {"int8", "int8"},
            {"int16", "int16"},
            {"int32", "int32"},
            {"int64", "int64"},
            {"leafref", "leafref"},
            {"string", "string"},
            {"uint8", "uint8"},
            {"uint16", "uint16"},
            {"uint32", "uint32"},
            {"uint64", "uint64"},
            {"union", "union"}

        };

        public static void AddNewYangType(string typekey, string typedisplayname,int line, YangNode tracer)
        {
            typekey = typekey.Replace("-", "_");
            if(!DisplayNameDict.TryAdd(typekey, typedisplayname))
            {
                InterpretationErrorHandler interpreterr = new InterpretationErrorHandler(
                                                          "Multiple type declaration.", 
                                                          "Type: " + typekey + " cannot be added baceuse it was already declared before", 
                                                          line, tracer);
            }
        }

        public static string GetDisplayName(string typekey)
        {
            typekey = typekey.Replace("-", "_");

            string _DisplayName = string.Empty;
            if(DisplayNameDict.TryGetValue(typekey, out _DisplayName))
            {
                return _DisplayName;
            }
            else
            {
                return null;
            }
        }

        public static TypeNode GetType(string typekey)
        {
            typekey = typekey.Replace("-", "_");
            TypeNode retnode;
            DefinedTypes.TryGetValue(typekey, out retnode);
            return retnode;
        }

        /*public static string GetDisplayName(YangBuiltInTypes Type)
        {
            return GetDisplayName(Type.ToString());
        }*/

        public static bool IsValidType(string typekey)
        {
            string outstr;
            return DisplayNameDict.TryGetValue(typekey, out outstr);
        }

        public static void BindType(string typekey,TypeNode node)
        {
            if (!DefinedTypes.TryAdd(typekey, node))
            {
                throw new TypebindingError("Type cannot be binded as \""+node.Name+"\" it already exists.");
            }
        }
    }

    /// <summary>
    /// This is the enum for yang built in types. This enum should be used with GetDisplayName() only
    /// </summary>
   /* public enum YangBuiltInTypes
    {
        [Display(Name = "binary")] binary,
        [Display(Name = "bits")] bits,
        [Display(Name = "boolean")] boolean,
        [Display(Name = "decimal64")] decimal64,
        [Display(Name = "empty")] empty,
        [Display(Name = "enumeration")] enumeration,
        [Display(Name = "identityref")] identityref,
        [Display(Name = "instance-identifier")] instance_identifier,
        [Display(Name = "int8")] int8,
        [Display(Name = "int16")] int16,
        [Display(Name = "int32")] int32,
        [Display(Name = "int64")] int64,
        [Display(Name = "leafref")] leafref,
        [Display(Name = "string")] YangString,
        [Display(Name = "uint8")] uint8,
        [Display(Name = "uint16")] uint16,
        [Display(Name = "uint32")] uint32,
        [Display(Name = "uint64")] uint64,
        [Display(Name = "union")] union
    }*/
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetName();
        }
        /*public static YangBuiltInTypes GetValueFromName(string name)
        {
            var type = typeof(YangBuiltInTypes);
            if (!type.IsEnum) throw new InvalidOperationException();

            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DisplayAttribute)) as DisplayAttribute;
                if (attribute != null)
                {
                    if (attribute.Name == name)
                    {
                        return (YangBuiltInTypes)field.GetValue(null);
                    }
                }
                else
                {
                    if (field.Name == name)
                        return (YangBuiltInTypes)field.GetValue(null);
                }
            }

            throw new ArgumentOutOfRangeException("name");
        }*/
    }
}
